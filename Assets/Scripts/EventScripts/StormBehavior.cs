using UnityEngine;
using System.Collections;

public class StormBehavior : StormCloudEvent, IWindSensitive
{

    float WindRadius = 60.0f;
    float DamageRadius = 40.0f;

    bool debug = true;
    // Update is called once per frame
    void Update()
    {

        Move();
        if (EnteredWindzone) transform.Translate(5, 0, 5);
        WindEffect();
        DestroyCloud();
    }

    void WindEffect() {
        foreach (TurbineObject to in TurbineObject.all) {
            float distance = Vector3.Distance(new Vector3(to.transform.position.x, 30, transform.position.z), this.transform.position);

            if (distance <= WindRadius) to.IncreaseEfficiency();
            if (distance <= DamageRadius) to.BreakTurbine();
        }

    }

    void Move()
    {

        transform.Translate(-Speed, 0, 0);
    }

    void OnDrawGizmos() {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, WindRadius);
            Gizmos.DrawSphere(transform.position, DamageRadius);
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

