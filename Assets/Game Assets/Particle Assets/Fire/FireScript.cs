using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

    ParticleSystem fire;
    public GameObject smokeObject;
    ParticleSystem smoke;
    float lifetime = 1.0f;
    float smokeLifetime = 12.0f;
    public bool burning = false;

	void Start () {
        fire = GetComponent<ParticleSystem>();
        smoke = smokeObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	if (burning == true)
        {
            if (lifetime < 4.0f)
            {
                lifetime += 0.009f;
                smokeLifetime += 0.025f;
                fire.GetComponent<ParticleSystem>().startLifetime = lifetime;
                smoke.GetComponent<ParticleSystem>().startLifetime = smokeLifetime;
            }


        }
	}


}
