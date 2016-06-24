using UnityEngine;
using System.Collections;

public class FingerTapTimers : MonoBehaviour {

	[SerializeField] bool debug = false;

	bool sceneRunning = false;

	int subStep = 0;

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
					if (debug)  Debug.Log (timer);
				}
				else
				{
					Target (Aim);
					subStep = 2;
				}
			}
		}
	}

	public void StartScene(GameObject pAim)
	{
		if (debug) Debug.Log (this.gameObject.name + "Started");
		Aim = pAim;
		subStep = 0;
		timer = 0;
		sceneRunning = true;
	}

	public void EndScene()
	{
		if (debug) Debug.Log (this.gameObject.name + "Ended");
		GetComponent<Animator> ().SetBool ("Active",false);
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
			if (pRectTransform.GetComponent<TurbineObject>() != null)
			{
				transform.position = Camera.main.WorldToScreenPoint (pRectTransform.transform.position);
			}
			else
			{
				transform.position = Camera.main.WorldToScreenPoint (pRectTransform.transform.position);
			}
		}
		GetComponent<Animator> ().SetBool ("Active", true);
	}
}
