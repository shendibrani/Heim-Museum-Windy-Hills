using UnityEngine;
using System.Collections;

public class CloudGameplayObject : MonoBehaviour {

	bool cloudSelect = false;

    [SerializeField]
    GameObject cloudObject;

    [SerializeField]
    float extent;

    [SerializeField]
    float radius;

	// Use this for initialization
	void Start () {
		cloudObject = this.gameObject;
        TurbineObject.windVelocity.value = transform.TransformDirection(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        cloudObject.transform.localPosition = new Vector3(0, cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hits;
        //Ray ray = new Ray();
        hits = Physics.BoxCastAll(cloudObject.transform.position, new Vector3(radius, radius * 4, radius), TurbineObject.windVelocity);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<IWindSensitive>() != null) hit.collider.GetComponent<IWindSensitive>().OnEnterWindzone();
        }
	}

	/*public void OnTouch(Touch t, RaycastHit hit)
	{
		//IncreaseEfficiency();
	}

	public void OnClick(ClickState state, RaycastHit hit)
	{	
		if (state == ClickState.Pressed) CloudMove(hit);
		if (state == ClickState.Down) OnCloudSelect(true);
        if (state == ClickState.Up) OnCloudSelect(false);
        //else OnCloudSelect(false);
	}

	void OnCloudSelect(bool state){
			cloudSelect = state;
            Debug.Log("State: " + state);
	}
    */

	public void CloudMove(float x){
        //Vector3 transformedPoint = transform.InverseTransformPoint(hit.point);
		cloudObject.transform.localPosition = new Vector3(x * extent,cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
	}
}
