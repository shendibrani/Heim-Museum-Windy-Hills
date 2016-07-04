using UnityEngine;
using System.Collections;

public class CleanersDepartment : Station 
{
	bool spawned;

	protected override void Start () 
	{
		base.Start ();
		Dispatcher<CleanupMessage>.Subscribe(SendCleaners);
	}

	void SendCleaners(CleanupMessage rm) 
	{
		GetFreeAgent().Send (rm.Sender.GetComponent<TurbineObject> ());
	}
}
