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

	GameObject badMill;
	TurbineObject firstMill;
	TurbineObject randomMill;

	bool fireWasFought = false;
	bool saboteurWasFought = false;
	bool messedUp = false;
	bool hasPlacedMills = false;
	bool isComplete = false;

	int requiredMills = 0;
	int currentMills = 0;

	Animator popup;

    [SerializeField] bool debug;
	[SerializeField] Cutscene scene;
	[SerializeField] Text helpText;
	[SerializeField] Image goodJob;
	[SerializeField] bool Skip;
	[SerializeField] GameObject DestroyDust;
	[SerializeField] Transform extraTarget;

    void Start()
	{
		popup = goodJob.GetComponent<Animator> ();

		PlaceObjectOnClick.Instance.SetDirty(true);

		scene.StartScene ();
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
			firstMill.state.OnValueChanged -= OnFireEnd;
		}
		if (saboteurWasFought)
		{
			randomMill.state.OnValueChanged -= OnSaboteur;
		}
	}

	public void OnSaboteur(TurbineState oldState, TurbineState newState){
		saboteurWasFought = newState == null;
	}
		
	public void OnFireEnd(TurbineState oldState, TurbineState newState)
	{
		fireWasFought = newState == null;
	}

	public void PlaceMillMission(int pReq)
	{
		PlaceObjectOnClick.Instance.SetDirty(false);
		popup.SetBool("play", false);
		requiredMills = pReq;
		currentMills = 0;
		hasPlacedMills = false;
	}

	public void EndMission()
	{
		PlaceObjectOnClick.Instance.SetDirty(true);
	}

	public void pauseMission()
	{
		PlaceObjectOnClick.Instance.SetDirty(true);
	}

	public void ResumeMission()
	{
		PlaceObjectOnClick.Instance.SetDirty (false);
	}

	public void Description (string pDescription)
	{
		helpText.text = pDescription;
	}

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

	public void Animation()
	{
		popup.SetBool ("play", true);
	}

    void OnDestroy()
    {
        instance = null;
    }

    public void StepBackPlacement(GameObject turbineObject)
    {
        badMill = turbineObject;
        messedUp = true;
    }

	public void Placed(TurbineObject pMill)
	{
		if (firstMill == null)
		{
			firstMill = pMill;
		}
		currentMills++;
	}

	public bool IsFinishedPlacing()
	{
		return hasPlacedMills;
	}
	public void GetMillReference(Cutscene pScript)
	{
		pScript.SetBoolReference (IsFinishedPlacing);
	}


	public bool HasFoughtFire()
	{
		return fireWasFought;
	}
	public void GetFireReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasFoughtFire);
	}


	public bool HasFoughtSaboteur()
	{
		return saboteurWasFought;
	}
	public void GetSaboteurReference(Cutscene pScript)
	{
		pScript.SetBoolReference (HasFoughtSaboteur);
	}

	public void StartFire()
	{
		TurbineStateManager.lowFireState.Copy(firstMill);
		firstMill.state.OnValueChanged += OnFireEnd;
	}

	public void StartSaboteur()
	{
		TurbineStateManager.saboteurState.Copy(randomMill);
		randomMill.state.OnValueChanged += OnSaboteur;
	}

	public void TargetFirst()
	{
		extraTarget.position = firstMill.transform.position;
	}
	public void TargetSpecial()
	{
		TurbineObject[] turbs = FindObjectsOfType<TurbineObject>();
		randomMill = turbs[Random.Range(0, turbs.Length)];
		extraTarget.position = randomMill.transform.position;
	}
}