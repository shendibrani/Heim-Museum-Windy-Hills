using UnityEngine;
using System.Collections;

public class TurbineParticle : MonoBehaviour {

	[SerializeField] GameObject windParticle;

    [SerializeField] GameObject breakingPoof, brokenSmoke;
 	
	[SerializeField] GameObject lowFire, highFire, fireSmoke;

	[SerializeField] GameObject dirtParticles;

	[SerializeField] GameObject SparkleParticles1;
	[SerializeField] GameObject SparkleParticles2;
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
			//windParticle.SetActive (false);
		} else { 
			lowFire.SetActive (false);
			fireSmoke.SetActive (false);
			//windParticle.SetActive (true);
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

	public void Sparkles(bool pSpark)
	{
		if (pSpark)
		{
			SparkleParticles1.GetComponent<ParticleSystem> ().Play ();
			SparkleParticles2.GetComponent<ParticleSystem> ().Play ();
		}
	}

	public void OnStateChange(TurbineState oldState, TurbineState newState)
	{
		if (oldState != null && oldState.name != TurbineStateManager.turbineCooldownState.name) {
			if (oldState.name == TurbineStateManager.brokenState.name) {
				Break (false);
			} else if (oldState.name == TurbineStateManager.lowFireState.name) {
				LowFire (false);
			} else if (oldState.name == TurbineStateManager.highFireState.name) {
				HighFire (false);
			} else if (oldState.name == TurbineStateManager.dirtyState.name) {
				Dirty (false);
			} else if (oldState.name == TurbineStateManager.occupiedState.name)
            {
                GetComponentInChildren<Saboteur>().EndAnimation();
            }
		}

		if (newState != null && newState.name != TurbineStateManager.turbineCooldownState.name)
		{
			if (newState.name == TurbineStateManager.saboteurState.name) {
				GetComponentInChildren<Saboteur> ().StartAnimation ();
			} else if (newState.name == TurbineStateManager.brokenState.name) {
				Break (true);
			} else if (newState.name == TurbineStateManager.lowFireState.name) {
				LowFire (true);
			} else if (newState.name == TurbineStateManager.highFireState.name) {
				HighFire (true);
			} else if (newState.name == TurbineStateManager.dirtyState.name) {
				Dirty (true);
			}
		}
		if(newState.name == TurbineStateManager.turbineCooldownState.name && oldState.name != TurbineStateManager.turbineCooldownState.name)
		{
			Sparkles (true);
            if (oldState.name == TurbineStateManager.saboteurState.name)
            {
                GetComponentInChildren<Saboteur>().EndAnimation();
            }
        }
	}
}
