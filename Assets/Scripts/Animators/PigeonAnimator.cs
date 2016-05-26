using UnityEngine;
using System.Collections;

public class PigeonAnimator : MonoBehaviour {

	Animator _anim;
	Vector3 _goal;

	// Use this for initialization
	void Start ()
	{
		_anim = GetComponent<Animator> ();
		RandomVal ();
	}

	void Update()
	{
		
		transform.localPosition = Vector3.Lerp (transform.localPosition, _goal, 0.001f);
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
		RandomVal ();
	}

	void RandomVal()
	{
		_anim.SetFloat("SpeedRandom",Random.Range(0.5f,1.3f));
		_goal = new Vector3 (Random.Range (-2, 2), 0, Random.Range (-2, 2));
		Debug.Log (_goal);
	}
}
