using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TurbineObject))]
public class TurbineMenu : MonoBehaviour
{
    [SerializeField]
    GameObject menuParent;

    Quaternion rotation;

	// Use this for initialization
	void Start ()
	{
        rotation = menuParent.transform.rotation;
		Hide();
        //menuParent.GetComponent<LookAtTransformBehaviour>().SetTarget(Camera.main.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
        menuParent.transform.rotation = rotation;
	}

	public void Show()
	{
        menuParent.SetActive(true);
	}

	public void Hide()
	{
        menuParent.SetActive(false);
	}
}

