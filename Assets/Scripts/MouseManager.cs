using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	[SerializeField] bool mouseInput;

	// Update is called once per frame
	void Update () {
		if(mouseInput){
			if(Input.GetMouseButtonDown(0)){
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
				if(hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null){
					hit.collider.gameObject.GetComponent<IMouseSensitive>().OnClick (hit);
				}
			}
		}
	}
}
