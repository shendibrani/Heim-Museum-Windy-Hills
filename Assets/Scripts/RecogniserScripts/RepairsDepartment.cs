using UnityEngine;
using System.Collections.Generic;

public class RepairsDepartment : Station {

    bool spawned;
   
	protected override void Start () 
	{
		base.Start ();
        Dispatcher<RepairMessage>.Subscribe(SendRepairmen);
    }

    void SendRepairmen(RepairMessage rm) 
	{
		GetFreeAgent().Send (rm.Sender.GetComponent<TurbineObject> ());
    }
}
