using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGameplayObject : MonoBehaviour, ITouchSensitive, IMouseSensitive {

	bool cloudSelect = false;

    [SerializeField]
    GameObject cloudObject;

    [SerializeField]
    GameObject windParticles;

    [SerializeField]
    float extent;

    [SerializeField]
    float radius;

    [SerializeField]
    bool _debug;

    HashSet<IWindSensitive> interfaces;
    HashSet<IWindSensitive> tmpList = new HashSet<IWindSensitive>();
    HashSet<IWindSensitive> deleteList = new HashSet<IWindSensitive>();

    RaycastHit hitTarget;

    // Use this for initialization
    void Start () {
        interfaces = new HashSet<IWindSensitive>();
		cloudObject = this.gameObject;
        TurbineObject.windVelocity.value = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
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
    void Update()
    {

        //HashSet<IWindSensitive> deleteList = new HashSet<IWindSensitive>();

        if (TutorialProgression.Instance.IsComplete && cloudSelect)
        {

            CloudMove();
            RaycastHit[] hits;
            hits = Physics.BoxCastAll(cloudObject.transform.position, new Vector3(radius, radius * 4, radius), TurbineObject.windVelocity);
            if (hits.Length != 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    IWindSensitive tmpCollider = hit.collider.GetComponent<IWindSensitive>();
                    if (tmpCollider != null)
                    {
                        tmpList.Add(tmpCollider);
                        if (!interfaces.Contains(tmpCollider))
                        {
                            interfaces.Add(tmpCollider);
                            tmpCollider.OnEnterWindzone();
                        }
                        else {
                            tmpCollider.OnStayWindzone();
                        }
                    }
                }
            }

            HashSet<IWindSensitive> tmpIntersect = new HashSet<IWindSensitive>(interfaces);
            tmpIntersect.ExceptWith(tmpList);
            interfaces.IntersectWith(tmpList);

            if (tmpIntersect.Count != 0)
            {
                foreach (IWindSensitive i in tmpIntersect)
                {
                    {
                        i.OnExitWindzone();
                    }
                }
            }
        }
        if (!cloudSelect)
        {
            foreach (IWindSensitive i in interfaces)
            {
                i.OnExitWindzone();
            }
            interfaces = new HashSet<IWindSensitive>();
        }
        
        if (windParticles != null)
        {
            windParticles.SetActive(cloudSelect);
        }
        OnCloudSelect(false, hitTarget);
    }

	public void OnTouch(Touch t, RaycastHit hit)
	{
		//IncreaseEfficiency();
		if (t.phase == TouchPhase.Ended) {
			OnCloudSelect (false, hit);
		} else if (t.phase != TouchPhase.Canceled) {
			OnCloudSelect (true, hit);
		}
	}

	public void OnClick(ClickState state, RaycastHit hit)
	{	
		if (state == ClickState.Pressed) OnCloudSelect(true, hit);
        if (state == ClickState.Down) OnCloudSelect(true, hit);
        if (state == ClickState.Up) OnCloudSelect(false, hit);
        //else OnCloudSelect(false);
	}

	void OnCloudSelect(bool state, RaycastHit pHit){
			cloudSelect = state;
            hitTarget = pHit;
           // Debug.Log("State: " + state);
	}
    

	public void CloudMove(){
        //RaycastHit hit;
        //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 transformedPoint = transform.parent.InverseTransformPoint(hitTarget.point);
        //cloudObject.transform.localPosition = new Vector3(x * extent,cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
        transformedPoint = new Vector3(transformedPoint.x, cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
        //cloudObject.transform.localPosition = transformedPoint;
        //cloudObject.transform.localPosition = new Vector3(transformedPoint.x, cloudObject.transform.localPosition.y, cloudObject.transform.localPosition.z);
        cloudObject.transform.localPosition = Vector3.Lerp(transform.localPosition, transformedPoint, 0.9f);
    }
}
