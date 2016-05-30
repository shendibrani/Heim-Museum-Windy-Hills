﻿using UnityEngine;
using System.Collections;

public class StormBehavior : MonoBehaviour, IWindSensitive
{

    float WindRadius = 40.0f;
    float DamageRadius =30.0f;

	public float speed;

    bool debug = true;
    // Update is called once per frame
    void Update()
    {

        Move();
        if (EnteredWindzone) transform.Translate(speed, 0, speed);
        WindEffect();
        DestroyCloud();
    }

    void WindEffect() {
        foreach (TurbineObject to in TurbineObject.all) {
            float distance = Vector3.Distance(new Vector3(to.transform.position.x, 0, transform.position.z), new Vector3(this.transform.position.x, 0, this.transform.position.z));

            if (distance <= WindRadius) to.IncreaseEfficiency();
            if (distance <= DamageRadius) to.BreakTurbine();
        }

    }

    void Move()
    {
        transform.Translate(-speed, 0, 0);
    }

    void OnDrawGizmos() {
        if (debug)
        {
            Gizmos.color = Color.blue;
           // Gizmos.DrawSphere(transform.position, WindRadius);
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

