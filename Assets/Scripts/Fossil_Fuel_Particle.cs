using UnityEngine;
using System.Collections;

public class Fossil_Fuel_Particle : MonoBehaviour {

	[SerializeField] ParticleSystem white1;
	[SerializeField] ParticleSystem white2;
	[SerializeField] ParticleSystem white3;
	[SerializeField] ParticleSystem dark1;
	[SerializeField] ParticleSystem dark2;

	float lifeTime;


    public bool lessFossil = false;


    void Start () {
		lifeTime = white1.startLifetime;
    }

	public void UpdateParticles()
	{
		TurbineObject[] mills = FindObjectsOfType<TurbineObject>();
		int num = mills.Length;
		white1.GetComponent<ParticleSystem>().startLifetime = lifeTime - num;
		white2.GetComponent<ParticleSystem>().startLifetime = lifeTime - num;
		white3.GetComponent<ParticleSystem>().startLifetime = lifeTime - num;
		dark1.GetComponent<ParticleSystem>().startLifetime = lifeTime - num;
		dark2.GetComponent<ParticleSystem>().startLifetime = lifeTime - num;
	}
}