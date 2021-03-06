﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cow : MonoBehaviour , ITouchSensitive,IMouseSensitive {

	[SerializeField] Animator anim;
	[SerializeField] Transform[] goals;
	[SerializeField] Transform[] runGoals;
	[SerializeField] float normalSpeed = 2f;
	[SerializeField] float runSpeed = 10f;

	[SerializeField] Sprite Angry;
	[SerializeField] Sprite Afraid;
 
	[SerializeField] Image emotionImage;
	[SerializeField] Cutscene touch;


	bool run = false;
	bool reachedGoal = false;

	NavMeshAgent agent;

	void Start ()
	{
		
		agent = GetComponent<NavMeshAgent> ();
		Emotion (0);
	}

	void Update ()
	{
		anim.SetFloat ("Speed", agent.velocity.magnitude);

		float distance = Vector3.Distance (this.transform.position, agent.destination);
		if (distance < 1)
		{
			reachedGoal = true;
			agent.speed = 0;
			agent.Stop ();
			run = false;
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
		if (run == false)
		{
			agent.speed = normalSpeed;
			agent.destination = goals[Random.Range (0, goals.Length)].position;
			agent.Resume ();
		}
	}

	public void RunAway()
	{
		Transform saveGoal = runGoals [0];

		float dist = Vector3.Distance (saveGoal.position, this.transform.position);

		foreach (var item in runGoals)
		{
			if (Vector3.Distance(item.position, this.transform.position) < dist)
			{
				dist = Vector3.Distance (item.position,  this.transform.position);
				saveGoal = item;
			}
		}
		RunGoal (saveGoal);
	}

	public void RunGoal(Transform pGoal)
	{
		agent.destination = pGoal.position;
		agent.speed = runSpeed;
		agent.Resume ();
		run = true;
	}

	public void stopRunning()
	{
		run = false;
		reachedGoal = false;
		agent.speed = normalSpeed;
		agent.speed = 0;
		agent.Stop ();

	}

	public void Emotion(int pEmo)
	{
		if (pEmo == 0)
		{
			emotionImage.sprite = null;
			emotionImage.color = new Color (1, 1, 1, 0);
		}
		else if (pEmo == 3)
		{
			//emotionImage.sprite = Happy;
		}
		else if (pEmo == 2)
		{
			emotionImage.sprite = Angry;
			emotionImage.color = new Color (1, 1, 1, 1);
		}
		else if (pEmo == 1)
		{
			emotionImage.sprite = Afraid;
			emotionImage.color = new Color (1, 1, 1, 1);
		}
	}
		
	public void OnTouch(Touch t, RaycastHit hit, Ray ray)
	{
		touch.StartScene ();
	}

	public void OnClick(ClickState state, RaycastHit hit, Ray ray)
	{
		touch.StartScene ();
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
