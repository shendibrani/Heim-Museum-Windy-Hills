using UnityEngine;
using System.Collections;

public class TurbineParticle : MonoBehaviour {

	[SerializeField] GameObject windParticle;

    [SerializeField] GameObject breakingPoof, brokenSmoke;
 	
	[SerializeField] GameObject lowFire, highFire, fireSmoke;

	[SerializeField] GameObject dirtParticles;

    float timer;

    bool smoke;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (smoke) timer += Time.deltaTime;
        if (timer >= 0.7f) {
            brokenSmoke.SetActive(true);
            smoke = false;
            timer = 0;
        } 
	}

	public void Break(bool broken)
    {
		if (broken) {
			breakingPoof.SetActive (true);
			windParticle.SetActive (false);
			smoke = true;
		} else {
			breakingPoof.SetActive (false);
			//brokenSmoke.GetComponent<ParticleSystem> ().Stop ();
			brokenSmoke.SetActive (false);
			windParticle.SetActive (true);
		}
    }

	public void LowFire(bool fire)
	{
		if (fire) {
			lowFire.SetActive (true);
			fireSmoke.SetActive (true);
			windParticle.SetActive (false);
		} else { 
			lowFire.SetActive (false);
			fireSmoke.SetActive (false);
			windParticle.SetActive (true);
		}
	}

	public void HighFire(bool fire)
	{
		if (fire) {
			highFire.SetActive (true);
			fireSmoke.SetActive (true);
			windParticle.SetActive (false);
		} else { 
			highFire.SetActive (false);
			fireSmoke.SetActive (false);
			windParticle.SetActive (true);
		}
	}

	public void Dirty(bool dirty)
	{
		if (dirty) {
			dirtParticles.SetActive (true);
			windParticle.SetActive (false);
		} else {
			dirtParticles.SetActive (false);
			windParticle.SetActive (true);
		}
	}
}
