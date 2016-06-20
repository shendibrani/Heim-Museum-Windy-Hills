using UnityEngine;
using System.Collections;

public class CityLevelManager : MonoBehaviour
{
	[SerializeField] GameObject[] cityLevels;

	int currentLevel;

	// Use this for initialization
	void Start ()
	{
		currentLevel = 0;

		foreach (GameObject go in cityLevels) {
			go.SetActive (false);
		}

		cityLevels [currentLevel].SetActive (true);
	}
	
	public void IncreaseCityLevel()
	{
		currentLevel++;

		foreach (GameObject go in cityLevels) {
			go.SetActive (false);
		}

		cityLevels [currentLevel].SetActive (true);
	}
}

