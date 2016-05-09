using UnityEngine;
using System.Collections;

public class Morale : MonoBehaviour
{
	[SerializeField] float range;

	public float morale {get; private set;}

	void Start()
	{
		FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += OnObjectPlaced;
	}

	void OnObjectPlaced(GameObject go)
	{
		morale = 1;
		foreach (TurbineObject t in FindObjectsOfType<TurbineObject>()){
			if (Vector3.Distance(transform.position, t.transform.position) < range){
				morale -= 0.2f;
			}
		}

		morale = (Mathf.Max(morale, 0));
	}
}

