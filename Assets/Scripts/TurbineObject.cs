using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurbineObject : MonoBehaviour, IMouseSensitive, ITouchSensitive, IWindSensitive
{

    //Wind Direction in Vector
	public static Monitored<Vector3> windVelocity = new Monitored<Vector3>(new Vector3(0, 0, 1));

	static HashSet<TurbineObject> _all;

	public static IEnumerable<TurbineObject> all {
		get {
			if (_all == null) {
				_all = new HashSet<TurbineObject>(FindObjectsOfType<TurbineObject>());
			}
			return _all;
		}
	}

	Vector3 windDirection;

	HashSet<TurbineState> states;

    //Maximum Power Avalible from Turbine (in MW)
    float _maxPower = 1;

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

    [SerializeField]
    bool isCharging;

    [SerializeField]
    bool _debug;

    public float currentEfficency { get; private set; }

    // Use this for initialization
    void Start()
    {
		if(_all != null){
			_all.Add(this);
		}

		windDirection = windVelocity.value.normalized;
		transform.forward = windDirection;
		windVelocity.OnValueChanged += OnWindVelocityChanged;
        UpdateEfficiency(this.gameObject);
    }

    //draw the a line along the line calculated for the raycast below
    void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + -windVelocity.value.normalized * _turbineDiameter * 8);
        }
    }

	void Update()
	{
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
        if (efficencyOvercharge > maxOvercharge) efficencyOvercharge = maxOvercharge;
        //efficencyOvercharge += (overchargeIncrease + overchargeDecrease);
    }

    public void UpdateEfficiency(GameObject go)
    {
        currentEfficency = GetEfficiency();
    }

    //Raycast to the this object according to wind direction. If an object is in the way, the efficency descreases depending on objects in the way
    public float GetEfficiency()
    {
		Ray obstructionRay = new Ray(transform.position, -windDirection);

        RaycastHit[] hitObjects;
        hitObjects = Physics.RaycastAll(obstructionRay);

        float value = _maxPower;

        foreach (RaycastHit hit in hitObjects)
        {
            //if not this object, reduce the power generated
            if (hit.collider != GetComponent<Collider>())
            {
                //reduce the power generated according distance to turbine in the way
                if (hit.collider.GetComponent<TurbineObject>() != null)
                {
                    float diff = Vector3.Distance(transform.position, hit.point) / _turbineDiameter;
                    //determines the efficency based on distance (distance of 8 turbine diameters is 0 difference)
                    float mod = Mathf.Pow(diff, (2f / 3f)) * 1 / 4;
                    if (mod > 1) mod = 1;
                    value *= mod;
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
                    Vector3 upOneObstruction = transform.position;

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
		if(!states.Contains(state)){
			states.Add(state);
		}
	}

	public void RemoveState(TurbineState state)
	{
		states.Remove(state);
	}

	#region Interfaces

	public void OnTouch(Touch t, RaycastHit hit)
	{
        if (states != null)
            foreach (TurbineState ts in states){
			    ts.OnTouch(t,hit);
		    }
	}

	public void OnClick(ClickState state, RaycastHit hit)
	{
        if (states != null)
            foreach (TurbineState ts in states){
			    ts.OnClick(state, hit);
		    }
	}

	public void OnEnterWindzone ()
	{
        if (_debug) Debug.Log("Enter Windzone");
        isCharging = true;
        if (states != null)
            foreach (TurbineState ts in states){
			    ts.OnEnterWindzone();
		    }
	}

    public void OnExitWindzone ()
    {
		if (_debug) Debug.Log("Exit Windzone");
        isCharging = false;
        if (states != null)
            foreach (TurbineState ts in states)
            {
                ts.OnExitWindzone();
            }
    }
	#endregion
}
