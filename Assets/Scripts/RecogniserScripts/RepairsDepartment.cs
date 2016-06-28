using UnityEngine;
using System.Collections.Generic;

public class RepairsDepartment : MonoBehaviour {

    bool spawned;
    static public RepairmanBehavior instance;
    private List<GameObject> repairMen;
    GameObject repairMan1, repairMan2, repairMan3;
    // Use this for initialization
	void Start () {
        repairMen = new List<GameObject>();
        repairMan1 = (GameObject)GameObject.Instantiate(Resources.Load("Repairman"), transform.position, Quaternion.identity);
        repairMan2 = (GameObject)GameObject.Instantiate(Resources.Load("Repairman"), transform.position, Quaternion.identity);
        repairMan3 = (GameObject)GameObject.Instantiate(Resources.Load("Repairman"), transform.position, Quaternion.identity);
        Dispatcher<RepairMessage>.Subscribe(SpawnRepairMan);
        repairMen.Add(repairMan1);
        repairMen.Add(repairMan2);
        repairMen.Add(repairMan3);
    }
	
	// Update is called once per frame
	void Update () {
   
	}

    void SpawnRepairMan(RepairMessage rm) {

        for (int i = 0; i < repairMen.Count; i++)
        {
            if (repairMen[i].GetComponent<RepairmanBehavior>().busy == false && repairMen[i].GetComponent<RepairmanBehavior>().targetTurbine == null) {
                repairMen[i].GetComponent<RepairmanBehavior>().SetTargetTurbine(rm.Sender.GetComponent<TurbineObject>());
                break;
            }
        }


    }
}
