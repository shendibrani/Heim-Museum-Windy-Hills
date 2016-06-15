using UnityEngine;
using System.Collections;

public class Sail : MonoBehaviour {

	public bool hasHit = false;
	bool free = false;
	Vector3 rotationAx;

	public TurbineObject goal;

	[SerializeField] bool boss = false;

	[SerializeField] Sail otherSail1;
	[SerializeField] Sail otherSail2;
	// Use this for initialization
	void Start () {
		rotationAx = new Vector3(Random.Range(3,12),Random.Range(1,3),Random.Range(1,3));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (free)
		{
			if (boss && !hasHit && goal != null)
			{
				if (Vector3.Distance(transform.position,goal.transform.position) < 4f)
				{
					goal.BreakTurbine ();
					otherSail1.hasHit = true;
					otherSail2.hasHit = true;
					hasHit = true;
				}
			}

			if (hasHit)
			{
				if (transform.localScale.x < 0.01f)
				{
					Destroy (this.gameObject);
				} else {
					transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
				}
			}

			transform.localPosition += new Vector3 (0, 0, 1);
			transform.Rotate (rotationAx);
		}
	}

	public void SetFree()
	{
		free = true;
	}
}
