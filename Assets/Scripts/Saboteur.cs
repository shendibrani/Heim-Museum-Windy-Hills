using UnityEngine;
using System.Collections;

public class Saboteur : MonoBehaviour {

	Animator _anim;
	float _start = -0.77f;
	float _end = 19.95f;

 	[SerializeField] float Speed = 0.01f;

	public float Progress = 0;

	void Update ()
	{
		// += speed is for testingl use the public Progress for final verison
		Progress += Speed;
		this.transform.localPosition = new Vector3(-2,Mathf.Lerp(_start,_end,Progress),0);
	}
}
