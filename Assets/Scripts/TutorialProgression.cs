using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CameraMovement))]
public class TutorialProgression : MonoBehaviour {

     

    static TutorialProgression instance;
    public static TutorialProgression Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<TutorialProgression>();
            return instance;
        }
    }

    public bool IsComplete
    {
        get
        {
            return isComplete;
        }

        private set
        {
            isComplete = value;
        }
    }

    public bool ProgressPause
    {
        get
        {
            return progressPause;
        }

        set
        {
            progressPause = value;
        }
    }

    GameObject badMill;
	TurbineObject savedMill;

	bool dirtyWasfought = false;
	bool fireWasFought = false;
	bool saboteurWasFought = false;
	bool brokenWasFixed = false;

	bool messedUp = false;
	bool hasPlacedMills = false;
	bool isComplete = false;

	int requiredMills = 0;
	int currentMills = 0;

	Animator popup;

    bool progressPause = false;

    [SerializeField] bool debug;
	[SerializeField] Cutscene scene;
	[SerializeField] Text helpText;
	[SerializeField] Image goodJob;
	[SerializeField] bool Skip;
	[SerializeField] GameObject DestroyDust;
	[SerializeField] Transform extraTarget;
	[SerializeField] GameObject birds;

	[SerializeField] Animator[] buttons;

    void Start()
	{
		popup = goodJob.GetComponent<Animator> ();

		PlaceObjectOnClick.Instance.SetDirty(true);
		if (!Skip)
		{
			scene.StartScene ();
		}
	}
	
    public void SetComplete()
    {
        isComplete = true;
    }

    void Update ()
	{
		
		if (Skip)
		{
			PlaceObjectOnClick.Instance.SetDirty (false);
			SetComplete ();
		}

		if (!hasPlacedMills)
		{
			if (currentMills == requiredMills && !messedUp)
			{
				hasPlacedMills = true;
			}
		}
		if (fireWasFought)
		{
			savedMill.state.OnValueChanged -= OnFireEnd;
		}
		if (saboteurWasFought)
		{
			savedMill.state.OnValueChanged -= OnSaboteurEnd;
		}
		if (brokenWasFixed)
		{
			savedMill.state.OnValueChanged -= OnBrokenEnd;
		}
		if (dirtyWasfought)
		{
			savedMill.state.OnValueChanged -= OnDirtEnd;
		}

		if (popup.GetBool("play") && popup.GetCurrentAnimatorStateInfo(0).IsTag("Anim") && popup.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			popup.SetBool ("play", false);
		}
	}

	//For Tutorial Progression
	public void Placed()
	{
		currentMills++;
	}
	public void StepBackPlacement(GameObject turbineObject)
	{
		badMill = turbineObject;
		messedUp = true;
	}
	public void SaveBadMill(GameObject turbineObject)
	{
		badMill = turbineObject;
	}

	public void RequirePlacing(int pReq)
	{
		progressPause = true;
		PlaceObjectOnClick.Instance.SetDirty(false);
		requiredMills = pReq;
		currentMills = 0;
		hasPlacedMills = false;
	}

	public void CanPlaceMills(bool canPlace)
	{
		PlaceObjectOnClick.Instance.SetDirty(!canPlace);
	}

	public void DoesProgress(bool doesProgress)
	{
		progressPause = !doesProgress;
	}

	//for Removing Mills
	public void SpawnDust()
	{
		if (badMill != null)
		{
			Instantiate (DestroyDust,badMill.transform.position + new Vector3(0,16,0),Quaternion.identity);
		}
	}
	public void RemoveMill()
	{
		if (badMill != null)
		{
			Object.Destroy (badMill);
			currentMills--;
			messedUp = false;
		}
	}

	// For Additional Feedback
	public void SpawnBirds()
	{
		TargetBad ();
		Instantiate (birds, extraTarget.position, Quaternion.identity);
	}
	public void Description (string pDescription)
	{
		helpText.text = pDescription;
	}

	//For Regular Feedback
	public void Animation()
	{
		//Will Contain calls to the rating probably?
		popup.SetBool ("play", true);
	}

	public void ActivateButton (int pnumber)
	{
		buttons [pnumber].SetBool ("Active", true);
	}

	//Cutscene backcall Reference sets

	//placing Mills
	public bool IsFinishedPlacing()
	{
		return hasPlacedMills;
	}
	public void GetMillReference(Cutscene pScript)
	{
		pScript.SetBoolReference (IsFinishedPlacing);
	}

	//Fire State
	public bool HasFoughtFire()
	{
		return fireWasFought;
	}
	public void GetFireReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasFoughtFire);
	}

	//SaboteurState
	public bool HasFoughtSaboteur()
	{
		return saboteurWasFought;
	}
	public void GetSaboteurReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasFoughtSaboteur);
	}

	//DirtyState
	public bool HasCleanedMill()
	{
		return dirtyWasfought;
	}
	public void GetCleanReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasCleanedMill);
	}

	//BrokenState
	public bool HasRepairedMill()
	{
		return brokenWasFixed;
	}
	public void GetRepairReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasCleanedMill);
	}

	//Start Events
	public void StartFire()
	{
		TurbineStateManager.lowFireState.Copy(savedMill);
		savedMill.state.OnValueChanged += OnFireEnd;
	}
	public void StartSaboteur()
	{
		TurbineStateManager.saboteurState.Copy(savedMill);
		savedMill.state.OnValueChanged += OnSaboteurEnd;
	}
	//Missing : Start Storm
	//Missing Birds

	// Called by statechanges. Change Bools of cleared goals
	public void OnSaboteurEnd(TurbineState oldState, TurbineState newState)
	{
		saboteurWasFought = newState == null;
	}
	public void OnFireEnd(TurbineState oldState, TurbineState newState)
	{
		fireWasFought = newState == null;
	}
	public void OnBrokenEnd(TurbineState oldState, TurbineState newState)
	{
		brokenWasFixed = newState == null;
	}
	public void OnDirtEnd(TurbineState oldState, TurbineState newState)
	{
		dirtyWasfought = newState == null;
	}

	public void TargetCurrentMill()
	{
		extraTarget.position = savedMill.transform.position;
	}
	public void TargetBad()
	{
		extraTarget.position = badMill.transform.position;
	}


	void OnDestroy()
	{
		instance = null;
	}
}