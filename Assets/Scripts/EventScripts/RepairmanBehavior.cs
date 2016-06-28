﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class RepairmanBehavior : MonoBehaviour
{

    Stopwatch timer;
    public TurbineObject targetTurbine;
    RepairsDepartment station;
    NavMeshAgent agent;
    public bool busy;

    void Start()
    {
        timer = new Stopwatch();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10.0f;
        station = FindObjectOfType<RepairsDepartment>();
        Dispatcher<RepairMessage>.Subscribe(SendRepairman);
    }

    bool arrived = false;
    bool extinguished;
    void Update()
    {
        if (targetTurbine != null)
        {
            float distanceToTurbine = Vector3.Distance(targetTurbine.transform.position, transform.position);
            float distanceToStation = Vector3.Distance(transform.position, station.transform.position);
            UnityEngine.Debug.Log("Distance to turbine: " + distanceToTurbine);

            if (distanceToTurbine < 8.0f && !arrived)
            {
                timer.Start();
                arrived = true;
                UnityEngine.Debug.Log("Fireman moved to the turbine");
                busy = true;
            }

            if (timer.Elapsed.Seconds > 0.1f)
            {
                timer.Stop();
                timer.Reset();
                agent.SetDestination(station.transform.position);
                extinguished = true;
                targetTurbine.state.value.OnRepair();
            }

            if (arrived && extinguished && distanceToStation < 5.0f)
            {
                arrived = false;
                extinguished = false;
                busy = false;
                targetTurbine = null;
            }
        }
    }

    public void SendRepairman(RepairMessage fm)
    {
        if (fm.Sender.GetComponent<TurbineObject>() == targetTurbine && !extinguished)
        {
            agent.SetDestination(targetTurbine.transform.position);
        }

    }

    public void SetTargetTurbine(TurbineObject to)
    {
        targetTurbine = to;
    }

    void OnDestroy()
    {
        Dispatcher<RepairMessage>.Unsubscribe(SendRepairman);
    }
}