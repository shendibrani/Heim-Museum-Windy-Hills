using UnityEngine;
using System.Collections;

public class UICallbacksSystem : MonoBehaviour 
{
	public static UIState currentState;

	[SerializeField] Highlightable police, firemen, repair, cleanup;

	public void OnPolice()
	{
		Clear ();
		police.SetHighlight(true);
		currentState = UIState.Police;
	}

	public void OnFiremen()
	{
		Clear ();
		firemen.SetHighlight(true);
		currentState = UIState.Firemen;
	}

	public void OnRepair()
	{
		Clear ();
		repair.SetHighlight(true);
		currentState = UIState.Repair;
	}

	public void OnCleanup()
	{
		Clear ();
		cleanup.SetHighlight(true);
		currentState = UIState.Cleanup;
	}

	public void Clear()
	{
		currentState = UIState.None;

		police.SetHighlight (false);
		firemen.SetHighlight (false);
		repair.SetHighlight (false);
		cleanup.SetHighlight (false);
	}
}

public enum UIState 
{
	None, Police, Firemen, Repair, Cleanup
}
