using UnityEngine;
using System.Collections;

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
	int requiredMills = 0;
	int currentMills = 0;

	[SerializeField] GameObject startMessage;


	delegate void TimerEvent();
	TimerEvent onTimerEvent;

    // Use this for initialization
    void Start()
	{
        cameraMover = GetComponent<CameraMovement>();
		onTimerEvent = OnStepTutorial;

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
				if (!missionIsSetUp && cameraStopped)
				{
					NewMission (1);
					missionIsSetUp = true;
				}
				//enable clickbait
				if (missionIsSetUp)
				{
					if (fuckedUp && !isTiming)
					{
						//start animation
						onTimerEvent = RemoveMill;
						BeginTimer (1);
					}

					//progression requirement
					if (currentMills == requiredMills && !fuckedUp)
					{
						if (firstMill == null)
					{
						firstMill = FindObjectOfType<TurbineObject> ();
					}
						EndMission (0);
					}
				}
			}
			else if (tutorialstep == 2)
			{
				if (!missionIsSetUp && cameraStopped)
				{
					NewMission (2);
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
						EndMission (0);
					}
				}
			}
			else if (tutorialstep == 3)
			{
				if (!missionIsSetUp && cameraStopped)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					TurbineStateManager.lowFireState.Copy (firstMill);
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
						EndMission (0);
					}
				}
			}
			else if (tutorialstep == 4)
			{
				if (!missionIsSetUp && cameraStopped)
				{
					NewMission (3);
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
						EndMission (0);
					}
				}
			}
			else if (tutorialstep == 5)
			{
				if (!missionIsSetUp && cameraStopped)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					missionIsSetUp = true;
				}
				if (missionIsSetUp)
				{
					TurbineObject[] turbs = FindObjectsOfType<TurbineObject> ();
					randomMill = turbs[Random.Range(0,turbs.Length)];
					cameraMover.NewWaypoint (6, randomMill.transform, 60, false);
					EndMission ();
				}
			}
			else if (tutorialstep == 6)
			{
				if (!missionIsSetUp && cameraStopped)
				{
					PlaceObjectOnClick.Instance.SetDirty(true);
					currentMills = 0;
					requiredMills = 0;
					TurbineStateManager.saboteurState.Copy (randomMill);
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
						EndMission (0);
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
	}

	void EndMission(int pTimer = 0)
	{
		PlaceObjectOnClick.Instance.SetDirty(true);
		onTimerEvent = OnStepTutorial;
		BeginTimer (pTimer);
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
	/*void OnGUI()
	{

		GUI.Label(new Rect(Screen.width - 250, Screen.height - 700, 250, 20), "Quest 1: Bouw een turbine " + q1 +"/1");
		if(q1 == 1)
			GUI.Label(new Rect(Screen.width - 250, Screen.height - 680, 250, 20), "Quest 2: Bouw twee turbines " + q2 +"/2");
		if (q2 ==2)
			GUI.Label(new Rect(Screen.width - 250, Screen.height - 660, 250, 20), "Quest 3: Bluss de brand op de turbine " + q3 +"/1");
		if (q3 == 1)
			GUI.Label(new Rect(Screen.width - 250, Screen.height - 640, 250, 20), "Quest 4: Bouw driw turbines " + q4 + "/3");
		if (q4 == 3)
			GUI.Label(new Rect(Screen.width - 250, Screen.height - 620, 250, 20), "Quest 5: Blaas de aanvallers weg " + q5 + "/1");
	}*/
}