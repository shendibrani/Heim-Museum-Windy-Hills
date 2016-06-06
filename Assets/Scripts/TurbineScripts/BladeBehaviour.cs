using UnityEngine;
using System.Collections;

public class BladeBehaviour : MonoBehaviour
{
	[SerializeField] float angularVelocityPerMetreSecond = 0.01f;
	float windSpeed;
	[SerializeField] TurbineObject turbine;

    float currentPower;
    [SerializeField]
    float easing = 0.1f;
	// Use this for initialization
	void Start ()
	{
        windSpeed = TurbineObject.windVelocity.value.magnitude;
		TurbineObject.windVelocity.OnValueChanged += OnWindVelocityChanged;
	}
	
	// Update is called once per frame
	void Update ()
	{
        currentPower += (turbine.GetPowerOutput() - currentPower) * easing;
       // Debug.Log("Power Output " + turbine.GetPowerOutput() + " Current " + currentPower); 
		transform.Rotate(Vector3.up,angularVelocityPerMetreSecond*windSpeed*currentPower);
	}

	void OnWindVelocityChanged(Vector3 oldValue, Vector3 newValue) 
	{
		windSpeed = newValue.magnitude;
	}
}

