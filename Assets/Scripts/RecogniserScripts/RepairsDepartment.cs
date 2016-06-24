using UnityEngine;
using System.Collections;

public class RepairsDepartment : MonoBehaviour {

    bool spawned;
    static public RepairmanBehavior instance;
   
    // Use this for initialization
	void Start () {

        Dispatcher<RepairMessage>.Subscribe(SpawnRepairMan);
       
	}
	
	// Update is called once per frame
	void Update () {
   
	}

    void SpawnRepairMan(RepairMessage rm) {
        if (!spawned)
        {
            GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Repairman"), transform.position, Quaternion.identity);
            spawned = true;
        }

    }
    
}
