using UnityEngine;
using System.Collections;

public class Morale : MonoBehaviour
{
	[SerializeField] float range;

	public Monitored<float> morale {get; private set;}

	void Start()
	{
		FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += OnObjectPlaced;
	}

	void OnObjectPlaced(GameObject go)
	{
		morale.value = 1;
		foreach (TurbineObject t in FindObjectsOfType<TurbineObject>()){
			if (Vector3.Distance(transform.position, t.transform.position) < range){
				morale.value -= 0.2f;
			}
		}

		morale.value = (Mathf.Max(morale, 0));
	}
}

