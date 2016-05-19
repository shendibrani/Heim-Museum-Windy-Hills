using UnityEngine;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour {

    StormCloudEvent stormCloud;
    private List<EventsClass> eventsList;
    static public int rnd;
    // Use this for initialization
    void Start () {
        eventsList = new List<EventsClass>();
        eventsList.Add(stormCloud);
	}

    void InitializeEvent<T>() where T : EventsClass
    {
        rnd = Random.Range(0, TurbineObject.all.Count);
        TurbineObject.all[rnd].gameObject.AddComponent<T>();
    }

    bool initializedEvent = false;
    // Update is called once per frame
	void Update () {
        if (TurbineObject.all.Count > 0 && !initializedEvent)
        {
            InitializeEvent<StormCloudEvent>();
            initializedEvent = true;
        }
	}
}
