using UnityEngine;
using System.Collections;

public class Morale : MonoBehaviour
{
	[SerializeField] float range;

    [SerializeField]
    bool debug;

	public Monitored<float> morale {get; private set;}

    public delegate void MoraleUpdate(float value);
    public MoraleUpdate onMoraleUpdate;

    void Start()
	{
        //morale.value = 1f;
		FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += OnObjectPlaced;
        onMoraleUpdate = FindObjectOfType<MoraleHUDManager>().MoraleUpdate;
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
        //morale.value = 1;
        //foreach (TurbineObject t in FindObjectsOfType<TurbineObject>())
        //{
        if (debug) Debug.Log("Object Placed");
        if (go.GetComponent<TurbineObject>() != null) {
            if (Vector3.Distance(transform.position, go.transform.position) < range)
            {
                morale.value -= 0.2f;
                onMoraleUpdate(-0.2f);
                if (debug) Debug.Log("Update Morale");
            }
        }

		morale.value = (Mathf.Max(morale, 0));
	}
}

