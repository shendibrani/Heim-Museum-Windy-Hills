using UnityEngine;
using System.Collections;

public class Fossil_Fuel_Particle : MonoBehaviour {

     ParticleSystem white1;
     ParticleSystem white2;
     ParticleSystem white3;
     ParticleSystem dark1;
     ParticleSystem dark2;

    public GameObject white1Obj;
    public GameObject white2Obj;
    public GameObject white3Obj;
    public GameObject dark1Obj;
    public GameObject dark2Obj;



    public bool lessFossil = false;


    void Start () {

        white1 = white1Obj.GetComponent<ParticleSystem>();
        white2 = white2Obj.GetComponent<ParticleSystem>();
        white3 = white3Obj.GetComponent<ParticleSystem>();
        dark1 = dark1Obj.GetComponent<ParticleSystem>();
        dark2 = dark2Obj.GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {

        if (lessFossil == true)
        {

            white1.GetComponent<ParticleSystem>().startLifetime -= 0.01f;
            white2.GetComponent<ParticleSystem>().startLifetime -= 0.01f;
            white3.GetComponent<ParticleSystem>().startLifetime -= 0.01f;
            dark1.GetComponent<ParticleSystem>().startLifetime -= 0.01f;
            dark2.GetComponent<ParticleSystem>().startLifetime -= 0.01f;

        }

    }
}
