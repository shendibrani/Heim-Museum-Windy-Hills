using UnityEngine;
using System.Collections;

public class HumanAnimator : MonoBehaviour {

	enum Type
	{
		Farmer,
		Saboteur,
	}

	Animator _anim;

	[SerializeField] Type type = Type.Farmer;

	void Start ()
	{
		_anim = GetComponent<Animator> ();

		if (type == Type.Farmer)
		{
			_anim.SetInteger ("Type", 0);
		}
		else if (type == Type.Saboteur)
		{
			_anim.SetInteger ("Type", 1);
			SetClimb(true);
		}
	}

	public void SetClimb(bool climb)
	{
		_anim.SetBool ("Climbing", climb);
	}
}
