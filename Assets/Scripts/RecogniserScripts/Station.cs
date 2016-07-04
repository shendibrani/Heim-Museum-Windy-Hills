using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Station : MonoBehaviour 
{
	[SerializeField] protected GameObject prefab;
	protected List<Agent> instances;

	protected virtual void Start()
	{
		instances = new List<Agent> ();
	}

	protected Agent GetFreeAgent()
	{
		Agent free = null;

		for (int i = 0; i < instances.Count; i++) {
			if (!instances [i].busy){
				free = instances [i];
				break;
			}
		}

		if (free == null) {
			free = (GameObject.Instantiate (prefab, transform.position, Quaternion.identity) as GameObject).GetComponent<Agent> ();
			free.station = this.gameObject;
		}

		return free;
	}
}

