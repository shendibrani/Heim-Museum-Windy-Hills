using UnityEngine;
using System.Collections;

public class Sail : MonoBehaviour {

	bool free = false;
	Vector3 rotationAx;

	// Use this for initialization
	void Start () {
		rotationAx = new Vector3(Random.Range(3,12),Random.Range(1,3),Random.Range(1,3));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (free)
		{
			transform.localPosition += new Vector3 (0, 0, 1);
			transform.Rotate (rotationAx);
		}
	}

	public void SetFree()
	{
		free = true;
	}
}
