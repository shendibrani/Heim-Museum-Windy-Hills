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

    // Use this for initialization
    void Start() {
        cameraMover = GetComponent<CameraMovement>();
	}
	
    public void SetComplete()
    {
        isComplete = true;
    }

	// Update is called once per frame
	void Update () {
        if (Skip)
        {
            PlaceObjectOnClick.Instance.SetDirty(false);
            SetComplete();
        }
        else {
            if (isTiming == true)
            {
                timer += Time.deltaTime;
                if (timer >= timerTarget)
                {
                    OnStepTutorial();
                    isTiming = false;
                }
            }
        }
	}

    void OnDestroy()
    {
        instance = null;
    }

    public void OnStepTutorial()
    {
        cameraMover.SetProgress();
    }

    public void BeginStepTimer(float t = 0)
    {
        isTiming = true;
        timerTarget = t;
        timer = 0;
    } 

}
