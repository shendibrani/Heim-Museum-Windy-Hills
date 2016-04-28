using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerHUDManager : MonoBehaviour {

    [SerializeField]
    float _targetPower = 5f;

    float _currentPower;

    [SerializeField]
    Slider _powerFillBar;

    [SerializeField]
    Text _debugPowerText;
   
	// Use this for initialization
	void Start () {
        _powerFillBar.minValue = 0;
        _powerFillBar.maxValue = 5;
	}
	
    //When an object is place, redo all calculations concerning power supply
    public void ResetPower()
    {
        _currentPower = 0;
        foreach (TurbineObject turbine in FindObjectsOfType<TurbineObject>())
        {
            _currentPower += turbine.WindDetectionRaycast();
        }
    }

	// Update is called once per frame
	void Update () {
        if (_powerFillBar != null)
        {
            if (_currentPower == 0) _powerFillBar.value = 0;
            else
            {
                //_powerFillBar.value = _targetPower / _currentPower;
                _powerFillBar.value = _currentPower;
            }
        }

        if (_debugPowerText != null)
        {
            _debugPowerText.text =  _currentPower + "/"  + _targetPower;

            //score calculated as 0 to 100, determined by ratio between actual placed turbines vs minimum required 
        }
	}


}
