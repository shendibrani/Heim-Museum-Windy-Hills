using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    [SerializeField]
    float startingTargetPower = 1f;
    float targetPower;

    float currentMorale = 1f;
    float moraleChange;

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
    }

    // Update is called once per frame
    void Update()
    {
        ResetPower();
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
        float currentFill = currentPower - (targetPower - 1);
        if (debug) Debug.Log("Current Power: " + currentPower + " Target Power: " + targetPower + " Current Fill:" + currentFill + " Morale: " + currentMorale);
        //fillValue = Mathf.Lerp(fillValue, currentFill, 0.2f);

        timerFill.fillAmount = currentFill;
        multiplierText.text = "x" + Mathf.Floor(currentMorale).ToString();
    }

    public void MoraleUpdate(float value)
    {
        moraleChange += value;
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
            MoraleUpdate(0.1f);
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
}
