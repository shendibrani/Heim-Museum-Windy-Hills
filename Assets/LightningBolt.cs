/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{ bool activated = true;
    int count = 0;
	public Transform target;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
    ParticleRenderer rend;
    int randNumber = 11;
    int randY = 11;
    int randX = 0;
    int randZ = 0;
    
    int randomColor;
   
	
	Perlin noise;
	float oneOverZigs;
	
	private Particle[] particles;
	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;
		GetComponent<ParticleEmitter>().emit = false;

		GetComponent<ParticleEmitter>().Emit(zigs);
		particles = GetComponent<ParticleEmitter>().particles;
        rend = GetComponent<ParticleRenderer>();

    }



	void Update ()
	{     
        randomColor = Random.RandomRange(0, 1);
        randY = Random.RandomRange(12,30);
        print(randY);
        randX = Random.RandomRange(-4, 4);
        randZ = Random.RandomRange(-4, 4);
        if (noise == null)
			noise = new Perlin();
			
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 5.21688f;
		float timez = Time.time * speed * 5.5564f;
		
		for (int i=0; i < particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position,  new Vector3 (transform.position.x + randX, transform.position.y - randY, transform.position.z + randZ), oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));
			
			particles[i].position = position;
           // particles[i].color = colors[1];
			particles[i].energy = 2f;
		}
		
		GetComponent<ParticleEmitter>().particles = particles;
		
		if (GetComponent<ParticleEmitter>().particleCount >= 2)
		{
			if (startLight)
				startLight.transform.position = particles[0].position;
			if (endLight)
				endLight.transform.position = particles[particles.Length - 1].position;
		}

        count += 1;
        randNumber = Random.RandomRange(11, 18);
     

        if (count < 10)
        {
            //gameObject.SetActive(true);
            rend.enabled = rend.enabled;
           
        }

        if (count > 10 && count < randNumber)
        {
           // gameObject.SetActive(false);

            rend.enabled = !rend.enabled;
        }
        if (count >= randNumber)
        {
            
            count = 0;
        }
    }	
}