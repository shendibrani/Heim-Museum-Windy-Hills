using UnityEngine;
using System.Collections;

public class WindParticleControll : MonoBehaviour {

	[SerializeField] ParticleSystem Wind;

	[Range(0,1)] public float Strength = 0;

	[SerializeField] float BaseSize = 2;
	[SerializeField] float BaseLifetime = 2;

	void Update ()
	{
		Wind.startSize = Random.Range (1f, Mathf.Lerp(0.1f,BaseSize,Strength));
		Wind.startLifetime = Random.Range (1,Mathf.Lerp(1,BaseLifetime,Strength));
	}
}
