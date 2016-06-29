using UnityEngine;
using System.Collections;

public class WindBoostEffect : MonoBehaviour {


   
    float overchargePercentage = 0.8f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider col) {
        if (col.GetComponent<TurbineObject>()) {
            col.GetComponent<TurbineObject>().IncreaseEfficiency(overchargePercentage);
            Debug.Log("Increasing efficiency by stormcloud");
        }

    }
}
