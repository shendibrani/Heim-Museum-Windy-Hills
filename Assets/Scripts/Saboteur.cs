using UnityEngine;
using System.Collections;

public class Saboteur : MonoBehaviour {

	Vector3 _startPosition;
	float _end = 19.95f;

 	[SerializeField] float Speed = 0f;
	[SerializeField] Animator Anim;

	float _progress = 0;
	bool _isDone = false;

	void Start()
	{
		_startPosition = transform.localPosition;
		Anim.SetInteger ("Type", 1);
	}

	void Update ()
	{
		if (_progress > 1)
		{
			Anim.SetBool ("Succes", true);
			Speed = 0;
			_progress = 0;

			if (! _isDone)
			{
				transform.localPosition = new Vector3(0,29.74f,0);
				transform.localRotation = Quaternion.identity;
				_isDone = true;
			}
		}
		else if (_progress < 0)
		{
			Speed = 0;
			_progress = 0;
			gameObject.SetActive (false);
		}
		else
		{
			_progress += Speed;
			transform.localPosition = _startPosition + Mathf.Lerp(_startPosition.y,_end,_progress) * Vector3.up;
		}
	}

	public void StartAnimation()
	{
		_progress = 0;
		transform.localPosition = _startPosition;
		gameObject.SetActive(true);
		Anim.SetBool ("Climbing", true);
		Speed = 0.001f;
	}

	public void EndAnimation()
	{
		Anim.SetBool ("Defeated", true);
		Speed = -0.01f;
	}
}
