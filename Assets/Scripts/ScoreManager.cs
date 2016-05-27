using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

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

    public static Monitored<float> totalScore = new Monitored<float>(0);

    [SerializeField]
    float startCityPower = 10f;
    float cityPower;

    float currentMorale = 1f;

    float currentPower;

    [SerializeField]
    float energyProgressionTimerTarget = 10f;
    float energyProgressionTimer = 0f;

    [SerializeField]
    float cityCheckTimerTarget = 10f;
    float cityCheckTimer = 0f;
    bool hasCheckedCity;
    bool hasPositiveTimer;
    bool hasNegativeTimer;

    [SerializeField]
    Text scoreText;

    // Use this for initialization
    void Start () {
        cityPower = startCityPower;
	}
	
	// Update is called once per frame
	void Update () {
        ResetPower();
        CityProgression();
        EnergyProgression();
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
        if (!hasCheckedCity)
        {
            if (currentPower > cityPower)
            {
                hasPositiveTimer = true;
                hasNegativeTimer = false;
                hasCheckedCity = true;
            }
            if (currentPower < cityPower)
            {
                hasNegativeTimer = true;
                hasPositiveTimer = false;
                hasCheckedCity = true; 
            }
            cityCheckTimer = 0;
        }
        else if (hasCheckedCity)
        {
            cityCheckTimer += Time.deltaTime;
            if (cityCheckTimer >= cityCheckTimerTarget)
            {
                if (hasPositiveTimer)
                {
                    hasCheckedCity = false;
                    if (currentPower > cityPower)
                    {
                        cityPower += 1;
                    }
                }
                if (hasNegativeTimer)
                {
                    hasCheckedCity = false;
                    if (currentPower < cityPower)
                    {
                        cityPower -= 1;
                    }
                }
            }
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
