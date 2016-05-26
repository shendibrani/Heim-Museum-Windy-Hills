using UnityEngine;
using System.Collections;

public class PigeonAnimator : MonoBehaviour {

	Animator _anim;

	// Use this for initialization
	void Start ()
	{
		_anim = GetComponent<Animator> ();
		_anim.SetFloat("SpeedRandom",Random.Range(0.5f,1.5f));
	}

	public void Flap()
	{
		if (_anim.GetBool("Flap") == false)
		{
			_anim.SetBool ("Flap", true);
		}

		else
		{
			_anim.SetBool ("Flap", false);
		}
	}
}
