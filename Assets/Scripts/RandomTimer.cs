using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RandomTimer : MonoBehaviour {

	float timer;

	[SerializeField] int minSeconds, maxSeconds;

	[SerializeField] UnityEvent TimerEnd;

	// Use this for initialization
	void Start () {
		timer = (RNG.NextFloat() * RNG.Next(0,maxSeconds-minSeconds)) + minSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if(timer <= 0){
			TimerEnd.Invoke();
			timer = (RNG.NextFloat() * RNG.Next(0,maxSeconds-minSeconds)) + minSeconds;
		}
	}
}
