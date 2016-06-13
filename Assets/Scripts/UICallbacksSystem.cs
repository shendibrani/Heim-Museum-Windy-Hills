using UnityEngine;
using System.Collections;

public class UICallbacksSystem : MonoBehaviour 
{
	public static UIState currentState;

	public void OnPolice(){
		currentState = UIState.Police;
	}

	public void OnFiremen(){
		currentState = UIState.Firemen;
	}

	public void OnRepair(){
		currentState = UIState.Repair;
	}

	public void OnCleanup(){
		currentState = UIState.Cleanup;
	}
}

public enum UIState 
{
	None, Police, Firemen, Repair, Cleanup
}
