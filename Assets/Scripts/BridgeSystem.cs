using UnityEngine;
using System.Collections;

public class BridgeSystem : MonoBehaviour
{

    Animator animation;
	public bool a;

    void Start()
    {
        animation = GetComponent<Animator>();
    }

	void Update()
	{
		animation.SetBool ("Open", a);
	}

    void OnTriggerStay(Collider col)
	{
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			col.gameObject.GetComponent<Boat> ().b = this;
			animation.SetBool ("Open", true);
			a = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			animation.SetBool ("Open", false);
			col.gameObject.GetComponent<Boat> ().b = null;
			a = false;
        }
    }
}
