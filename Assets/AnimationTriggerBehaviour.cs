using UnityEngine;
using System.Collections;

public class AnimationTriggerBehaviour : MonoBehaviour 
{
	public void TriggerAnimation(string name)
	{
		GetComponent<Animator>().SetBool (name, true);
	}
}
