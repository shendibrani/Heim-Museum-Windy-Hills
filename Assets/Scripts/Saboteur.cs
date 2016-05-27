using UnityEngine;
using System.Collections;

public class Saboteur : MonoBehaviour {

	Animator _anim;
	Vector3 _startPosition;
	float _end = 19.95f;

 	[SerializeField] float Speed = 0.01f;

	float Progress = 0;

	void Start()
	{
		_startPosition = transform.localPosition;
	}

	void Update ()
	{
		// += speed is for testingl use the public Progress for final verison
		Progress += Speed;
		transform.localPosition = _startPosition + Mathf.Lerp(_startPosition.y,_end,Progress) * Vector3.up;
	}

	public void StartAnimation()
	{
		Progress = 0;
		transform.localPosition = _startPosition;
		gameObject.SetActive(true);
	}

	public void EndAnimation()
	{
		gameObject.SetActive(false);
	}
}
