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
    float greyPowerIncome = 5f;
    float greenPowerIncome;

    float currentGreenPower;
    float currentGreyPower;

    [SerializeField]
    Image _powerFillBar;

    [SerializeField]
    Text _debugPowerText;

    // Use this for initialization
    void Start()
    {
        _powerFillBar.fillAmount = 0;
        currentGreenPower = 0;
        currentGreyPower = 0;
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

        //currentGreyPower += greyPowerIncome;
        //_currentPower = 0;
        //foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        //{
        //    _currentPower += turbine.GetPowerOutput();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        ReadPower();
        if (_powerFillBar != null)
        {
            if (greenPowerIncome + greyPowerIncome == 0) _powerFillBar.fillAmount = 0;
            else
            {
                _powerFillBar.fillAmount = greenPowerIncome / 10;
                //_powerFillBar.value = _currentPower;
            }
        }

        /*if (_debugPowerText != null)
        {
            _debugPowerText.text = _currentPower + "/" + _targetPower;

            //score calculated as 0 to 100, determined by ratio between actual placed turbines vs minimum required 
        }*/
    }


}
