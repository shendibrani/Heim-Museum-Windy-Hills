using UnityEngine;
using System.Collections;

public class SunTest : MonoBehaviour {

	[SerializeField] float speed = 0;

	float dist;

	// Use this for initialization
	void Start () {
		dist = GetComponent<Light> ().cookieSize;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.transform.localPosition.x < dist)
		{
			this.transform.localPosition += new Vector3 (speed, 0, 0);
		} else {
			this.transform.position = Vector3.zero;
		}
	}
}
