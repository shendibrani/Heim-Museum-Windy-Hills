using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class Morale : MonoBehaviour
{
    static List<Morale> _all;

    public static List<Morale> all
    {
        get
        {
            if (_all == null)
            {
                _all = new List<Morale>(FindObjectsOfType<Morale>());
            }
            return _all;
        }
    }

    [SerializeField]
    bool usesCollider;
	[SerializeField] float range;

    [SerializeField]
    bool debug;

    [SerializeField]
    bool doesRemove;

	[SerializeField] UnityEvent moraleEvent;

	public Monitored<float> morale {get; private set;}

    /*public delegate void MoraleUpdate(float value);
    public MoraleUpdate onMoraleUpdate;*/

    void Start()
	{
        morale = new Monitored<float>(1f);
		FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += OnObjectPlaced;
        //onMoraleUpdate = ScoreManager.Instance.MoraleUpdate;
	}

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }

    void OnObjectPlaced(GameObject go)
    {
        if (go && !usesCollider)
        {
            if (go.GetComponentInChildren<TurbineObject>() != null)
            {
                if (Vector3.Distance(transform.position, go.transform.position) < range)
                {
                    morale.value -= 0.1f;
                    ScoreManager.Instance.MoraleUpdate(-0.1f);
                    //onMoraleUpdate(-0.1f);
                    CheckWindmillPlacement(go);
                    if (debug) Debug.Log("Update Morale");
                }
                else
                {
                    morale.value += 0.1f;
                    ScoreManager.Instance.MoraleUpdate(0.1f);
                    //onMoraleUpdate(0.1f);
                    if (debug) Debug.Log("Update Morale");
                }
            }
        }
        if (go && usesCollider)
        {
            if (GetComponent<Collider>().bounds.Contains(go.transform.position)) {
                morale.value -= 0.1f;
                ScoreManager.Instance.MoraleUpdate(-0.1f);
                //onMoraleUpdate(-1f);
                CheckWindmillPlacement(go);
                if (debug) Debug.Log("Update Morale");
            }
            else
            {
                morale.value += 0.1f;
                ScoreManager.Instance.MoraleUpdate(0.1f);
                //onMoraleUpdate(0.05f);
                if (debug) Debug.Log("Update Morale");
            }
        }
        morale.value = (Mathf.Max(morale, 0));
    }

    void CheckWindmillPlacement(GameObject go)
    {
       if (go != null  && doesRemove)
		{
            TutorialProgression.Instance.StepBackPlacement(go);
        }
		if (go != null && !doesRemove) {
			TutorialProgression.Instance.SaveBadMill (go);
		}
		moraleEvent.Invoke();
    }
}

