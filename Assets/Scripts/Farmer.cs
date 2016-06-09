using UnityEngine;
using System.Collections;

public class Farmer : MonoBehaviour, ITouchSensitive {

	[SerializeField] Animator anim;
	[SerializeField] Transform[] points;

	bool move = false;
	bool turn = false;
	bool freeWill = true;

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
		WalkRandom ();
	}

	void Update ()
	{
		anim.SetFloat ("Speed", agent.velocity.magnitude);

		if (!isFinished)
		{
			float distance = Vector3.Distance (this.transform.position, agent.destination);
			if (distance < 0.1f)
			{
				isFinished = true;
			}
		}

		if (turn)
		{
			Quaternion currentRot = this.transform.rotation;
			this.transform.LookAt (turnTarget);

			if (Quaternion.Angle(currentRot,transform.rotation) < 3)
			{
				turn = false;
			}
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, currentRot,0.95f);
			this.transform.rotation = Quaternion.Euler( new Vector3 (currentRot.x, this.transform.rotation.eulerAngles.y, currentRot.z));
		}
	}

	public void WalkRandom()
	{
		if (freeWill)
		{
			int i = Random.Range (0, points.Length);
			Wave (false);
			StartWalk (points [i]);
		}
	}

	public void StartWalk(Transform pGoal)
	{
		agent.SetDestination(pGoal.position);
		isFinished = false;
	}

	public void StopWalk()
	{
		agent.SetDestination(this.transform.position);
		isFinished = true;
	}

	public void Wave(bool pWave)
	{
		if (pWave) {
			anim.SetInteger ("FarmerState", 1);
		} else {
			anim.SetInteger ("FarmerState", 0);
		}
	}

	public void Angry (bool pAngry)
	{
		if (pAngry) {
			anim.SetInteger ("FarmerState", 2);
		} else {
			anim.SetInteger ("FarmerState", 0);
		}

	}

	public void Free(bool pFree)
	{
		freeWill = pFree;
		if (freeWill) {
			turn = false;
			agent.speed = 7;
		} else {
			agent.speed = 30;
		}
	}

	public void TurnAround(Transform pTarget)
	{
		turnTarget = pTarget;
		turn = true;
	}

	public void OnTouch(Touch t, RaycastHit hit)
	{
		Debug.Log ("ouch");
		StopWalk ();
		Wave (true);
	}

	public bool FinishedWalking()
	{
		return isFinished;
	}

	public void GetReference(Cutscene pScript)
	{
		pScript.SetBoolReference (FinishedWalking);
	}
}
