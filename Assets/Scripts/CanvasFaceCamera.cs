using UnityEngine;
using System.Collections;

public class CanvasFaceCamera : MonoBehaviour {

	void Update ()
	{
		transform.rotation = Camera.main.transform.rotation;
	}
}
