using UnityEngine;
using System.Collections;

public class PlaceObjectOnClick : MonoBehaviour {

	[SerializeField] bool debug;

	[SerializeField] GameObject prefab;

    [SerializeField] float tileSize = 5f;

	public delegate void ObjectPlaced (GameObject go);
	public ObjectPlaced OnObjectPlaced;

	// Use this for initialization
	void Start () {
		//OnObjectPlaced += FindObjectOfType<PowerHUDManager>().;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
			if(hit.collider.gameObject == this.gameObject){
				PlaceObject (hit.point.x, hit.point.z);
			}
		}
	}

	bool PlaceObject(float hx, float hz)
	{
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
		return true;
	}
}
