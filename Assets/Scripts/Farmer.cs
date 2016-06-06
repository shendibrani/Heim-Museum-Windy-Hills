using UnityEngine;
using System.Collections;

public class Farmer : MonoBehaviour {

	[SerializeField] Animator anim;
	[SerializeField] float minSpeed;
	[SerializeField] float normalSpeed;
	//[SerializeField] float runSpeed = 0.1f;

	[SerializeField] float stopDist;

	bool move = false;
	bool turn = false;
	//bool run = false;

	float maxDistance;
	Vector3 goal = Vector3.zero;
	Transform turnTarget;
	float currentspeed;
	float goalspeed;

	void Start ()
	{
		currentspeed = 0;
		anim.SetInteger ("Type", 0);
	}

	void Update ()
	{
		if (move)
		{
			Quaternion currentRot = this.transform.rotation;
			this.transform.LookAt (goal);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, currentRot,0.99f);
			this.transform.position += currentspeed *  this.transform.forward;

			float distance = Vector3.Distance (this.transform.position, goal);

			if (distance < stopDist)
			{
				move = false;
				TurnAround (Camera.main.transform);
				Wave (true);
				currentspeed = 0;
			}
			else
			{
				goalspeed = Mathf.Lerp( minSpeed,normalSpeed,(distance / maxDistance));
				currentspeed = Mathf.Lerp (currentspeed, goalspeed, 0.1f);
				Debug.Log (distance);
			}

			/*if (run)
			{
				if (distance < 9)
				{
					run = false;
					move = true;
				}
			}*/
		}
		if (turn)
		{
			Quaternion currentRot = this.transform.rotation;
			this.transform.LookAt (turnTarget);

			if (Quaternion.Angle(currentRot,transform.rotation) < 3)
			{
				turn = false;
			}
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, currentRot,0.5f);
			this.transform.rotation = Quaternion.Euler( new Vector3 (currentRot.x, this.transform.rotation.eulerAngles.y, currentRot.z));
		}
	}


	public void Walk(Vector3 pGoal)
	{
		if (!move)
		{
			Debug.Log ("thing");
			goal = pGoal;
			maxDistance = Vector3.Distance (this.transform.position, goal);
			currentspeed = normalSpeed;
			move = true;
			Wave (false);
		}
	}

	public void Wave(bool pWave)
	{
		anim.SetBool ("Calling", pWave);
	}

	public void TurnAround(Transform pTarget)
	{
		turnTarget = pTarget;
		turn = true;
	}

	/*public void Run()
	{
		goal = runGoals [0].position;

		float dist = Vector3.Distance (goal, this.transform.position);

		foreach (var item in runGoals)
		{
			if (Vector3.Distance(item.position, this.transform.position) < dist)
			{
				dist = Vector3.Distance (item.position,  this.transform.position);
				goal = item.position;
			}
		}
		maxDistance = Vector3.Distance (this.transform.position, goal);
		move = false;
		run = true;
	}*/
}
