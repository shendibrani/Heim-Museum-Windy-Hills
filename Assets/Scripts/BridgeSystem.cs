using UnityEngine;
using System.Collections;

public class BridgeSystem : MonoBehaviour
{

    NavMeshObstacle obstacle;
    Animator animation;
    // Use this for initialization
    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame

    float Distance;
   
    void Update()
    {

 

    }

    void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Boat")
        {
            obstacle.enabled = false;
            animation.speed = -1;
            animation.Play("Bridge_Animation");

        }
        //Activate animation

    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Boat")
        {
            animation.speed = 1;
            animation.Play("Bridge_Animation");
            obstacle.enabled = true;
        }
        //Activate Animation
    }

}
