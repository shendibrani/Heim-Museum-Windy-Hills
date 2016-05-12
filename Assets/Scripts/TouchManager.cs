using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
	[SerializeField] bool touchInput;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(touchInput){
			for (int i = 0; i < Input.touchCount; i++){
				Touch t = Input.GetTouch(i);
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(t.position), out hit);
				if(hit.collider != null && hit.collider.GetComponent<ITouchSensitive>() != null){
					hit.collider.gameObject.GetComponent<ITouchSensitive>().OnTouch (t, hit);
				}
			}
		}
	}
}

