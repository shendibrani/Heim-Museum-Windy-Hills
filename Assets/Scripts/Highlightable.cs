using System;
using UnityEngine;

public abstract class Highlightable : MonoBehaviour
{
	[SerializeField] protected bool highlighted, reactToMouseClick, reactToMouseOver;

	protected virtual void Start(){
		ProcessHighlight();
	}

	void OnMouseEnter()
	{
		if(reactToMouseOver){
			SetHighlight(true);
		}
	}
	
	void OnMouseExit()
	{
		if(reactToMouseOver){
			SetHighlight(false);
		}
	}
	
	void OnMouseOver()
	{
		if(reactToMouseClick){
			if(Input.GetMouseButtonUp(0)){
				ToggleHighlight();
			}
		}
	}

	public void ToggleHighlight()
	{
		highlighted = !highlighted;
		ProcessHighlight();
	}
	
	public void SetHighlight(bool b) 
	{
		highlighted = b;
		ProcessHighlight();
	}

	public abstract void ProcessHighlight();
}


