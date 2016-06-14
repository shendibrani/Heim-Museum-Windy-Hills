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
	[SerializeField] int waveDifficulty = 5;

	#region Events
	[SerializeField] StormCloudEvent stormEvent;
	[SerializeField] FireEvent fireEvent;
	[SerializeField] SaboteurEvent saboteurEvent;
	[SerializeField] FlockEvent flockEvent;
    [SerializeField] BoatEvent boatEvent;
    #endregion
    
    #region EventFlags
    bool waveStarted;
    bool waveEnded;
    #endregion

    public bool debug;
    private List<EventClass> eventsList, currentWave;
  
    // Use this for initialization
    void Start () 
	{
        WaveTimer = new Stopwatch();
        
        eventsList = new List<EventClass>();

        eventsList.Add(boatEvent);
		eventsList.Add(stormEvent);
		eventsList.Add(fireEvent);
		eventsList.Add(saboteurEvent);
		eventsList.Add(flockEvent);

		currentWave = GenerateWave(waveDifficulty);
	}

    bool initializedEvent = false;
    // Update is called once per frame
    bool initializeWaves = false;
    void Update () {
		if (TutorialProgression.Instance.IsComplete && !initializeWaves){
        	StartWaves();
            initializeWaves = true;
		}
        UpdateWaves();
    }

    void StartWaves() 
	{
		if (debug)UnityEngine.Debug.Log("<color=red>Wave with difficulty "+ GetWaveDifficulty(currentWave) +" Started!</color>");
        WaveTimer.Start();
        waveEnded = false;
        waveStarted = true;
        secondToSpawn = Random.Range(0, waveMaxTime);
    }

    void UpdateWaves() 
	{
    	if (waveStarted)// if wave system was started
        {
            if (debug) UnityEngine.Debug.Log("Elapsed Seconds: " + WaveTimer.Elapsed.Seconds);
            InitializeEvent(); //initialize first event

            if (WaveTimer.Elapsed.Seconds >= waveMaxTime && !waveEnded) // if Wave Time has ended, reset clock.
            {
               // if (debug) ClearLog();
                waveEnded = true;
                waveStarted = false;
                WaveTimer.Reset();
                WaveTimer.Start();
            }
        }

        if (WaveTimer.Elapsed.Seconds < waveCooldownTime && !waveStarted && waveEnded)
        {
            if (debug) UnityEngine.Debug.Log("<color=red>Wave Ended</color>");
            if (debug) UnityEngine.Debug.Log("Wave in Cooldown mode! Ending in: " + (waveCooldownTime - WaveTimer.Elapsed.Seconds));
        }

        if (WaveTimer.Elapsed.Seconds >= waveCooldownTime && waveEnded) //if waveCooldown has ended
        {
            secondToSpawn = Random.Range(0, waveMaxTime);
            if (debug) UnityEngine.Debug.Log("<color=red>Wave Cooldown Ended... New Wave Starting!</color>");
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
		EventClass e = GetRandomEventUnderDifficulty(difficulty);

		while (waveDiff < difficulty && e != null){
			wave.Add(e);
			waveDiff = GetWaveDifficulty(wave);
			e = GetRandomEventUnderDifficulty(difficulty - waveDiff);
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

	/// <summary>
	/// Gets a random event whose difficulty is equal or under the given value.
	/// </summary>
	/// <returns>A random event.</returns>
	/// <param name="difficulty">The highest difficulty allowed.</param>
	EventClass GetRandomEventUnderDifficulty (int difficulty)
	{
		List<EventClass> viable = eventsList.FindAll(x => x.difficulty < difficulty);

		if(viable.Count == 0) return null;

		return viable[RNG.Next(0, viable.Count)];
	}

    void InitializeEvent() 
	{
        if (WaveTimer.Elapsed.Seconds == secondToSpawn && !initializedEvent)
        {
            UnityEngine.Debug.Log("<color=red>Initializing event: </color>" + currentWave[0].name);
            currentWave[0].EventStart();
            currentWave.RemoveAt(0);
            initializedEvent = true;
        }

        if (currentWave.Count == 0) {
			currentWave = GenerateWave(waveDifficulty);
        }

        else if (WaveTimer.Elapsed.Seconds > secondToSpawn)
        {
            initializedEvent = false;
        }
    }

    /*public static void ClearLog()
	{
		var assembly = Assembly.GetAssembly (typeof(UnityEditor.ActiveEditorTracker));
		var type = assembly.GetType ("UnityEditorInternal.LogEntries");
		var method = type.GetMethod ("Clear");
		method.Invoke (new object (), null);
	}*/

}
