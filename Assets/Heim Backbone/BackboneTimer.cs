using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Arguments))]
public class BackboneTimer : MonoBehaviour {

	float targetTime;
	float shutDownTargetTimer;
	float timer;

	[SerializeField]
	float countDownTime = 10;
	[SerializeField]
	float endScreenTime = 8;

	[SerializeField]
	Text timerText;

	// Use this for initialization
	void Start () {
		timerText.gameObject.SetActive(false);
		SetTime(GetComponent<Arguments>().getGameTime());
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > shutDownTargetTimer)
		{
			shutDownTimeResponse();
		}
		if  (timer > targetTime)
		{
			endGameTimeResponse();
		}
	}

	public void SetTime(float pTime)
	{
		targetTime = (pTime * 60000) - (endScreenTime * 1000);
		shutDownTargetTimer = targetTime - (countDownTime * 1000);
		timer = 0;
	}

	void shutDownTimeResponse()
	{
		timerText.gameObject.SetActive(true);
		timerText.text = Mathf.CeilToInt((timer - targetTime) / 1000).ToString();	
	}

	void endGameTimeResponse()
	{
		FindObjectOfType<GameOverScript>().EndGame();
	}
}
