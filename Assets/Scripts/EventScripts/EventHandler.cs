using UnityEngine;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour {

    StormCloudEvent stormCloud;
    public List<EventsClass> eventsList;

	// Use this for initialization
	void Start () {
        eventsList.Add(stormCloud);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
