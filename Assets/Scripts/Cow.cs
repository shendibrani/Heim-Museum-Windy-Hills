using UnityEngine;
using System.Collections;

public class Cow : MonoBehaviour {

	[SerializeField] Animator anim;
	[SerializeField] Transform[] goals;
	[SerializeField] Transform[] runGoals;
	[SerializeField] float normalSpeed = 0.04f;
	[SerializeField] float runSpeed = 0.1f;

	int run = 0;
	bool reachedGoal = false;

	NavMeshAgent agent;

	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update ()
	{
		anim.SetFloat ("Speed", agent.velocity.magnitude);

		if (run == 2) {
			float distance = Vector3.Distance (this.transform.position, agent.destination);
			if (distance < 1) {
				reachedGoal = true;
			}
		}
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
		if (run == 0)
		{
			agent.speed = normalSpeed;
			agent.destination = goals[Random.Range (0, goals.Length)].position;
		}
	}

	public void Run()
	{
		agent.destination = runGoals [0].position;

		float dist = Vector3.Distance (agent.destination, this.transform.position);

		foreach (var item in runGoals)
		{
			if (Vector3.Distance(item.position, this.transform.position) < dist)
			{
				dist = Vector3.Distance (item.position,  this.transform.position);
				agent.destination = item.position;
			}
		}
		agent.speed = runSpeed;
		run = 1;
	}

	public void stopRunning()
	{
		run = 0;
		reachedGoal = false;
		agent.speed = normalSpeed;
	}

	public void RunBack()
	{
		run = 2;
		reachedGoal = false;
		agent.destination = goals[Random.Range (0, goals.Length)].position;
	}

	public bool FinishedWalking()
	{
		return reachedGoal;
	}

	public void GetReference(Cutscene pScript)
	{
		pScript.SetBoolReference (FinishedWalking);
	}
}
