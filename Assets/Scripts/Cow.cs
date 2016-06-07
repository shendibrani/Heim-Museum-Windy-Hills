using UnityEngine;
using System.Collections;

public class Cow : MonoBehaviour {

	[SerializeField] Animator anim;
	[SerializeField] Transform[] goals;
	[SerializeField] Transform[] runGoals;
	[SerializeField] float minSpeed;
	float normalSpeed = 0.04f;
	[SerializeField] float runSpeed = 0.1f;

	[SerializeField] float stopDist;

	bool move = false;
	bool run = false;

	float maxDistance;
	Vector3 goal = Vector3.zero;
	float currentspeed;
	float goalspeed;

	void Start ()
	{
		currentspeed = 0;
	}

	void Update ()
	{
		if (move || run)
		{
			Quaternion currentRot = this.transform.rotation;
			this.transform.LookAt (goal);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, currentRot,0.99f);
			this.transform.position += currentspeed * this.transform.forward;

			float distance = Vector3.Distance (this.transform.position, goal);

			if (distance < stopDist)
			{
				move = false;
				currentspeed = 0;
			}
			else
			{
				if (!run)
				{
					goalspeed = Mathf.Lerp( minSpeed,normalSpeed,(distance / maxDistance));
				}
				else
				{
					goalspeed = Mathf.Lerp( minSpeed,runSpeed,(distance / maxDistance));
				}
				currentspeed = Mathf.Lerp (currentspeed, goalspeed, 0.1f);
			}

			if (run)
			{
				if (distance < 9)
				{
					run = false;
					move = true;
				}
			}
		}


		anim.SetBool ("Walk", move);
		anim.SetBool ("Run", run);
	}

	public void Action()
	{
		int random = Random.Range (0, 2);
		if (random == 0)
		{
			Eat ();
		}
		else
		{
			Walk ();
		}
	}

	void Eat ()
	{
		if (anim.GetBool ("Eating"))
		{
			anim.SetBool ("Eating", false);
		}
		else
		{
			anim.SetBool ("Eating", true);
		}
	}
	void Walk()
	{
		if (!move && !run)
		{
			goal = goals[Random.Range (0, goals.Length)].position;
			maxDistance = Vector3.Distance (this.transform.position, goal);
			currentspeed = normalSpeed;
			move = true;
		}
	}

	public void Run()
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
	}
}
