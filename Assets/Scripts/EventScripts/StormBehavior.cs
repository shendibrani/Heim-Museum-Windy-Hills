using UnityEngine;
using System.Collections;

public class StormBehavior : MonoBehaviour, IWindSensitive
{
    [SerializeField]
    float WindRadius = 30.0f;
    [SerializeField]
    float DamageRadius = 15.0f;

   [SerializeField]
    public float speed;

    Collider[] gottenColliders;

    [SerializeField]
    public bool debug = true;

    // Update is called once per frame
    void Update()
    {
        Move();
        if (EnteredWindzone) transform.Translate(speed, 0, speed);
        DestroyCloud();
    
    }
   
    void Move()
    {
        transform.Translate(-speed, 0, 0);
    }

    private void DestroyCloud()
    {
		if (transform.position.x < -300)
		{
			if (GetComponentInChildren<RecognizeTarget>() != null)
			{
				GetComponentInChildren<RecognizeTarget> ().transform.SetParent (null);
			}
			Destroy(gameObject);
		}
    }

    bool EnteredWindzone = false;
    public void OnEnterWindzone()
    {
        EnteredWindzone = true;

    }

    public void OnExitWindzone()
    {
        EnteredWindzone = false;
    }

    public void OnStayWindzone()
    {

    }
}

