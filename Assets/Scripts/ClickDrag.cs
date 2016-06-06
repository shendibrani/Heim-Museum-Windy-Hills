using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ClickDrag : MonoBehaviour {

	Vector2 targetPos, startingPos;

	[SerializeField] bool debug;

	// Use this for initialization
	void Start () 
	{
		startingPos = GetComponent<RectTransform> ().anchoredPosition;
		targetPos = startingPos;
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (GetComponent<RectTransform> ().anchoredPosition, targetPos, 0.5f);
	}
		
	public void OnDrag(UnityEngine.EventSystems.BaseEventData data)
	{
		var pointerEventData = data as UnityEngine.EventSystems.PointerEventData;

		if (pointerEventData == null) return;
			
		targetPos += pointerEventData.delta/GetComponentInParent<Canvas>().scaleFactor;

		if(debug) Debug.Log ("[GUI] Started dragging " + gameObject.name + " at " + Time.time);
	}
	public void OnDragEnd(UnityEngine.EventSystems.BaseEventData data)
	{
		targetPos = startingPos;

		var pointerEventData = data as UnityEngine.EventSystems.PointerEventData;

		if(debug) Debug.Log ("[GUI] Stopped dragging " + gameObject.name + " at " + Time.time);

		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(pointerEventData.position), out hit);
		if(hit.collider != null && hit.collider.GetComponent<TurbineObject>() != null){

			if(debug) Debug.Log ("[GUI] Callback sent from " + gameObject.name + " to " + hit.collider.gameObject.name);

			switch (gameObject.name){
			case "Police":
				hit.collider.GetComponent<TurbineObject> ().Police();
				break;
			case "Firemen":
				hit.collider.GetComponent<TurbineObject> ().Firemen();
				break;
			case "Cleanup":
				hit.collider.GetComponent<TurbineObject> ().Cleanup();
				break;
			case "Repair":
				hit.collider.GetComponent<TurbineObject> ().Repair();
				break;
			}
		}
	}
}
