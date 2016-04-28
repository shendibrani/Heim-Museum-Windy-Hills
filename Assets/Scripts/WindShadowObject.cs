using UnityEngine;
using System.Collections;

public class WindShadowObject : MonoBehaviour {

    [SerializeField]
    bool _debug;

    //the distance on which this object is detected and its 'shadow'is applied, in all directions from the edge 
    [SerializeField]
    float _shadowDistance = 10f;

    //the 1 - power multiplier for the efficency for the turbine in shadow
    [SerializeField]
    float _shadowPower = 1f;

    public float ShadowDistance
    {
        get
        {
            return _shadowDistance;
        }

        private set
        {
            _shadowDistance = value;
        }
    }

    public float ShadowPower
    {
        get
        {
            return _shadowPower;
        }

        private set
        {
            _shadowPower = value;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * (_shadowDistance + transform.localScale.x/2));
            Gizmos.DrawLine(transform.position, transform.position + - transform.forward * (_shadowDistance + transform.localScale.x / 2));
            Gizmos.DrawLine(transform.position, transform.position + transform.right* (_shadowDistance + transform.localScale.z / 2));
            Gizmos.DrawLine(transform.position, transform.position + -transform.right * (_shadowDistance + transform.localScale.z / 2));
        }
    }
}
