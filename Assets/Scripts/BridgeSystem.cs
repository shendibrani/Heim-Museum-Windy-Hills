using UnityEngine;
using System.Collections;

public class BridgeSystem : MonoBehaviour
{

    Animator animation;

    void Start()
    {
        animation = GetComponent<Animator>();
    }


    void OnTriggerStay(Collider col)
	{
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			animation.SetBool ("Open", true);
        }
    }

    void OnTriggerExit(Collider col)
    {
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			animation.SetBool ("Open", false);
        }
    }
}
