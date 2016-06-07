using UnityEngine;
using System.Collections;

public class PaticleDestroyer : MonoBehaviour {

	void Update ()
	{
		if (!GetComponent<ParticleSystem>().isPlaying)
		{
			Destroy (this.gameObject);
		}
	}
}
