using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ClickDrag : MonoBehaviour {

	int currentTouchID;
	bool dragged;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	public void OnDrag(UnityEngine.EventSystems.BaseEventData data)
	{
		Debug.Log ("Callback");

		var pointerEventData = data as UnityEngine.EventSystems.PointerEventData;

		if (pointerEventData == null) { 
			Debug.Log ("Wrong Type of Event"); 
			return;
		}
			
		GetComponent<RectTransform>().anchoredPosition += pointerEventData.delta/GetComponentInParent<Canvas>().scaleFactor;
	}
	public void OnDragEnd()
	{

	}
}
