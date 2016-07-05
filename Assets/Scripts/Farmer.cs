using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Farmer : MonoBehaviour, ITouchSensitive,IMouseSensitive {

	[SerializeField] Animator anim;
	[SerializeField] Transform[] points;

	[SerializeField] Sprite Angry;
	[SerializeField] Sprite Afraid;
	[SerializeField] Image emotionImage;

	[SerializeField] bool debug;

	[SerializeField] Cutscene touch;

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
		Emotion (0);
	}

	void Update ()
	{
		anim.SetFloat ("Speed", agent.velocity.magnitude);

		if (!isFinished)
		{
			float distance = Vector3.Distance (this.transform.position, agent.destination);
			if (debug)
			{
				Debug.Log (distance);
			}
			if (distance < 0.6f)
			{
				StopWalk ();
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
			StartWalk (points [i]);
		}
	}

	public void StartWalk(Transform pGoal)
	{
		agent.SetDestination(pGoal.position);
		agent.Resume ();
		isFinished = false;
	}
	public void StopWalk()
	{
		agent.Stop ();
		isFinished = true;
	}

	public void Emotion(int pEmo)
	{
		if (pEmo == 0)
		{
			//emotionImage.sprite = null;
			emotionImage.color = new Color(0,0,0,0);
			anim.SetInteger ("FarmerState", 0);
		}
		else if (pEmo == 3)
		{
			//emotionImage.sprite = Happy;
			anim.SetInteger ("FarmerState", 0);
			emotionImage.color = new Color(1,1,1,1);
		}
		else if (pEmo == 2)
		{
			emotionImage.sprite = Angry;
			anim.SetInteger ("FarmerState", 2);
			emotionImage.color = new Color(1,1,1,1);
		}
		else if (pEmo == 1)
		{
			emotionImage.sprite = Afraid;
			anim.SetInteger ("FarmerState", 1);
			emotionImage.color = new Color(1,1,1,1);
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

	public void OnTouch(Touch t, RaycastHit hit, Ray ray)
	{
		if (freeWill)
		{
			touch.StartScene ();
		}
	}

	public void OnClick(ClickState state, RaycastHit hit, Ray ray)
	{
		if (state == ClickState.Down && freeWill)
		{
			touch.StartScene ();
		}
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
