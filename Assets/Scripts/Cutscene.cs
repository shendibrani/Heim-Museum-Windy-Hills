using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour {

	bool sceneRunning = false;
	int step = 0;

	bool actionRunning = false;

	bool end = false;
	public float timer { get; private set; }

	GradingParameters gp;

	public delegate bool CheckForEnd();
	CheckForEnd onCheckForEnd;

	[System.Serializable] struct actions
	{
		public UnityEvent StartEvent;

		public bool IsEndedByBool;
		public UnityEvent SetReference;

		public bool IsEndedByTimer;
		public float TimeTarget;
	}

	[SerializeField] actions[] Actions;
	[SerializeField] actions OnEnd;

	void Update ()
	{
		if (sceneRunning)
		{
			if (!actionRunning)
			{
				Actions [step].StartEvent.Invoke ();
				Actions [step].SetReference.Invoke ();
				actionRunning = true;
				timer = 0;
			}
			else if (actionRunning)
			{
				if (Actions[step].IsEndedByBool)
				{
					end = onCheckForEnd ();
				}
				if (Actions[step].IsEndedByTimer)
				{
					timer += Time.deltaTime;
					if (timer >= Actions[step].TimeTarget)
					{
						end = true;
					}
				}
			}
			if (end)
			{
				actionRunning = false;
				end = false;
				if (step + 1 < Actions.Length)
				{
					step ++;
				}
				else
				{
					EndScene ();
				}
			}
		}
	}

	public void StartScene()
	{
		if (!sceneRunning)
		{
			sceneRunning = true;
			step = 0;
		}
		else
		{
			actionRunning = false;
			step = 0;
		}
	}

	public void EndScene()
	{
		OnEnd.StartEvent.Invoke ();
		sceneRunning = false;
	}

	public void SetBoolReference(CheckForEnd pCheckForEnd)
	{
		onCheckForEnd = pCheckForEnd;
	}
}