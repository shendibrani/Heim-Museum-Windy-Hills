using UnityEngine;
using System.Collections;

public class AnimationTriggerBehaviour : MonoBehaviour 
{
	bool wait;

	void Update()
	{
		if (wait)
		{
			wait = false;
		}
		else
		{
			GetComponent<Animator>().SetBool ("Active", false);
		}

	}

	public void TriggerAnimation(string name)
	{
		GetComponent<Animator>().SetBool (name, true);
		wait = true;
	}
}
