using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	[SerializeField] bool mouseInput;

	// Update is called once per frame
	void Update () {
		if(mouseInput){
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                foreach (RaycastHit hit in hits) {
                if (hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null)
                {
                    foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>())
                    {
                        ms.OnClick(ClickState.Down, hit, Camera.main.ScreenPointToRay(Input.mousePosition));
                    }
                }
            }
			} else if (Input.GetMouseButtonUp(0)){
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null)
                    {
                        foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>())
                        {
                            ms.OnClick(ClickState.Up, hit, Camera.main.ScreenPointToRay(Input.mousePosition));
                        }
                    }
                }
			} else if(Input.GetMouseButton(0)){
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.GetComponent<IMouseSensitive>() != null)
                    {
                        foreach (IMouseSensitive ms in hit.collider.GetComponents<IMouseSensitive>())
                        {
                            ms.OnClick(ClickState.Pressed, hit, Camera.main.ScreenPointToRay(Input.mousePosition));
                        }
                    }
                }
			}
		}
	}
}
	