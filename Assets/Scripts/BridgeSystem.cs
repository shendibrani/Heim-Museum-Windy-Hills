using UnityEngine;
using System.Collections;

public class BridgeSystem : MonoBehaviour
{

    Animator animation;
	public bool open;

    void Start()
    {
        animation = GetComponent<Animator>();
    }

	void Update()
	{
		animation.SetBool ("Open", open);
	}

    void OnTriggerStay(Collider col)
	{
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			col.gameObject.GetComponent<Boat> ().bridge = this;
			animation.SetBool ("Open", true);
			open = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
		if (col.gameObject.GetComponent<Boat>() != null)
        {
			animation.SetBool ("Open", false);
			col.gameObject.GetComponent<Boat> ().bridge = null;
			open = false;
        }
    }
}
