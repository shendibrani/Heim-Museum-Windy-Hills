using UnityEngine;
using System.Collections;

public class BladeBehaviour : MonoBehaviour
{
	[SerializeField] float angularVelocityPerMetreSecond = 0.01f;
	float windSpeed;
	[SerializeField] TurbineObject turbine;

	// Use this for initialization
	void Start ()
	{
		windSpeed = TurbineObject.windVelocity.value.magnitude;
		TurbineObject.windVelocity.OnValueChanged += OnWindVelocityChanged;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.up,angularVelocityPerMetreSecond*windSpeed*turbine.currentEfficency);
	}

	void OnWindVelocityChanged(Vector3 oldValue, Vector3 newValue) 
	{
		windSpeed = newValue.magnitude;
	}
}

