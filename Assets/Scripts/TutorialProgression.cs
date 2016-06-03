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

	bool fireWasFought = false;
	bool saboteurWasFought = false;

	int tutorialstep = 0;
	int next = 1;
	int requiredMills = 0;
	int currentMills = 0;

	[SerializeField] GameObject startMessage;
	[SerializeField] Text helpText;
	[SerializeField] Image goodJob;
	Animator popup;

	[SerializeField] Farmer farmer_1;

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
        
			popup.SetBool("play",false);

			//a story explanation or event shows, the whole map is in the background. Game tells you to click if wait too long
			if (tutorialstep == 0)
			{
				startMessage.SetActive (true);

				//enable clickbait

				//progression requirement
				if (Input.GetMouseButtonDown (0))
				{
					startMessage.SetActive (false);
					OnStepTutorial ();
				}
			}
		//camera is on the first farm, you are eventually told to place a windmill if waited too long
			else if (tutorialstep == 1)
			{
				if (!missionIsSetUp && cameraStopped && next == 1)
				{
					farmer_1.Wave (true);
					NewMission (1);
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
						BeginTimer (2);
					}

					//progression requirement
					if (currentMills == requiredMills && !fuckedUp)
					{
						if (firstMill == null)
						{
							firstMill = FindObjectOfType<TurbineObject> ();
						}
						popup.SetBool("play",true);
						EndMission (3);
					}
				}
			}
			else if (tutorialstep == 2)
			{
				if (!missionIsSetUp && cameraStopped && next == 2)
				{
					NewMission (2);
					helpText.text = "Bouw twee turbines";
					next = 3;

					missionIsSetUp = true;
				}
				if (missionIsSetUp)
				{
					if (fuckedUp && !isTiming)
					{
						//start animation
						onTimerEvent = RemoveMill;
						BeginTimer (1);
					}

					if (currentMills == requiredMills && !fuckedUp)
					{
						popup.SetBool("play",true);
						EndMission (3);
					}
				}
			}
			else if (tutorialstep == 3)
			{
				if (!missionIsSetUp && cameraStopped && next == 3)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					TurbineStateManager.lowFireState.Copy (firstMill);
					helpText.text = "Bluss de brand op de turbine";
					next = 4;
					missionIsSetUp = true;
				}
				if (missionIsSetUp)
				{
					if (Input.GetKey(KeyCode.Space))
					{
						fireWasFought = true;
					}
					if (fireWasFought)
					{
						popup.SetBool("play",true);
						EndMission (3);
					}
				}
			}
			else if (tutorialstep == 4)
			{
				if (!missionIsSetUp && cameraStopped && next == 4)
				{
					NewMission (3);
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
						BeginTimer (1);
					}
					if (currentMills == requiredMills && !fuckedUp)
					{
						FindObjectOfType<Fossil_Fuel_Particle> ().lessFossil = true;
						popup.SetBool("play",true);
						EndMission (3);
					}
				}
			}
			else if (tutorialstep == 5)
			{
				if (!missionIsSetUp && cameraStopped && next == 5)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					next = 5;
					missionIsSetUp = true;
				}
				if (missionIsSetUp)
				{
					popup.SetBool("play",true);

					TurbineObject[] turbs = FindObjectsOfType<TurbineObject> ();
					randomMill = turbs[Random.Range(0,turbs.Length)];
					cameraMover.NewWaypoint (6, randomMill.transform, 60, false);

					EndMission ();
				}
			}
			else if (tutorialstep == 6)
			{
				if (!missionIsSetUp && cameraStopped && next == 6)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					TurbineStateManager.saboteurState.Copy (randomMill);
					Debug.Log ("saboteur seeded");
					next = 7;
					missionIsSetUp = true;
				}
				if (missionIsSetUp)
				{
					if (Input.GetKey(KeyCode.Space))
					{
						saboteurWasFought = true;
					}
					if (saboteurWasFought)
					{
						popup.SetBool("play",true);
						EndMission (3);
					}
				}
			}
			else if (tutorialstep == 7)
			{
				if (cameraStopped)
				{
					SetComplete ();
				}
			}
		}
	}

	void NewMission(int pReq)
	{
		PlaceObjectOnClick.Instance.SetDirty(false);
		requiredMills = pReq;
		currentMills = 0;
		missionIsSetUp = true;
	}

	void EndMission(int pTimer = 0)
	{
		PlaceObjectOnClick.Instance.SetDirty(true);
		onTimerEvent = OnStepTutorial;
		BeginTimer (pTimer);

		helpText.text = "";
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

	public void Mess(GameObject pMill)
	{
		fuckedUp = true;
		badMill = pMill;
		Debug.Log ("fucked Up");
		PlaceObjectOnClick.Instance.SetDirty(true);
	}

	public void setCamera(bool pCam)
	{
		cameraStopped = pCam;
	}
}