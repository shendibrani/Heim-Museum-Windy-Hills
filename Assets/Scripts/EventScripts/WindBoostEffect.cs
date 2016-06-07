using UnityEngine;
using System.Collections;

public class WindBoostEffect : MonoBehaviour {


    [SerializeField]
    float overchargePercentage = 0.5f;
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
