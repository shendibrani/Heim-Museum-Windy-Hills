using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

	[SerializeField] Transform dest;

	void Update ()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			GetComponent<NavMeshAgent> ().SetDestination (dest.position);
		}
	}
}
