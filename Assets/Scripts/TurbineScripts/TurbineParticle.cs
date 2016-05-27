using UnityEngine;
using System.Collections;

public class TurbineParticle : MonoBehaviour {

    [SerializeField]
    GameObject breakingPoof;
    [SerializeField]
    GameObject brokenSmoke;
    [SerializeField]
    GameObject windParticle;

    float timer;

    bool activate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (activate) timer += Time.deltaTime;
        if (timer >= 0.7f) {
            brokenSmoke.SetActive(true);
            activate = false;
            timer = 0;
        } 
	}

    public void ActivateBreaking()
    {
        breakingPoof.SetActive(true);
        activate = true;
    }

    public void DeactivateBreaking()
    {
        breakingPoof.SetActive(false);
        brokenSmoke.GetComponent<ParticleSystem>().Stop();
        brokenSmoke.SetActive(false);
    }
}
