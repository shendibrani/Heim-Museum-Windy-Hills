using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerHUDManager : MonoBehaviour {

    //static reference to the instance of MoraleHudManger in scene
    public static PowerHUDManager instance;
    public static PowerHUDManager Instance
    {
        get
        {
            if (instance == null)
            {
                if (FindObjectsOfType<PowerHUDManager>().Length > 1)
                {
                    Debug.LogError("Multiple instances of PowerHUDManager detected");
                }
                instance = FindObjectOfType<PowerHUDManager>();
                Debug.Log("Power Manager Set");
            }
            return instance;
        }
    }

    [SerializeField]
    float _targetPower = 5f;

    float _currentPower;

    [SerializeField]
    Image _powerFillBar;

    [SerializeField]
    Text _debugPowerText;
   
	// Use this for initialization
	void Start () {
        _powerFillBar.fillAmount = 0;
        //_powerFillBar.minValue = 0;
        //_powerFillBar.maxValue = 5;
        if (FindObjectsOfType<PowerHUDManager>().Length > 1){
			Debug.LogError("Multiple instances of PowerHUDManager detected");
		}
	}
	
    void OnDestroy()
    {
        instance = null;
    }

    //When an object is place, redo all calculations concerning power supply
	void ResetPower()
    {
        _currentPower = 0;
        foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        {
            _currentPower += turbine.GetPowerOutput();
        }
    }

	// Update is called once per frame
	void Update () {
        ResetPower();
        if (_powerFillBar != null)
        {
            if (_currentPower == 0) _powerFillBar.fillAmount = 0;
            else
            {
                _powerFillBar.fillAmount = _currentPower / _targetPower;
                //_powerFillBar.value = _currentPower;
            }
        }

        if (_debugPowerText != null)
        {
            _debugPowerText.text =  _currentPower + "/"  + _targetPower;

            //score calculated as 0 to 100, determined by ratio between actual placed turbines vs minimum required 
        }
	}


}
