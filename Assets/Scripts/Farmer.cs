using UnityEngine;
using System.Collections;

public class Farmer : MonoBehaviour {

	[SerializeField] Animator anim;
//	[SerializeField] float minSpeed;
//	[SerializeField] float normalSpeed;
	//[SerializeField] float runSpeed = 0.1f;
//	[SerializeField] float evadeDistance;

//	[SerializeField] float stopDist;

	bool move = false;
	bool turn = false;
	//bool run = false;

	bool isFinished = true;

	float maxDistance;
	Transform turnTarget;
	float currentspeed;
	float goalspeed;
	NavMeshAgent agent;



	void Start ()
	{
		currentspeed = 0;
		anim.SetInteger ("Type", 0);
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update ()
	{
		anim.SetFloat ("Speed", agent.velocity.magnitude);

		if (!isFinished)
		{
			//Quaternion currentRot = this.transform.rotation;
			/*this.transform.LookAt (goal);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, currentRot,0.99f);
			this.transform.position += currentspeed *  this.transform.forward;
*/
			float distance = Vector3.Distance (this.transform.position, agent.destination);
			if (distance < 0.1f)
			{
				isFinished = true;
				Debug.Log ("tralala");
			}

			/*else
			{
				goalspeed = Mathf.Lerp( minSpeed,normalSpeed,(distance / maxDistance));
				currentspeed = Mathf.Lerp (currentspeed, goalspeed, 0.1f);
			}*/

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


	public void StartWalk(Transform pGoal)
	{
		agent.ResetPath ();
		agent.SetDestination(pGoal.position);
		isFinished = false;
		//maxDistance = Vector3.Distance (this.transform.position, goal);
		//currentspeed = normalSpeed;
		//move = true;
	}

	public void Wave(bool pWave)
	{
		anim.SetInteger ("FarmerState", 1);
	}

	public void Angry (bool pAngry)
	{
		anim.SetInteger ("FarmerState", 2);
	}

	public void TurnAround(Transform pTarget)
	{
		turnTarget = pTarget;
		turn = true;
	}

	public bool FinishedWalking()
	{
		return isFinished;
	}

	public void GetReference(Cutscene pScript)
	{
		pScript.SetBoolReference (FinishedWalking);
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
