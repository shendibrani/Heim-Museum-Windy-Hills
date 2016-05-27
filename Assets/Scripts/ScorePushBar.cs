using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePushBar : MonoBehaviour
{

    public static ScorePushBar instance;
    public static ScorePushBar Instance
    {
        get
        {
            if (instance == null)
            {
                if (FindObjectsOfType<ScorePushBar>().Length > 1)
                {
                    Debug.LogError("Multiple instances of ScorePushBar detected");
                }
                instance = FindObjectOfType<ScorePushBar>();
                Debug.Log("ScorePushBar Set");
            }
            return instance;
        }
    }

    [SerializeField]
    float maxRequired = 10f;

    float greenPowerIncome;

    [SerializeField]
    Image _powerFillBar;

    [SerializeField]
    Text _debugPowerText;

    // Use this for initialization
    void Start()
    {
        _powerFillBar.fillAmount = 0;
        if (FindObjectsOfType<ScorePushBar>().Length > 1)
        {
            Debug.LogError("Multiple instances of ScorePushBar detected");
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    //When an object is place, redo all calculations concerning power supply
    void ReadPower()
    {
        greenPowerIncome = 0;
        foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        {
            greenPowerIncome += turbine.GetPowerOutput();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReadPower();
        if (_powerFillBar != null)
        {
            if (greenPowerIncome == 0) _powerFillBar.fillAmount = 0;
            else
            {
                _powerFillBar.fillAmount =  greenPowerIncome / (ScoreManager.Instance.CityPower * 2);
            }
        }
    }


}
