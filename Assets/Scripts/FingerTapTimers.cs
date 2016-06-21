using UnityEngine;
using System.Collections;

public class FingerTapTimers : MonoBehaviour {

	bool sceneRunning = false;

	int subStep = 0;

	bool end = false;
	float timer;

	public int activationTime;
	GameObject Aim;

	void Update ()
	{
		if (sceneRunning)
		{
			if (subStep == 0)
			{
				timer = 0;
				subStep = 1;
			}
			if (subStep == 1)
			{
				if (timer < activationTime)
				{
					timer += Time.deltaTime;
				}
				else
				{
					Target (Aim);
				}
			}
		}
	}

	public void StartScene(GameObject pAim)
	{
		Aim = pAim;
		if (!sceneRunning)
		{
			sceneRunning = true;
		}
		else
		{
			subStep = 0;
		}
	}

	public void EndScene()
	{
		EndTapFinger ();
		sceneRunning = false;
	}

	void Target(GameObject pRectTransform)
	{
		if (pRectTransform.GetComponent<RectTransform> () != null) 
		{
			transform.position = pRectTransform.GetComponent<RectTransform> ().position;
		}
		else if (pRectTransform.GetComponent<Transform> () != null)
		{
			transform.position = Camera.main.WorldToScreenPoint (pRectTransform.transform.position);
		}
		GetComponent<Animator> ().SetBool ("Active", true);
	}

	void EndTapFinger()
	{
		GetComponent<Animator> ().SetBool ("Active",false);
	}
}
