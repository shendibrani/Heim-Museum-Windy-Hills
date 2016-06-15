using UnityEngine;
using System.Collections;

public class Sail : MonoBehaviour {

	public bool hasHit = false;
	Vector3[] rotationAx;
	Transform[] sails;
	Vector3 start;

	float scale = 1;

	public TurbineObject goal;

	void Start ()
	{
		start = this.transform.position;
		rotationAx = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			rotationAx[i] = new Vector3(Random.Range(3,12),Random.Range(1,3),Random.Range(1,3));
		}

		sails = GetComponentsInChildren<Transform> ();
	}

	void Update ()
	{
		if (!hasHit && goal != null)
		{
			if (Vector3.Distance(transform.position,goal.transform.position) < 8f)
			{
				goal.BreakTurbine ();
				hasHit = true;
			}
		}

		if (hasHit)
		{
			if (scale < 0.01f)
			{
				Destroy (this.gameObject);
			}
			else
			{
				foreach (var item in sails)
				{
					item.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);

				}
				scale -= 0.01f;
			}
		}
		else if(Vector3.Distance(start,transform.position) > 250)
		{
			Destroy (this.gameObject);
		}
		for (int i = 0; i < 3; i++)
		{
			if (i != 0)
			{
				sails[i].transform.Rotate (rotationAx[i]);
			}
		}
		transform.position += transform.forward;
	}
}
