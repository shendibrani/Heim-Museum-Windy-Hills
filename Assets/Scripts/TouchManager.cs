﻿using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
	[SerializeField] bool debug, touchInput;
	
	// Update is called once per frame
	void Update ()
	{
		if(touchInput){
			for (int i = 0; i < Input.touchCount; i++){
				Touch t = Input.GetTouch(i);
				if (debug) Debug.Log ("[Touch] Touch " + i + " position: " + t.position);
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(t.position));
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.GetComponent<ITouchSensitive>() != null)
                    {
                        foreach (ITouchSensitive ts in hit.collider.GetComponents<ITouchSensitive>())
                        {
                            if (debug) Debug.Log("[Touch] [TouchInterfaceCallback] Touch sent to " + hit.collider.gameObject.name);
                            ts.OnTouch(t, hit, Camera.main.ScreenPointToRay(t.position));
                        }
                    }
                }
			}
		}
	}
}

