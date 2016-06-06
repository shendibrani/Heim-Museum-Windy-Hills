using UnityEngine;
using System.Collections;

public class Morale : MonoBehaviour
{
	[SerializeField] float range;

    [SerializeField]
    bool debug;

    [SerializeField]
    bool doesRemove;

	public Monitored<float> morale {get; private set;}

    public delegate void MoraleUpdate(float value);
    public MoraleUpdate onMoraleUpdate;

    void Start()
	{
        morale = new Monitored<float>(1f);
		FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += OnObjectPlaced;
        onMoraleUpdate = ScoreManager.Instance.MoraleUpdate;
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
        if (go)
        {
            //morale.value = 1;
            //foreach (TurbineObject t in FindObjectsOfType<TurbineObject>())
            //{
            if (go.GetComponentInChildren<TurbineObject>() != null)
            {
                if (Vector3.Distance(transform.position, go.transform.position) < range)
                {
                    morale.value -= 0.05f;
                    onMoraleUpdate(-0.05f);
					CheckWindmillPlacement (go);
                    if (debug) Debug.Log("Update Morale");
                }
                else
                {
                    morale.value += 0.05f;
                    onMoraleUpdate(0.05f);
                    if (debug) Debug.Log("Update Morale");
                }
            }

            morale.value = (Mathf.Max(morale, 0));
        }
    }

    void CheckWindmillPlacement(GameObject go)
    {
       if (go != null && doesRemove) {
            //Object.Destroy(go);
            TutorialProgression.Instance.StepBackPlacement(go);
            //PlaceObjectOnClick.Instance.SetDirty(false);
        }
    }
}

