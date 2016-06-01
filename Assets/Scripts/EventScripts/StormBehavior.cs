using UnityEngine;
using System.Collections;

public class StormBehavior : MonoBehaviour, IWindSensitive
{

    float WindRadius = 30.0f;
    float DamageRadius = 15.0f;

    public float speed;
    Collider[] gottenColliders;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<TurbineObject>())
        {
            col.GetComponent<TurbineObject>().BreakTurbine();
        }
    }

    private void DestroyCloud()
    {
        if (transform.position.x < -300) Destroy(gameObject);
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

