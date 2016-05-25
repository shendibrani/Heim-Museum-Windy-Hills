using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public class EventHandler : MonoBehaviour {
    
	private Stopwatch WaveTimer;
	[SerializeField] int secondToSpawn;
	[SerializeField] int waveNumber;
	[SerializeField] int waveMaxTime = 10;
	[SerializeField] int waveCooldownTime = 5;

	#region Events
	[SerializeField] StormCloudEvent stormEvent;
	[SerializeField] FireEvent fireEvent;
	[SerializeField] SaboteurEvent saboteurEvent;
	[SerializeField] FlockEvent flockEvent;
	#endregion

    #region EventFlags
    bool waveStarted;
    bool waveEnded;
    #endregion

    private List<EventClass> eventsList, currentWave;
  

    // Use this for initialization
    void Start () 
	{
        WaveTimer = new Stopwatch();
        
        eventsList = new List<EventClass>();

		eventsList.Add(stormEvent);
		eventsList.Add(fireEvent);
		eventsList.Add(saboteurEvent);
		eventsList.Add(flockEvent);

		currentWave = GenerateWave(5);
	}

    bool initializedEvent = false;
    // Update is called once per frame
   
    void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
        	StartWaves();
		}
        UpdateWaves();
    }

    void StartWaves() 
	{
        UnityEngine.Debug.Log("<color=red>Wave Started!</color>");
        WaveTimer.Start();
        waveEnded = false;
        waveStarted = true;
        secondToSpawn = Random.Range(0, waveMaxTime);
    }

    void UpdateWaves() 
	{
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

	List<EventClass> GenerateWave(int difficulty)
	{
		List<EventClass> wave = new List<EventClass>();

		int waveDiff = 0;
		EventClass e = GetRandomEventUnderDifficluty(difficulty - waveDiff);

		while (waveDiff < difficulty && e != null){
			wave.Add(e);
			waveDiff = GetWaveDifficulty(wave);
			e = GetRandomEventUnderDifficluty(difficulty - waveDiff);
		}

		return wave;
	}

	int GetWaveDifficulty(List<EventClass> wave)
	{
		int difficulty = 0;

		foreach(EventClass e in wave){
			difficulty += e.difficulty;
		}

		return difficulty;
	}

	EventClass GetRandomEventUnderDifficluty (int difficulty)
	{
		List<EventClass> viable = eventsList.FindAll(x => x.difficulty < difficulty);

		if(viable.Count == 0) return null;

		return viable[RNG.Next(0, viable.Count)];
	}

    void InitializeEvent() 
	{
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
