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
	int TapTargetID = 0;

	bool dirtyWasfought = false;
	bool fireWasFought = false;
	bool saboteurWasFought = false;
	bool brokenWasFixed = false;
	bool cloudWasTapped = false;

	public bool CloudWasTapped {
		get {
			return cloudWasTapped;
		}
		set{ cloudWasTapped = value;}
	}

	bool enteredWindzone = false;

	public bool EnteredWindzone {
		get {
			return enteredWindzone;
		}
		set{ enteredWindzone= value;}
	}

	bool canStartPlacing = false;

	public bool CanStartPlacing {
		get {
			return canStartPlacing;
		}
		set{ canStartPlacing= value;
			setMillButton (value);
		}
	}



	bool messedUp = false;
	bool hasPlacedMills = false;
	bool isComplete = false;

    bool brokenTutorialCompleted = false;
	bool build1 = false;
	bool scoreWasIncreased = false;

	int requiredMills = 0;
	int currentMills = 0;

	int MillStep = 0;
	[SerializeField] Cutscene[] PlaceMills;

	Animator popup;

    bool progressPause = false;

    [SerializeField] bool debug;
	[SerializeField] Text helpText;
	[SerializeField] Image goodJob;
	[SerializeField] bool Skip;
	[SerializeField] GameObject DestroyDust;
	[SerializeField] Transform extraTarget;
	public RectTransform TapFinger;
	[SerializeField] GameObject birds;

	[SerializeField] Image MillButtonActive;
	[SerializeField] Image MillButtonInactive;

	[SerializeField] Animator[] buttons;

	[SerializeField] Cutscene HighFireTutorial;
    [SerializeField]
    Cutscene SaboteurTutorial;
    [SerializeField]
    Cutscene stormCloudTutorial;
    [SerializeField]
    Cutscene flockTutorial;
    [SerializeField]
    Cutscene boatTutorial;
    [SerializeField]
    Cutscene brokenTutorial;

	[SerializeField] Animator PlusMill;

	[SerializeField] Cutscene FirstCutscene;

    void Start()
	{
		popup = goodJob.GetComponent<Animator> ();
		progressPause = false;

		if (!Skip) {
			FirstCutscene.StartScene ();	
			PlaceObjectOnClick.Instance.SetDirty(true);
		} else {
			PlaceObjectOnClick.Instance.SetDirty (false);
		}
		FindObjectOfType<ScoreManager> ().ScoreTargetIncrease.AddListener (SetIncreasedScore);

	}

    void Update ()
	{
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
		if (PlusMill.GetBool("Active") && PlusMill.GetCurrentAnimatorStateInfo(0).IsTag("Anim") && PlusMill.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			PlusMill.SetBool ("Active", false);
		}
	}

	//For Tutorial Progression
	public void Placed()
	{
		TurbineLimitManager.Instance.ChangeAvailable (-1);
		currentMills++;
	}
	public void StepBackPlacement(GameObject turbineObject)
	{
		TurbineLimitManager.Instance.ChangeAvailable (1);
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

    public void CheckBrokenWindmill(TurbineObject to)
    {
        if (!brokenTutorialCompleted)
        {
            brokenTutorialCompleted = true;
			if (to != null)
			{
				SetMill(to);
			}
            savedMill.state.OnValueChanged += OnBrokenEnd;
            brokenTutorial.StartScene();
        }
    }

	//start windmil placing scene
	public void PlacingMills()
	{
		if (TurbineLimitManager.Instance.availableCount > 0 && PlaceMills.Length > MillStep && PlaceObjectOnClick.Instance.DirtyFlag)
		{
			if (canStartPlacing)
			{
				PlaceMills [MillStep].StartScene ();
				MillStep++;
			}
		}
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

	void setMillButton(bool pActive)
	{
		MillButtonActive.enabled = pActive;
		MillButtonInactive.enabled = !pActive;
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

	public void AddMill()
	{
		TurbineLimitManager.Instance.ChangeAvailable (1);
		PlusMill.SetBool ("Active", true);
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
		pScript.SetBoolReference (HasRepairedMill);
	}

	public bool HasFireClick()
	{
		if (UICallbacksSystem.currentState == UIState.Firemen)
		{
			return true;
		}
		return false;
	}
	public void GetFireClickReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasFireClick);
	}

	public bool HasPoliceClick()
	{
		if (UICallbacksSystem.currentState == UIState.Police)
		{
			return true;
		}
		return false;
	}
	public void GetPoliceClickReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasPoliceClick);
	}

	public bool HasRepairClick()
	{
		if (UICallbacksSystem.currentState == UIState.Repair)
		{
			return true;
		}
		return false;
	}
	public void GetRepairClickReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasRepairClick);
	}

	public bool HasBuildClick()
	{
		return build1;
	}
	public void GetBuildClickReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasBuildClick);
	}
	public void SetHasBuildBool()
	{
		build1 = true;
	}

	public bool HasClickedCloud()
	{
		return cloudWasTapped;
	}
	public void GetCloudClickReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasClickedCloud);
	}

	public bool HasIncreasedScore()
	{
		return scoreWasIncreased;
	}
	public void GetScoreIncreaseReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasIncreasedScore);
	}
	void SetIncreasedScore()
	{
		FindObjectOfType<ScoreManager> ().ScoreTargetIncrease.RemoveListener (SetIncreasedScore);
		scoreWasIncreased = true;
	}

	public bool HasWindzonedMill()
	{
		return enteredWindzone;
	}
	public void GetWindzoneReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasWindzonedMill);
	}
	public void WasWindZoned(TurbineWindZoneMessage erge)
	{
		if (erge.state == WindZoneState.Enter)
		{
			enteredWindzone = true;
			Dispatcher<TurbineWindZoneMessage>.Unsubscribe (WasWindZoned);
		}
	}
	public void EmptyWindzoned()
	{
		enteredWindzone = false;
		Dispatcher<TurbineWindZoneMessage>.Subscribe (WasWindZoned);
	}

	//Start Events
	public void StartFire(bool high)
	{
		if (!high) 
		{
			TurbineStateManager.tutorialLowFireState.Copy (savedMill);
			savedMill.state.OnValueChanged += OnFireEnd;
		}
		else
		{
			TurbineStateManager.highFireState.Copy(savedMill);
			savedMill.state.OnValueChanged += OnFireEnd;
		}
		fireWasFought = false;
	}

	public void StartSaboteur()
	{
		TurbineStateManager.saboteurState.Copy(savedMill);
		savedMill.state.OnValueChanged += OnSaboteurEnd;
	}

    public void StartFlock()
    {
        popup.SetBool("play", false);
        FlockEvent e = new FlockEvent();
        e.EventStart();
    }

    public void StartStormCloud()
    {
        popup.SetBool("play", false);
        StormCloudEvent e = new StormCloudEvent();
        e.EventStart();
    }

    public void StartBoatEvent()
    {
        popup.SetBool("play", false);
        BoatEvent e = new BoatEvent();
        e.EventStart();
    }

    // Called by statechanges. Change Bools of cleared goals
    public void OnSaboteurEnd(TurbineState oldState, TurbineState newState)
	{
		saboteurWasFought = newState == null;
	}
	public void OnFireEnd(TurbineState oldState, TurbineState newState)
	{
		fireWasFought = newState == null;
		Debug.Log (fireWasFought);
	}
	public void OnBrokenEnd(TurbineState oldState, TurbineState newState)
	{
		brokenWasFixed = newState == null;
	}
	public void OnDirtEnd(TurbineState oldState, TurbineState newState)
	{
		dirtyWasfought = newState == null;
	}

	public void SetMill(TurbineObject pMill)
	{
		savedMill = pMill;
		if(debug) Debug.Log (pMill);
	}
	public void TargetCurrentMill()
	{
		extraTarget.position = savedMill.transform.position + new Vector3 (0, 28, 0);
	}
	public void TargetBad()
	{
		extraTarget.position = badMill.transform.position;
	}

	public void TargetCloud(){
	
		extraTarget.transform.position = FindObjectOfType<StormBehavior> ().gameObject.transform.position;
		extraTarget.transform.parent = FindObjectOfType<StormBehavior> ().gameObject.transform;
	}
	public void ReleaseCloud(){

		extraTarget.transform.parent = null;
	}

	public void TargetBoat(){

		extraTarget.transform.position = FindObjectOfType<Boat> ().gameObject.transform.position;
		extraTarget.transform.parent = FindObjectOfType<Boat> ().gameObject.transform;
	}
	public void ReleaseBoat(){

		extraTarget.transform.parent = null;
	}


    public void PopulateReplayEvents()
    {
        // create a wave from already completed events to populate next wave
    }

    public void CheckEventStart(EventClass e)
    {
        switch (e.name)
        {
            case EventNames.Fire:
                {
                    SetMill(e.usedTurbine);
                    HighFireTutorial.StartScene();
                    break;
                }
            case EventNames.Saboteur:
                {
                    SetMill(e.usedTurbine);
                    SaboteurTutorial.StartScene();
                    break;
                }
            case EventNames.Boat: {
                    //SetMill(e.usedTurbine);
                    boatTutorial.StartScene();
                    break;
                }
            case EventNames.Flock:
                {
                    flockTutorial.StartScene();
                    break;
                }
            case EventNames.StormCloud:
                {
                    stormCloudTutorial.StartScene();
                    break;
                }
        }
        
    }

	void OnDestroy()
	{
		instance = null;
	}
}