using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurbineObject : MonoBehaviour, IMouseSensitive, ITouchSensitive, IWindSensitive
{

    //Wind Direction in Vector
    public static Monitored<Vector3> windVelocity = new Monitored<Vector3>(new Vector3(0, 0, 1));

	static List<TurbineObject> _all;

	public static List<TurbineObject> all {
		get {
			if (_all == null) {
				_all = new List<TurbineObject>(FindObjectsOfType<TurbineObject>());
			}
			return _all;
		}
	}

	Vector3 windDirection;

	HashSet<TurbineState> states;
	HashSet<TurbineState> deletionQueue;

    //Maximum Power Avalible from Turbine (in MW)
    float _maxPower = 1;

	#region State Booleans

	public bool isFine {
		get {
			return states.Count == 0;
		}
	}

	public bool isBroken
	{
		get {
			foreach (TurbineState ts in states){
				if(ts.name == TurbineStateManager.brokenState.name) return true;
			}

			return false;
		}
	}

	#endregion

    public float efficencyOvercharge
    {
        get;
        private set;
    }

    float stateMultiplier
    {
        get {
            float multi = 1f;
            if (states != null)
                foreach (TurbineState ts in states)
                {
                    multi *= ts.efficiencyMultiplyer;
                }
            return multi;
        }
    }

    [SerializeField]
    float overchargeIncrease = 0.02f;

    [SerializeField]
    float overchargeDecrease = 0.01f;

	[SerializeField]
	float maxOvercharge = 1f;

    [SerializeField]
    float _turbineDiameter = 4f;
    [SerializeField]
    float _turbineHeight = 8f;

    bool isCharging;
    bool hasOverchargeDanger;

    [SerializeField]
    bool _debug;

    [SerializeField]
    Vector3 raycastPoint;

    public float currentEfficency { get; private set; }

    [SerializeField]
    Image powerHUD;

    // Use this for initialization
    void Start()
    {
		if(_all != null){
			_all.Add(this);
		}

        states = new HashSet<TurbineState>();
		deletionQueue = new HashSet<TurbineState>();
		windDirection = windVelocity.value.normalized;
		transform.forward = windDirection;
		windVelocity.OnValueChanged += OnWindVelocityChanged;
        UpdateEfficiency();
    }

    //draw the a line along the line calculated for the raycast below
    void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position + raycastPoint, transform.position + raycastPoint + windVelocity.value.normalized * _turbineDiameter * -8);
        }
    }

	void Update()
	{
		foreach (TurbineState ts in deletionQueue){
			states.Remove(ts);
		}

		if (states.Count == 0){
			GetComponent<TurbineMenu>().Hide();
		}

		foreach (TurbineState ts in states){
			ts.Update();
		}

        UpdateEfficiency();
		transform.forward = Vector3.Lerp(transform.forward, windDirection, 0.5f);
        if (isCharging) IncreaseEfficiency();
        else efficencyOvercharge -= overchargeDecrease;
        if (efficencyOvercharge < 0) efficencyOvercharge = 0;
    }

	void OnDestroy()
	{
		if(_all != null){
			_all.Remove(this);
		}
	}

    public void IncreaseEfficiency()
    {
        efficencyOvercharge += overchargeIncrease;
        if (efficencyOvercharge >= maxOvercharge) {
            efficencyOvercharge = maxOvercharge;

			

			TurbineStateManager.brokenState.Copy(this);
        }
        //efficencyOvercharge += (overchargeIncrease + overchargeDecrease);
    }

    public void BreakTurbine()
    {
        if (isBroken) return;
        TurbineStateManager.brokenState.Copy(this);
        if (GetComponent<TurbineParticle>() != null) GetComponent<TurbineParticle>().ActivateBreaking();
    }

    public void UpdateEfficiency()
    {
        currentEfficency = GetEfficiency();
        UpdateHUDOverlay();
    }

    public void UpdateHUDOverlay()
    {
        float power = currentEfficency * (1 + efficencyOvercharge);
       if (powerHUD.gameObject != null) {
            powerHUD.fillAmount = power / (_maxPower * (1 + maxOvercharge));
            if (currentEfficency <= 0.5f) powerHUD.color = Color.blue;
            else powerHUD.color = Color.green;
            if (efficencyOvercharge >= maxOvercharge / 2f) powerHUD.color = Color.yellow;
            if (efficencyOvercharge >= 3 * maxOvercharge / 4f) powerHUD.color = Color.red;
        }
    }

    //Raycast to the this object according to wind direction. If an object is in the way, the efficency descreases depending on objects in the way
    public float GetEfficiency()
    {
		Ray obstructionRay = new Ray(transform.position + raycastPoint, -windDirection);

        RaycastHit[] hitObjects;
        hitObjects = Physics.RaycastAll(obstructionRay);

        float value = _maxPower;

        foreach (RaycastHit hit in hitObjects)
        {
            //if not this object, reduce the power generated
            if (hit.collider != GetComponent<Collider>())
            {
                if (_debug) Debug.Log(hit.collider.gameObject);
                //reduce the power generated according distance to turbine in the way
                if (hit.collider.GetComponent<TurbineObject>() != null)
                {
                    float diff = Vector3.Distance(transform.position, hit.point) / _turbineDiameter;
                    //determines the efficency based on distance (distance of 8 turbine diameters is 0 difference)
                    float mod = Mathf.Pow(diff, (2f / 3f)) * 1 / 4;
                    if (mod > 1) mod = 1;
                    value *= mod;
                    if (_debug) Debug.Log("diff: " + diff + " value " + value);
                }

                //reduced the power generated if there is an object blocking wind
                if (hit.collider.GetComponent<WindShadowObject>() != null)
                {
                    float diff = Vector3.Distance(transform.position, hit.point);
                    float offset = diff / hit.collider.GetComponent<WindShadowObject>().ShadowDistance;
                    if (offset > 1) offset = 1;
                    value *= offset;
                    if (_debug) Debug.Log("diff: " + diff + " offset: " + offset + " value " + value);
                }

                if (hit.collider.GetComponent<Terrain>() != null)
                {
                    //checks the height and uses to deterine the shadow
                    float terrainShadow = _turbineHeight;
                    Vector3 upOneObstruction = transform.position + raycastPoint;

                    for (int i = 0; i < 100; i++)
                    {
                        upOneObstruction += Vector3.up * _turbineHeight * 0.25f;
						Ray upOneObstructionRay = new Ray(upOneObstruction, -windDirection);
                        RaycastHit[] oneUpHit;
                        oneUpHit = Physics.RaycastAll(upOneObstructionRay);
                        foreach (RaycastHit h in oneUpHit)
                        {
                            if (h.collider.GetComponent<Terrain>() != null)
                            {
                                terrainShadow += _turbineHeight * 0.25f;
                                break;
                            }
                        }
                    }

                    float diff = Vector3.Distance(transform.position, hit.point);
                    float offset = diff / terrainShadow;
                    if (offset > 1) offset = 1;
                    value *= offset;

                }
            }
        }
        if (_debug) Debug.Log("Efficiency Value: " + value);
        return value;
    }

	public float GetPowerOutput()
	{
		return currentEfficency * stateMultiplier * (1  + efficencyOvercharge) * _maxPower;
	}

	void OnWindVelocityChanged(Vector3 oldValue, Vector3 newValue)
	{
		windDirection = newValue.normalized;
	}

	public void AddState(TurbineState state)
	{
		foreach (TurbineState ts in states){
			if (ts.name == state.name) return;
		}

		if(state.name == TurbineStateManager.saboteurState.name){
			GetComponentInChildren<Saboteur>().StartAnimation();
		}

		states.Add(state);
	}

	public void RemoveState(TurbineState state)
	{
		deletionQueue.Add(state);

		if(state.name == TurbineStateManager.saboteurState.name){
			GetComponentInChildren<Saboteur>().EndAnimation();
		}
	}

	#region Interfaces

	public void OnTouch(Touch t, RaycastHit hit)
	{
        foreach (TurbineState ts in states){
		    ts.OnTouch(t,hit);
	    }
	}

	public void OnClick(ClickState state, RaycastHit hit)
	{
        foreach (TurbineState ts in states){
		    ts.OnClick(state, hit);
	    }
	}

	public void OnEnterWindzone ()
	{
        if (_debug) Debug.Log("Enter Windzone");
        isCharging = true;
        foreach (TurbineState ts in states){
		    ts.OnEnterWindzone();
	    }
	}

	public void OnStayWindzone(){}

    public void OnExitWindzone ()
    {
		if (_debug) Debug.Log("Exit Windzone");
        isCharging = false;
        foreach (TurbineState ts in states)
        {
            ts.OnExitWindzone();
        }
    }

	#endregion

	#region Button Callbacks

	public void Police()
	{
		Debug.Log("Police");

		foreach (TurbineState ts in states)
		{
			ts.OnPolice();
		}
	}

	public void Firemen()
	{
		Debug.Log("Firemen");
		foreach (TurbineState ts in states)
		{
			ts.OnFiremen();
		}
	}

	public void Repair()
	{
		Debug.Log("Repair");
		foreach (TurbineState ts in states)
		{
			ts.OnRepair();
		}
	}

	public void Cleanup()
	{
		Debug.Log("Cleanup");
		foreach (TurbineState ts in states)
		{
			ts.OnCleanup();
		}
	}

	#endregion
}
