using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGameplayObject : MonoBehaviour {

	bool cloudSelect = false;

    [SerializeField]
    GameObject cloudObject;

    [SerializeField]
    float extent;

    [SerializeField]
    float radius;

    [SerializeField]
    bool _debug;

    HashSet<IWindSensitive> interfaces;

    // Use this for initialization
    void Start () {
        interfaces = new HashSet<IWindSensitive>();
		cloudObject = this.gameObject;
        //TurbineObject.windVelocity.value = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        cloudObject.transform.localPosition = new Vector3(0, cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
    }

    void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(cloudObject.transform.position, new Vector3(radius, radius * 4, radius));
        }
    }

    // Update is called once per frame
    void Update () {

        HashSet<IWindSensitive> tmpList = new HashSet<IWindSensitive>();
        HashSet<IWindSensitive> deleteList = new HashSet<IWindSensitive>();

        RaycastHit[] hits;
        hits = Physics.BoxCastAll(cloudObject.transform.position, new Vector3(radius, radius * 4, radius), TurbineObject.windVelocity);
        foreach(RaycastHit hit in hits)
        {
            IWindSensitive tmpCollider = hit.collider.GetComponent<IWindSensitive>();
            if (tmpCollider != null)
            {
                tmpList.Add(tmpCollider);
                if (!interfaces.Contains(tmpCollider))
                {
                    interfaces.Add(tmpCollider);
                    hit.collider.GetComponent<IWindSensitive>().OnEnterWindzone();
                }
            }
        }
        foreach (IWindSensitive i in interfaces)
        {
            if (tmpList.Contains(i))
            {
                //onstaywindzone if needed
            }
            else
            {
                i.OnExitWindzone();
                deleteList.Add(i);
            } 
        }
        foreach(IWindSensitive i in deleteList)
        {
            interfaces.Remove(i);
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
