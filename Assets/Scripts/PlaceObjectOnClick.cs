﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlaceObjectOnClick : MonoBehaviour, ITouchSensitive, IMouseSensitive {

    static PlaceObjectOnClick instance;
    public static PlaceObjectOnClick Instance
    {
       get { if (instance == null) instance = FindObjectOfType<PlaceObjectOnClick>();
            return instance;
        }
    }

	[SerializeField] bool debug;

	[SerializeField] GameObject prefab;

    [SerializeField] float tileSize = 5f;

    [SerializeField]
    float holdTime = 30f;

    float timer;

	bool dirtyFlag = true;

	public delegate void ObjectPlaced (GameObject go);
	public ObjectPlaced OnObjectPlaced;

	// Use this for initialization
	void Start () {
		//OnObjectPlaced += FindObjectOfType<PowerHUDManager>().;
	}

    void OnDestroy()
    {
        instance = null;
    }

    public bool TestUICast(float hx, float hz)
    {
        bool hitTestUI = true;
        // get pointer event data, then set current mouse position
        /*PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(hx, hz);

        // create an empty list of raycast results
        List <RaycastResult> hits = new List<RaycastResult>();

        // ray cast into UI and check for hits
        EventSystem.current.RaycastAll(ped, hits);

        foreach(RaycastResult r in hits)
        {
            if (r.gameObject.GetComponent<RectTransform>() != null & r.gameObject.layer == 1)
            {
                hitTestUI = true;
                break;
            }
        }*/
        if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                hitTestUI = false;
            }
        if (debug) Debug.Log("raycast hits UI: " + hitTestUI);
        return hitTestUI;
    }

    public void OnClick(ClickState state, RaycastHit hit)
	{
		if (state == ClickState.Pressed) PointHold(hit.point.x, hit.point.z);
        if (state == ClickState.Down) PlaceObject (hit.point.x, hit.point.z);
        if (state == ClickState.Up) ReleaseHold();
	}

	public void OnTouch(Touch t, RaycastHit hit)
	{
		if(t.phase == TouchPhase.Ended){
			if(debug) Debug.Log ("[Placement] Placed " + prefab.name + " at " + hit.point + " at " + Time.time);
			PlaceObject (hit.point.x, hit.point.z);
		}
	}
    public void SetDirty(bool s)
    {
        dirtyFlag = s;
    }

    bool PointHold(float hx, float hz)
    {
        timer += Time.deltaTime;
        if (timer > holdTime) return false;
        return true;
    }

    bool ReleaseHold()
    {
        timer = 0;
        return true;
    }

    bool PlaceObject(float hx, float hz)
	{
		if(!FindObjectOfType<TurbineLimitManager>().isCap && !dirtyFlag && TestUICast(hx, hz)){
			if(debug) Debug.Log("Building points array");
			Vector2[] points = new Vector2[5];

			points[0] = new Vector2(Mathf.Floor (hx / tileSize) * tileSize, Mathf.Floor (hz / tileSize) * tileSize);
			points[1] = new Vector2(points[0].x, points[0].y+tileSize);
			points[2] = new Vector2(points[0].x+tileSize, points[0].y+tileSize);
			points[3] = new Vector2(points[0].x+tileSize, points[0].y);
			points[4] = new Vector2(points[0].x+tileSize/2, points[0].y+tileSize/2);

			if(debug) Debug.Log("Starting Foreach");
			foreach (Vector2 point in points){
				Vector3 size = GetComponent<TerrainCollider> ().terrainData.size;
				if (GetComponent<TerrainCollider> ().terrainData.GetSteepness(point.x/size.x, point.y/size.z) > 0){
					if(debug) Debug.Log("Not flat");
					OnObjectPlaced(null);
					return false;
				}
				if(debug) Debug.Log("Flat");
			}

			//Snap to grid size according to _snapValue
			if(debug) Debug.Log("Calculating position");
			Vector3 snapPoint = new Vector3 (points[4].x, GetComponent<TerrainCollider> ().terrainData.GetHeight ((int)points[4].x, (int)points[4].y), points[4].y);
			if(debug) Debug.Log("Instantiating");
			GameObject instance = (GameObject)GameObject.Instantiate (prefab, snapPoint, Quaternion.identity); 
	        if (OnObjectPlaced != null) OnObjectPlaced (instance);

			TutorialProgression.Instance.Placed(instance.GetComponent<TurbineObject>());
            return true;
		}
		return false;
	}
}
