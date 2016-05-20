using UnityEngine;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour {

    StormCloudEvent stormCloudEvent;
    GameObject stormGO;


    private List<EventsClass> eventsList;
    static public int rnd;
    // Use this for initialization
    void Start () {
        stormGO = (GameObject)Resources.Load("StormCloud");

        eventsList = new List<EventsClass>();
        eventsList.Add(stormCloudEvent);
	}

    void InitializeEvent<T>() where T : StormCloudEvent 
    {
        rnd = Random.Range(0, TurbineObject.all.Count);
        GameObject tempStorm = Instantiate(stormGO);
        tempStorm.gameObject.AddComponent<StormCloudEvent>() ;
    }

    bool initializedEvent = false;
    // Update is called once per frame
	void FixedUpdate () {
        if (TurbineObject.all.Count > 0 && !initializedEvent)
        {
            InitializeEvent<StormCloudEvent>();
            initializedEvent = true;
        }
	}
}
