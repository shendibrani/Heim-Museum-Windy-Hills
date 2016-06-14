using UnityEngine;
using System.Collections;

public class WarningSignManager : MonoBehaviour {

	[SerializeField] Highlightable police, firemen, repair, cleanup;

	void Start()
	{
		GetComponent<TurbineObject> ().state.OnValueChanged += OnStateChange;
	}

	public void OnPolice()
	{
		Clear ();
		police.SetHighlight(true);
	}

	public void OnFiremen()
	{
		Clear ();
		firemen.SetHighlight(true);
	}

	public void OnRepair()
	{
		Clear ();
		repair.SetHighlight(true);
	}

	public void OnCleanup()
	{
		Clear ();
		cleanup.SetHighlight(true);
	}

	public void Clear()
	{
		police.SetHighlight (false);
		firemen.SetHighlight (false);
		repair.SetHighlight (false);
		cleanup.SetHighlight (false);
	}

	public void OnStateChange(TurbineState oldState, TurbineState newState)
	{
		Clear ();

		if (newState != null)
		{
			if (newState.name == TurbineStateManager.saboteurState.name) {
				OnPolice ();
			} else if (newState.name == TurbineStateManager.brokenState.name) {
				OnRepair ();
			} else if (newState.name == TurbineStateManager.lowFireState.name) {
				OnFiremen ();
			} else if (newState.name == TurbineStateManager.highFireState.name) {
				OnFiremen ();
			} else if (newState.name == TurbineStateManager.dirtyState.name) {
				OnCleanup ();
			}
		}
	}
}