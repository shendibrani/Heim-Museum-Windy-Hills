using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlaceObjectOnClick : MonoBehaviour, ITouchSensitive, IMouseSensitive
{

    static PlaceObjectOnClick instance;
    public static PlaceObjectOnClick Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlaceObjectOnClick>();
            return instance;
        }
    }

    [SerializeField]
    bool debug;

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    GameObject displayPrefab;

    [SerializeField]
    float tileSize = 5f;

    [SerializeField]
    float holdTime = 30f;

    float timer;

	bool dirtyFlag = true;

	public bool DirtyFlag {
		get {
			return dirtyFlag;
		}
	}

    bool touchPressed;

    GameObject temporaryMill;

    public delegate void ObjectPlaced(GameObject go);
    public ObjectPlaced OnObjectPlaced;

    // Use this for initialization
    void Start()
    {
        //OnObjectPlaced += FindObjectOfType<PowerHUDManager>().;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        if (touchPressed)
        {
            
        }
    }

    public bool TestUICast(float hx, float hz)
    {
        bool hitTestUI = true;

        if (EventSystem.current.IsPointerOverGameObject())
        {
			
            Debug.Log("Clicked on the UI");
            hitTestUI = false;
        }
        if (debug) Debug.Log("raycast hits UI: " + hitTestUI);
        return hitTestUI;
    }

    public void OnClick(ClickState state, RaycastHit hit, Ray ray)
    {
        if (state == ClickState.Pressed) { PointHold(hit.point.x, hit.point.z);
            DisplayObject(hit.point.x, hit.point.z, ray);
        }
        if (state == ClickState.Down) { }
        if (state == ClickState.Up)
        {
            ReleaseHold();
            GameObject.Destroy(temporaryMill);
            temporaryMill = null;
            PlaceObject(hit.point.x, hit.point.z, ray);
        }
    }

    public void OnTouch(Touch t, RaycastHit hit, Ray ray)
    {
        if (t.phase == TouchPhase.Began)
        {
        }
        if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
        {
            DisplayObject(hit.point.x, hit.point.z, ray);
        }
        if (t.phase == TouchPhase.Ended)
        {
            if (debug) Debug.Log("[Placement] Placed " + prefab.name + " at " + hit.point + " at " + Time.time);
            GameObject.Destroy(temporaryMill);
            temporaryMill = null;
            PlaceObject(hit.point.x, hit.point.z, ray);
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

    Collider ConfirmBlockingCast(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    bool DisplayObject(float hx, float hz, Ray ray)
    {
        if (debug) Debug.Log("Checking Temporary");
        if (!dirtyFlag && TestUICast(hx, hz))
        {
            if (debug) Debug.Log("Building points array");
            Vector2[] points = new Vector2[5];

            points[0] = new Vector2(Mathf.Floor(hx / tileSize) * tileSize, Mathf.Floor(hz / tileSize) * tileSize);
            points[1] = new Vector2(points[0].x, points[0].y + tileSize);
            points[2] = new Vector2(points[0].x + tileSize, points[0].y + tileSize);
            points[3] = new Vector2(points[0].x + tileSize, points[0].y);
            points[4] = new Vector2(points[0].x + tileSize / 2, points[0].y + tileSize / 2);

            if (debug) Debug.Log("Starting Foreach");

            bool safe = true;
            if (temporaryMill != null)
            {
                foreach (Vector2 point in points)
                {
                    Vector3 size = GetComponent<TerrainCollider>().terrainData.size;
                    if (GetComponent<TerrainCollider>().terrainData.GetSteepness(point.x / size.x, point.y / size.z) > 0)
                    {
                        if (debug) Debug.Log("Not flat");
                        safe = false;
                        //OnObjectPlaced(null);
                        //return false;
                    }
                    if (ConfirmBlockingCast(ray) != this.GetComponent<Collider>())
                    {
                        if (debug) Debug.Log("Placement obscured");
                        safe = false;
                    }
                    if (debug) Debug.Log("Flat");
                }
                if (!safe)
                {
                    foreach (Renderer r in temporaryMill.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.red;
                        break;
                    }
                }
                else
                {
                    foreach (Renderer r in temporaryMill.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.white;
                        break;
                    }
                }
            }

            if (temporaryMill == null)
            {
                //Snap to grid size according to _snapValue
                if (debug) Debug.Log("Calculating position");
                Vector3 snapPoint = new Vector3(points[4].x, GetComponent<TerrainCollider>().terrainData.GetHeight((int)points[4].x, (int)points[4].y), points[4].y);
                if (debug) Debug.Log("Instantiating");
                GameObject instance = (GameObject)GameObject.Instantiate(displayPrefab, snapPoint, Quaternion.identity);
                //if (OnObjectPlaced != null) OnObjectPlaced(instance);
                temporaryMill = instance;
            }

            if (temporaryMill != null)
            {
                temporaryMill.transform.position = new Vector3(points[4].x, GetComponent<TerrainCollider>().terrainData.GetHeight((int)points[4].x, (int)points[4].y), points[4].y);
            }

            //TutorialProgression.Instance.Placed(instance.GetComponent<TurbineObject>());
            return true;
        }
        return false;
    }

    bool PlaceObject(float hx, float hz, Ray ray)
    {
        if (!dirtyFlag && TestUICast(hx, hz) && ConfirmBlockingCast(ray))
        {
            if (debug) Debug.Log("Building points array");
            Vector2[] points = new Vector2[5];

            points[0] = new Vector2(Mathf.Floor(hx / tileSize) * tileSize, Mathf.Floor(hz / tileSize) * tileSize);
            points[1] = new Vector2(points[0].x, points[0].y + tileSize);
            points[2] = new Vector2(points[0].x + tileSize, points[0].y + tileSize);
            points[3] = new Vector2(points[0].x + tileSize, points[0].y);
            points[4] = new Vector2(points[0].x + tileSize / 2, points[0].y + tileSize / 2);

            if (debug) Debug.Log("Starting Foreach");
            foreach (Vector2 point in points)
            {
                Vector3 size = GetComponent<TerrainCollider>().terrainData.size;
                if (GetComponent<TerrainCollider>().terrainData.GetSteepness(point.x / size.x, point.y / size.z) > 0)
                {
                    if (debug) Debug.Log("Not flat");
                    OnObjectPlaced(null);
                    return false;
                }
                else if (ConfirmBlockingCast(ray) != this.GetComponent<Collider>())
                {
                    if (debug) Debug.Log("Placement obscured");
                    OnObjectPlaced(null);
                    return false;
                }
                else if (debug) Debug.Log("Flat");
            }

            //Snap to grid size according to _snapValue
            if (debug) Debug.Log("Calculating position");
            Vector3 snapPoint = new Vector3(points[4].x, GetComponent<TerrainCollider>().terrainData.GetHeight((int)points[4].x, (int)points[4].y), points[4].y);
            if (debug) Debug.Log("Instantiating");
            GameObject instance = (GameObject)GameObject.Instantiate(prefab, snapPoint, Quaternion.identity);
            if (OnObjectPlaced != null) OnObjectPlaced(instance);

            TutorialProgression.Instance.Placed();
			TutorialProgression.Instance.SetMill(instance.GetComponent<TurbineObject>());
            return true;
        }
        return false;
    }
}
