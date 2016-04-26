using UnityEngine;
using System.Collections;

public class TurbineObject : MonoBehaviour
{

    //Wind Direction in Vector
    public static Vector3 windDirection = new Vector3(0, 0, 1);

    //Maximum Power Avalible from Turbine (in MW)
    float _maxPower = 1;

    float _efficencyDecrease = 0.5f;

    [SerializeField]
    bool _debug;

    // Use this for initialization
    void Start()
    {
        transform.forward = windDirection;
    }

    // Update is called once per frame
    void Update()
    {
    }


    //draw the a line along the line calculated for the raycast below
    void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + -windDirection * 20);
        }
    }

    //Raycast to the this object according to wind direction. If an object is in the way, the efficency descreases depending on objects in the way
    public float WindDetectionRaycast()
    {
        Ray obstructionRay = new Ray(transform.position, -windDirection);
        RaycastHit[] hitObjects;
        hitObjects = Physics.RaycastAll(obstructionRay);

        float value = _maxPower;
        foreach (RaycastHit hit in hitObjects)
        {
            if (hit.collider != GetComponent<Collider>())
            {
                value *= 0.5f;
            }
        }

        return value;
    }
}
