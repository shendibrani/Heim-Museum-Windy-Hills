using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                if (FindObjectsOfType<ScoreManager>().Length > 1)
                {
                    Debug.LogError("Multiple instances of ScoreManager detected");
                }
                instance = FindObjectOfType<ScoreManager>();
                Debug.Log("Score Manager Set");
            }
            return instance;
        }
    }
		
    public static Monitored<float> totalScore = new Monitored<float>(0);

	public UnityEvent ScoreTargetIncrease;

    [SerializeField]
    float startingTargetPower = 1f;
    float targetPower;

    float currentMorale = 1f;

    float fillTarget;
    float fillValue;

    float currentPower;

    [SerializeField]
    float energyProgressionTimerTarget = 10f;
    float energyProgressionTimer = 0f;

    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text multiplierText;
    [SerializeField]
    Image timerFill;
    [SerializeField]
    Image lightBulb;
    [SerializeField]
    Sprite bulbOn;
    [SerializeField]
    Sprite bulbOff;

    [SerializeField]
    bool debug;

    // Use this for initialization
    void Start()
    {
        targetPower = startingTargetPower;
		scoreQueue = new Queue<MoraleChangeMessage> ();

		Dispatcher<MoraleChangeMessage>.Subscribe (AddMessageToQueue);
    }

    // Update is called once per frame
    void Update()
    {
        ResetPower();
		ProcessScoreChangeQueue ();
        if (!TutorialProgression.Instance.ProgressPause)
        {
            TargetProgression();
            ScoreProgression();
            UIUpdate();
        }
        scoreText.text = Mathf.Floor(totalScore.value).ToString();
    }

    void OnDestroy()
    {
        instance = null;
    }

    void UIUpdate()
    {
        float currentFill = currentPower - (targetPower);
        if (debug) Debug.Log("Current Power: " + currentPower + " Target Power: " + targetPower + " Current Fill:" + currentFill + " Morale: " + currentMorale);
        //fillValue = Mathf.Lerp(fillValue, currentFill, 0.2f);

        timerFill.fillAmount = currentFill;
        multiplierText.text = "x" + Mathf.Floor(currentMorale).ToString();
    }

    void ResetPower()
    {
        currentPower = 0;
        foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        {
            currentPower += turbine.GetPowerOutput();
        }
    }

    void TargetProgression()
    {
        if (currentPower >= targetPower)
        {
            targetPower += 1f;
			Dispatcher<MoraleChangeMessage>.Dispatch (new MoraleChangeMessage (gameObject, 0.1f));
			ProcessScoreChangeQueue ();
            ScoreTargetIncrease.Invoke();
        }
    }

    void ScoreProgression()
    {
        energyProgressionTimer += Time.deltaTime;

        if (energyProgressionTimer >= energyProgressionTimerTarget)
        {
            float value = currentPower * currentMorale;
            totalScore.value += value;
            energyProgressionTimer = 0f;
        }
    }

    public void EventScore(float value)
    {
        totalScore.value = value;
    }

	#region Message Queue and Processing

	Queue<MoraleChangeMessage> scoreQueue;

	void ProcessScoreChangeQueue()
	{
		if (scoreQueue.Count > 0) {
			foreach (MoraleChangeMessage stm in scoreQueue) {
				currentMorale += stm.scoreChange;
			}
			scoreQueue.Clear ();
		}
	}

	void AddMessageToQueue(MoraleChangeMessage stm)
	{
		scoreQueue.Enqueue (stm);
	}

	#endregion
}

public class MoraleChangeMessage : Message
{
	public readonly float scoreChange;

	public MoraleChangeMessage(GameObject sender, float valueChange):base(sender)
	{
		scoreChange = valueChange;
	}
}