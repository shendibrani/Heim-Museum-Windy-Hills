using UnityEngine;
using System.Collections;

public class WarningSignManager : MonoBehaviour {

	//[SerializeField] Highlightable police, firemen, repair, cleanup;
	[SerializeField] Animator police, firemen, repair, cleanup,help;

	public bool calledHelp { get; private set; }

	void Start()
	{
		calledHelp = false;
		GetComponent<TurbineObject> ().state.OnValueChanged += OnStateChange;
	}

	public void OnPolice()
	{
		Clear ();
		//police.SetHighlight(true);
		police.SetBool("Active",true);
		Dispatcher<PoliceMessage>.Subscribe (OnCallPolice);
	}

	public void OnFiremen()
	{
		Clear ();
		//firemen.SetHighlight(true);
		firemen.SetBool("Active",true);
		Dispatcher<FiremenMessage>.Subscribe (OnCallFiremen);
	}

	public void OnRepair()
	{
		Clear ();
		//repair.SetHighlight(true);
		repair.SetBool("Active",true);
		Dispatcher<RepairMessage>.Subscribe (OnCallRepair);
	}

	public void OnCleanup()
	{
		Clear ();
		//cleanup.SetHighlight(true);
		cleanup.SetBool("Active",true);
		Dispatcher<CleanupMessage>.Subscribe (OnCallClean);
	}

	void OnCallPolice(PoliceMessage pMessage)
	{
		if (pMessage.Sender.GetComponent<TurbineObject>() == this.GetComponent<TurbineObject>())
		{
			Dispatcher<PoliceMessage>.Unsubscribe(OnCallPolice);
			OnCallHelp ();
		}
	}

	void OnCallClean(CleanupMessage pMessage)
	{
		if (pMessage.Sender.GetComponent<TurbineObject>() == this.GetComponent<TurbineObject>())
		{
			Dispatcher<CleanupMessage>.Unsubscribe(OnCallClean);
			OnCallHelp ();
		}
	}

	void OnCallRepair(RepairMessage pMessage)
	{
		if (pMessage.Sender.GetComponent<TurbineObject>() == this.GetComponent<TurbineObject>())
		{
			Dispatcher<RepairMessage>.Unsubscribe(OnCallRepair);
			OnCallHelp ();
		}
	}

	void OnCallFiremen(FiremenMessage pMessage)
	{
		if (pMessage.Sender.GetComponent<TurbineObject>() == this.GetComponent<TurbineObject>())
		{
			Dispatcher<FiremenMessage>.Unsubscribe(OnCallFiremen);
			OnCallHelp ();
		}
	}

	void OnCallHelp()
	{
		Clear ();
		calledHelp = true;
		help.SetBool ("Active", true);
	}

	public void Clear()
	{
//		police.SetHighlight (false);
//		firemen.SetHighlight (false);
//		repair.SetHighlight (false);
//		cleanup.SetHighlight (false);
		police.SetBool 	("Active",false);
		firemen.SetBool ("Active",false);
		repair.SetBool 	("Active",false);
		cleanup.SetBool ("Active",false);
		help.SetBool 	("Active", false);
	}

	public void OnStateChange(TurbineState oldState, TurbineState newState)
	{
		Clear ();
		if (newState == null) 
		{
			calledHelp = false;
		}
		else if (newState != null && !calledHelp)
		{
			if (newState.name == TurbineStateManager.saboteurState.name) {
				OnPolice ();
			}
			else if (newState.name == TurbineStateManager.occupiedState.name){
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