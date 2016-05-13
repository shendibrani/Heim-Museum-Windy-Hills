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
					foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>()){
						ms.OnClick (ClickState.Down, hit);
					}
				}
			} else if (Input.GetMouseButtonUp(0)){
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
				if(hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null){
					foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>()){
						ms.OnClick (ClickState.Up, hit);
					}
				}
			} else if(Input.GetMouseButton(0)){
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
				if(hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null){
					foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>()){
						ms.OnClick (ClickState.Pressed, hit);
					}
				}
			}
		}
	}
}
	