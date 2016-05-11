using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoraleHUDManager : MonoBehaviour {

    float totalMorale = 1f;

    float currentMorale;

    [SerializeField]
    Image moraleFillBar;

    [SerializeField]
    Text debugMoraleText;

    // Use this for initialization
    void Start()
    {
        moraleFillBar.fillAmount = 1f;
        currentMorale = 1f;

        if (FindObjectsOfType<MoraleHUDManager>().Length > 1)
        {
            Debug.LogError("Multiple instances of MoraleHUDManager detected");
        }
    }

    public void MoraleUpdate(float value)
    {
        currentMorale += value;
    }

    // Update is called once per frame
    void Update()
    {
        if (moraleFillBar != null)
        {
            if (currentMorale == 0) moraleFillBar.fillAmount = 0;
            else
            {
                moraleFillBar.fillAmount = currentMorale / totalMorale;
                //_powerFillBar.value = _currentPower;
            }
        }

        if (debugMoraleText != null)
        {
            debugMoraleText.text = currentMorale + "/" + totalMorale;

            //score calculated as 0 to 100, determined by ratio between actual placed turbines vs minimum required 
        }
    }


}
