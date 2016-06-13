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

    public float CityPower
    {
        get
        {
            return cityPower;
        }

        set
        {
            cityPower = value;
        }
    }

    public float MaximumCityPower
    {
        get
        {
            return maximumCityPower;
        }

        set
        {
            maximumCityPower = value;
        }
    }

    public static Monitored<float> totalScore = new Monitored<float>(0);

    [SerializeField]
    float startCityPower = 5f;
    [SerializeField]
    float maximumCityPower = 11f;
    float cityPower;

    int newTurbineCount;
    [SerializeField]
    int newTurbineTarget = 3;
    float currentMorale = 1f;

    float currentPower;

    [SerializeField]
    float energyProgressionTimerTarget = 10f;
    float energyProgressionTimer = 0f;

    [SerializeField]
    float cityCheckTimerTarget = 10f;
    float cityCheckTimer = 0f;
    bool hasCheckedCity;
    bool hasPositivePower;
    bool hasNegativePower;

    [SerializeField]
    Text scoreText;
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
        cityPower = startCityPower;
    }

    // Update is called once per frame
    void Update()
    {
        ResetPower();
        CityProgression();
        EnergyProgression();
        if (newTurbineCount == newTurbineTarget)
        {
            newTurbineCount = 0;
            TurbineLimitManager.Instance.IncreaseMax();
        }
        scoreText.text = Mathf.Floor(totalScore.value).ToString();
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void MoraleUpdate(float value)
    {
        currentMorale += value;
    }

    void ResetPower()
    {
        currentPower = 0;
        foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        {
            currentPower += turbine.GetPowerOutput();
        }
    }

    void CityProgression()
    {
        cityCheckTimer += Time.deltaTime;
        timerFill.fillAmount = cityCheckTimer / cityCheckTimerTarget;

        if (currentPower >= cityPower)
        {
            if (debug) Debug.Log("Current Power > City Power");
            lightBulb.sprite = bulbOn;
            hasPositivePower = true;
            hasNegativePower = false;

        }
        if (currentPower < cityPower)
        {
            if (debug) Debug.Log("Current Power < City Power");
            lightBulb.sprite = bulbOff;
            hasPositivePower = false;
            hasNegativePower = true;
        }

        if (cityCheckTimer >= cityCheckTimerTarget)
        {
            if (hasPositivePower)
            {
                    if (debug) Debug.Log("Increase City Power");
                    cityPower += 1;
                    //newTurbineCount += 1;
            }
            if (hasNegativePower)
            {
                    if (debug) Debug.Log("Decrease City Power");
                    cityPower -= 1;
                    //newTurbineCount -= 1;
            }
            cityCheckTimer = 0;
        }
    }

    void EnergyProgression()
    {
        energyProgressionTimer += Time.deltaTime;

        if (energyProgressionTimer >= energyProgressionTimerTarget)
        {
            float value = currentPower *= currentMorale;
            totalScore.value += value;
            energyProgressionTimer = 0f;
        }
    }

    public void EventScore(float value)
    {
        totalScore.value = value;
    }
}
