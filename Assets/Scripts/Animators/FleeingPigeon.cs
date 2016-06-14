using UnityEngine;
using System.Collections;

public class FleeingPigeon : MonoBehaviour {

	[SerializeField] Transform[] Pigeons;
	[SerializeField] float EndDistance;
	float speed;

	// Use this for initialization
	void Start () {
		foreach (var item in Pigeons)
		{
			item.Rotate (Random.Range (-30, -50), Random.Range (0, 361), 0);
		}
		speed = Random.Range (0.05f, 0.15f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool empty = true;
		foreach (var item in Pigeons)
		{
			if (item != null)
			{
				empty = false;
				item.localPosition += item.forward*speed;
				if (item.localPosition.magnitude > EndDistance)
				{
					Destroy (item.gameObject);
				}
				if (item.localPosition.y > 5)
				{
					item.rotation =  Quaternion.Lerp(item.rotation,Quaternion.Euler(0,item.eulerAngles.y,0),0.1f);

				}
			}
		}
		if (empty)
		{
			Destroy (this.gameObject);
		}
	}
}
