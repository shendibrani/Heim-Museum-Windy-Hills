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

    CameraMovement cameraMover;

    [SerializeField]
    bool Skip;

    float timer = 0;
    float timerTarget = 0;
    bool isTiming = false;
    bool isComplete = false;

	bool fuckedUp = false;
	GameObject badMill;
	TurbineObject firstMill;
	TurbineObject randomMill;

	bool missionIsSetUp = false;
	bool cameraStopped = true;
	bool missionEnded = false;

	bool fireWasFought = false;
	bool saboteurWasFought = false;

	int tutorialstep = 0;
	int next = 0;
	int requiredMills = 0;
	int currentMills = 0;

	[SerializeField] GameObject startMessage;
	[SerializeField] Text helpText;
	[SerializeField] Image goodJob;
	[SerializeField] Cow cow1;
	[SerializeField] Cow cow2;
	[SerializeField] Cow cow3;
	Animator popup;

	[SerializeField] Farmer farmer_1;
    [SerializeField] bool debug;

	delegate void TimerEvent();
	TimerEvent onTimerEvent;

    // Use this for initialization
    void Start()
	{
        cameraMover = GetComponent<CameraMovement>();
		onTimerEvent = OnStepTutorial;

		popup = goodJob.GetComponent<Animator> ();

		PlaceObjectOnClick.Instance.SetDirty(true);
	}
	
    public void SetComplete()
    {
        isComplete = true;
    }

    void TutorialProgession0()
    {
        startMessage.SetActive (true);

				//enable clickbait

				//progression requirement
				if (next == 0)

                {
            startMessage.SetActive(false);
            OnStepTutorial();
            next = 1;
        }
    }
	
    void TutorialProgession1()
    {
        if (!missionIsSetUp && cameraStopped && next == 1)
        {
            farmer_1.Wave(true);
            NewMission(1);
            helpText.text = "Bouw een turbine";
            next = 2;

        }
        //enable clickbait
        if (missionIsSetUp)
        {
            if (fuckedUp && !isTiming)
            {
                //farmer_1.Walk (badMill.transform.position);
                onTimerEvent = RemoveMill;
                BeginTimer(1);
            }

            //progression requirement
            if (currentMills == requiredMills && !fuckedUp && !missionEnded)
            {
                if (firstMill == null)
                {
                    firstMill = FindObjectOfType<TurbineObject>();
                }
                popup.SetBool("play", true);
                EndMission(3);
            }
        }
    }

    void TutorialProgression2()
    {
        if (!missionIsSetUp && cameraStopped && next == 2)
        {
            popup.SetBool("play", false);
            NewMission(2);
            helpText.text = "Bouw twee turbines";
            next = 3;

            missionIsSetUp = true;
        }
        if (missionIsSetUp)
        {
            if (fuckedUp && !isTiming)
            {
                cow1.Run();
                cow2.Run();
                cow3.Run();
                onTimerEvent = RemoveMill;
                BeginTimer(1);
            }

            if (currentMills == requiredMills && !fuckedUp && !missionEnded)
            {
                popup.SetBool("play", true);
                EndMission(3);
            }
        }
    }
    
    void TutorialProgression3()
    {
        if (!missionIsSetUp && cameraStopped && next == 3)
        {
            popup.SetBool("play", false);
            PlaceObjectOnClick.Instance.SetDirty(true);
            currentMills = 0;
            requiredMills = 0;
            TurbineStateManager.lowFireState.Copy(firstMill);
            firstMill.state.OnValueChanged += OnFireEnd;
            helpText.text = "Bluss de brand op de turbine";
            next = 4;
            missionEnded = false;
            missionIsSetUp = true;
        }
        if (missionIsSetUp)
        {
            if (fireWasFought)
            {
                popup.SetBool("play", true);
                EndMission(3);
                firstMill.state.OnValueChanged -= OnFireEnd;
            }
        }
    }

    void TutorialProgression4()
    {
        if (!missionIsSetUp && cameraStopped && next == 4)
        {
            popup.SetBool("play", false);
            NewMission(3);
            helpText.text = "Bouw driw turbines";
            next = 5;
            missionIsSetUp = true;
        }
        if (missionIsSetUp)
        {
            if (fuckedUp && !isTiming)
            {
                //start animation
                onTimerEvent = RemoveMill;
                BeginTimer(1);
            }
            if (currentMills == requiredMills && !fuckedUp && !missionEnded)
            {
                FindObjectOfType<Fossil_Fuel_Particle>().lessFossil = true;
                popup.SetBool("play", true);
                EndMission(3);
            }
        }
    }

    void TutorialProgression5()
    {
        if (!missionIsSetUp && cameraStopped && next == 5)
        {
            popup.SetBool("play", false);
            PlaceObjectOnClick.Instance.SetDirty(true);
            currentMills = 0;
            requiredMills = 0;
            next = 6;
            helpText.text = "Verdedig de windmolen";
            missionEnded = false;
            missionIsSetUp = true;
        }
        if (missionIsSetUp && !missionEnded)
        {
            TurbineObject[] turbs = FindObjectsOfType<TurbineObject>();
            randomMill = turbs[Random.Range(0, turbs.Length)];
            cameraMover.NewWaypoint(6, randomMill.transform, 60, false);

            EndMission();
        }
    }

    void TutorialProgression6()
    {
        if (!missionIsSetUp && cameraStopped && next == 6)
        {
            popup.SetBool("play", false);
            PlaceObjectOnClick.Instance.SetDirty(true);
            currentMills = 0;
            requiredMills = 0;
            TurbineStateManager.saboteurState.Copy(randomMill);
            randomMill.state.OnValueChanged += OnSaboteur;
            next = 7;
            missionEnded = false;
            missionIsSetUp = true;
        }
        if (missionIsSetUp)
        {
            if (saboteurWasFought && !missionEnded)
            {
                popup.SetBool("play", true);
                EndMission(3);
                randomMill.state.OnValueChanged -= OnSaboteur;
            }
        }
    }

    void TutorialProgression7()
    {
        if (cameraStopped)
        {
            popup.SetBool("play", false);
            SetComplete();
        }
    }


    // Update is called once per frame
    void Update ()
	{
		if (Skip)
		{
			PlaceObjectOnClick.Instance.SetDirty (false);
			SetComplete ();
		}
		else
		{
			if (isTiming == true)
			{
				timer += Time.deltaTime;
				if (timer >= timerTarget)
				{
					onTimerEvent ();
					isTiming = false;
				}
			}
        
			//popup.SetBool("play",false);

			//a story explanation or event shows, the whole map is in the background. Game tells you to click if wait too long
			if (tutorialstep == 0)
			{
                TutorialProgession0();	
			}
		//camera is on the first farm, you are eventually told to place a windmill if waited too long
			else if (tutorialstep == 1)
			{
                TutorialProgession1();
			}
			else if (tutorialstep == 2)
			{
                TutorialProgression2();
			}
			else if (tutorialstep == 3)
			{
                TutorialProgression3();
			}
			else if (tutorialstep == 4)
			{
                TutorialProgression4();
			}
			else if (tutorialstep == 5)
			{
                TutorialProgression5();
			}
			else if (tutorialstep == 6)
			{
                TutorialProgression6();
			}
			else if (tutorialstep == 7)
			{
                TutorialProgression7();
			}
		}
	}

	public void OnSaboteur(TurbineState oldState, TurbineState newState){
		saboteurWasFought = newState == null;
	}
		
	public void OnFireEnd(TurbineState oldState, TurbineState newState)
	{
		fireWasFought = newState == null;
	}

	void NewMission(int pReq)
	{
		PlaceObjectOnClick.Instance.SetDirty(false);
		requiredMills = pReq;
		currentMills = 0;

		missionEnded = false;
		missionIsSetUp = true;
	}

	void EndMission(int pTimer = 0)
	{
		PlaceObjectOnClick.Instance.SetDirty(true);
		onTimerEvent = OnStepTutorial;
		BeginTimer (pTimer);

		helpText.text = "";
		missionEnded = true;
		missionIsSetUp = false;
	}

	void RemoveMill()
	{
		if (badMill != null)
		{
			Object.Destroy (badMill);
			currentMills--;
			fuckedUp = false;
			PlaceObjectOnClick.Instance.SetDirty(false);
		}
	}

    void OnDestroy()
    {
        instance = null;
    }

    public void StepBackPlacement(GameObject turbineObject)
    {
        badMill = turbineObject;
        fuckedUp = true;
        PlaceObjectOnClick.Instance.SetDirty(true);
    }

    public void OnStepTutorial()
    {
		cameraMover.SetProgress();
		tutorialstep++;
    }

    void BeginTimer(float t = 0)
    {
        isTiming = true;
        timerTarget = t;
        timer = 0;
    } 

	public void Placed()
	{
		currentMills++;
		Debug.Log (currentMills);
		Debug.Log (requiredMills);
	}

	public void setCamera(bool pCam)
	{
		cameraStopped = pCam;
	}
}