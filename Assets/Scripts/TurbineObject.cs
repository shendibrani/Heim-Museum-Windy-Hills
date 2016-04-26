using UnityEngine;
using System.Collections;

public class TurbineObject : MonoBehaviour {

    //Wind Direction in Vector
    public static Vector3 windDirection;

    //Maximum Power Avalible from Turbine (in MW)
    float _maxPower = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        WindDetectionRaycast();
	}

    //Raycast to the this object according to wind direction. If an object is in the way, the efficency descreases depending on objects in the way
    void WindDetectionRaycast()
    {
        Ray obstructionRay = new Ray(transform.position, -windDirection);
        RaycastHit [] hitObjects;
        hitObjects = Physics.RaycastAll(obstructionRay);


        float efficency = 1;
        foreach (RaycastHit hit in hitObjects)
        {
            if (hit.collider != GetComponent<Collider>())
            {
                efficency -= 0.25f;
            }
        }

    }
}
