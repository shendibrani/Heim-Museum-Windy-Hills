using UnityEngine;
using System.Collections;

public class lightdetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (var item in GameObject.FindObjectsOfType<Light> ()) {
			Debug.Log (item.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
