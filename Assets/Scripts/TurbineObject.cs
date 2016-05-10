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
    float _turbineDiameter = 4f;
    [SerializeField]
    float _turbineHeight = 8f;
    //[SerializeField]
    //float _turbine = 30f;

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
            Gizmos.DrawLine(transform.position, transform.position + -windDirection * _turbineDiameter * 8);
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
            //if not this object, reduce the power generated
            if (hit.collider != GetComponent<Collider>())
            {
                //reduce the power generated according distance to turbine in the way
                if (hit.collider.GetComponent<TurbineObject>() != null)
                {
                    float diff = Vector3.Distance(transform.position, hit.point) / _turbineDiameter;
                    //determines the efficency based on distance (distance of 8 turbine diameters is 0 difference)
                    float mod = Mathf.Pow(diff, (2f / 3f)) * 1 / 4;
                    if (mod > 1) mod = 1;
                    value *= mod;
                }

                //reduced the power generated if there is an object blocking wind
                if (hit.collider.GetComponent<WindShadowObject>() != null)
                {
                    float diff = Vector3.Distance(transform.position, hit.point);
                    float offset = diff / hit.collider.GetComponent<WindShadowObject>().ShadowDistance;
                    if (offset > 1) offset = 1;
                    value *= offset;
                }

                if (hit.collider.GetComponent<Terrain>() != null)
                {
                    //checks the height and uses to deterine the shadow
                    float terrainShadow = _turbineHeight;
                    Vector3 upOneObstruction = transform.position;

                    for (int i = 0; i < 100; i++)
                    {
                        upOneObstruction += Vector3.up * _turbineHeight * 0.25f;
                        Ray upOneObstructionRay = new Ray(upOneObstruction, -windDirection.normalized);
                        RaycastHit[] oneUpHit;
                        oneUpHit = Physics.RaycastAll(upOneObstructionRay);
                        foreach (RaycastHit h in oneUpHit)
                        {
                            if (h.collider.GetComponent<Terrain>() != null)
                            {
                                terrainShadow += _turbineHeight * 0.25f;
                                break;
                            }
                        }
                    }

                    float diff = Vector3.Distance(transform.position, hit.point);
                    float offset = diff / terrainShadow;
                    Debug.Log("diff " + diff + " shadow " + terrainShadow + " offset " + offset);
                    //if (diff <= hit.collider.GetComponent<WindShadowObject>().ShadowDistance)
                    if (offset > 1) offset = 1;
                    value *= offset;

                }
            }
        }

        return value;
    }
}
