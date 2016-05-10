using UnityEngine;
using System.Collections;

public class BladeBehaviour : MonoBehaviour
{
	[SerializeField] float angularVelocityPerMetreSecond = 0.01f;
	float windSpeed;

	// Use this for initialization
	void Start ()
	{
		windSpeed = TurbineObject.windVelocity.value.magnitude;
		TurbineObject.windVelocity.OnValueChanged += OnWindVelocityChanged;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.forward,angularVelocityPerMetreSecond*windSpeed);
	}

	void OnWindVelocityChanged(Vector3 oldValue, Vector3 newValue) 
	{
		windSpeed = newValue.magnitude;
	}
}

