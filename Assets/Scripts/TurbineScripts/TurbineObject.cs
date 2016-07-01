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

	public Monitored<TurbineState> state { get; private set;}

    //Maximum Power Avalible from Turbine (in MW)
    float _maxPower = 1;

	#region State Booleans

	public bool isFine {
		get {
			return (state.value == null);
		}
	}

	public bool isBroken
	{
		get {
			return (!isFine && state.value.name == TurbineStateManager.brokenState.name);
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
			if (state.value != null) {
				return state.value.efficiencyMultiplyer;
			} else {
				return 1;
			}
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
	[SerializeField]
	WindParticleControll windParticle;
	[SerializeField]
	Image warning;

    // Use this for initialization
    void Start()
    {
		if(_all != null){
			_all.Add(this);
		}

		state = new Monitored<TurbineState> (null);
		state.OnValueChanged += GetComponent<TurbineParticle> ().OnStateChange;
		windDirection = windVelocity.value.normalized;
		transform.forward = windDirection;
		windVelocity.OnValueChanged += OnWindVelocityChanged;
        UpdateEfficiency();
		FindObjectOfType<Fossil_Fuel_Particle>().UpdateParticles ();
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
		if (!isFine) { 
			state.value.Update ();
			warning.enabled = true;
		} else {
			warning.enabled = false;
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

    public void IncreaseEfficiency(float i = 1f)
    {
		if (state.value == null || state.value.name == TurbineStateManager.turbineCooldownState.name){
        efficencyOvercharge += overchargeIncrease * i;
        if (efficencyOvercharge >= maxOvercharge) {
            efficencyOvercharge = maxOvercharge;


            BreakTurbine();
           
			//TurbineStateManager.brokenState.Copy(this);
        }
        //efficencyOvercharge += (overchargeIncrease + overchargeDecrease);
		}}

    public void BreakTurbine()
    {
        if (isBroken) return;

		if (TurbineStateManager.brokenState.Copy(this) != null)
		{
			TutorialProgression.Instance.CheckBrokenWindmill(this);
		}
    }

    public void UpdateEfficiency()
    {
		if (isBroken) { efficencyOvercharge = 0; }
        currentEfficency = GetEfficiency();
        UpdateHUDOverlay();
    }

    public void UpdateHUDOverlay()
    {
        float power = currentEfficency * (1 + efficencyOvercharge) * stateMultiplier;
       if (powerHUD.gameObject != null) {
            powerHUD.fillAmount = power / (_maxPower * (1 + maxOvercharge));

			windParticle.Strength = powerHUD.fillAmount;

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

	#region Interfaces

	public void OnTouch(Touch t, RaycastHit hit, Ray ray)
	{
        if (t.phase == TouchPhase.Ended)
        {
            if (!isFine)
            {
                state.value.OnTouch(t, hit, ray);
            }

            ConsumeUIState();
        }
	}

	public void OnClick(ClickState clickState, RaycastHit hit, Ray ray)
	{
        if (clickState == ClickState.Down)
        {
            if (!isFine)
            {
                state.value.OnClick(clickState, hit, ray);
            }

            ConsumeUIState();
        }
	}

	void ConsumeUIState ()
	{
		Dispatcher<TurbineClickMessage>.Dispatch(new TurbineClickMessage(this.gameObject));
		switch (UICallbacksSystem.currentState) {
		case UIState.Police:
			Police ();
			break;
		case UIState.Firemen:
			Firemen ();
			break;
		case UIState.Repair:
			Repair ();
			break;
		case UIState.Cleanup:
			Cleanup ();
			break;
		}
		//UICallbacksSystem.currentState = UIState.None;
	}

	public void OnEnterWindzone ()
	{
		Dispatcher<TurbineWindZoneMessage>.Dispatch(new TurbineWindZoneMessage(gameObject, WindZoneState.Enter));

        if (_debug) Debug.Log("Enter Windzone");
        isCharging = true;
		if (!isFine) {
			state.value.OnEnterWindzone ();
		}
		else
		{
			GetComponent<TurbineParticle> ().Sparkles (true);
		}

	}

	public void OnStayWindzone(){
		if (!isFine)
		{
			state.value.OnStayWindzone();
		}
	}

    public void OnExitWindzone ()
    {
		Dispatcher<TurbineWindZoneMessage>.Dispatch(new TurbineWindZoneMessage(gameObject, WindZoneState.Exit));
		if (_debug) Debug.Log("Exit Windzone");
        isCharging = false;
		if (!isFine) {
			state.value.OnExitWindzone ();
		}
    }

	#endregion

	#region Button Callbacks

	public void Police()
	{
		if (!isFine && (state.value.name == TurbineStateManager.occupiedState.name || state.value.name == TurbineStateManager.saboteurState.name) ) {
			Dispatcher<PoliceMessage>.Dispatch(new PoliceMessage(gameObject));
			//state.value.OnPolice ();
		}
	}

	public void Firemen()
	{
		if (!isFine && (state.value.name == TurbineStateManager.lowFireState.name || state.value.name == TurbineStateManager.highFireState.name) ){
			Dispatcher<FiremenMessage>.Dispatch(new FiremenMessage(gameObject));
			//state.value.OnFiremen ();
		}
	}

	public void Repair()
	{
		if (!isFine && state.value.name == TurbineStateManager.brokenState.name) {
			Dispatcher<RepairMessage>.Dispatch(new RepairMessage(gameObject));
			//state.value.OnRepair ();

		}
	}

	public void Cleanup()
	{
		if (!isFine && state.value.name == TurbineStateManager.dirtyState.name) {
			Dispatcher<CleanupMessage>.Dispatch(new CleanupMessage(gameObject));
			//state.value.OnCleanup ();
		}
	}

	#endregion
}

public class TurbineWindZoneMessage : Message
{
	public readonly WindZoneState state;

	public TurbineWindZoneMessage (GameObject sender, WindZoneState state) : base (sender)
	{
		this.state = state;
	}
}

public enum WindZoneState { Enter, Exit }