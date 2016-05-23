using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


public class EventHandler : MonoBehaviour {
    private Stopwatch WaveTimer;
    private int secondToSpawn;
    private int waveNumber;
    private int waveMaxTime = 10;
    private int waveCooldownTime = 5;


    #region EventFlags
    bool waveStarted;
    bool waveEnded;
    #endregion

    #region Events
    StormCloudEvent stormCloudEvent;
    #endregion

    private List<EventsClass> eventsList;
  

    // Use this for initialization
    void Start () {
        WaveTimer = new Stopwatch();
        

        eventsList = new List<EventsClass>();

        stormCloudEvent = GetComponent<StormCloudEvent>();


        eventsList.Add(stormCloudEvent);
	}

    bool initializedEvent = false;
    // Update is called once per frame
   
    void Update () {

        startWaves();
        UpdateWaves();

    }

    void startWaves() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.Debug.Log("<color=red>Wave Started!</color>");
            WaveTimer.Start();
            waveEnded = false;
            waveStarted = true;
            secondToSpawn = Random.Range(0, waveMaxTime);
        }
    }

    void UpdateWaves() {
        
        if (waveStarted)// if wave system was started
        {
            
            UnityEngine.Debug.Log("Elapsed Seconds: " + WaveTimer.Elapsed.Seconds);
            InitializeEvent(); //initialize first event

            if (WaveTimer.Elapsed.Seconds >= waveMaxTime && !waveEnded) // if Wave Time has ended, reset clock.
            {
                ClearLog();
                waveEnded = true;
                waveStarted = false;
                WaveTimer.Reset();
                WaveTimer.Start();

            }

        }

        if (WaveTimer.Elapsed.Seconds < waveCooldownTime && !waveStarted && waveEnded)
        {
            UnityEngine.Debug.Log("<color=red>Wave Ended</color>");
            UnityEngine.Debug.Log("Wave in Cooldown mode! Ending in: " + (waveCooldownTime - WaveTimer.Elapsed.Seconds));
            
        }

        if (WaveTimer.Elapsed.Seconds >= waveCooldownTime && waveEnded) //if waveCooldown has ended
        {
            secondToSpawn = Random.Range(0, waveMaxTime);
            UnityEngine.Debug.Log("<color=red>Wave Cooldown Ended... New Wave Starting!</color>");
            WaveTimer.Reset();
            WaveTimer.Start();
            waveEnded = false;
            waveStarted = true;
            
       
        }
    }

    void InitializeEvent() {

        if (WaveTimer.Elapsed.Seconds == secondToSpawn && !initializedEvent)
        {
            UnityEngine.Debug.Log("<color=red>Initializing random event!</color>");
            eventsList[Random.Range(0, eventsList.Count)].EventStart();
            initializedEvent = true;

        }
        else if ( WaveTimer.Elapsed.Seconds > secondToSpawn) {
            initializedEvent = false;
        }
    }

    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditorInternal.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

}
