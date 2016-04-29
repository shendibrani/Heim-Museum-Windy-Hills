using UnityEngine;
using System.Collections;

public class PlaceObjectOnClick : MonoBehaviour {

	[SerializeField] GameObject prefab;

    float _snapValue = 5f;

	public delegate void ObjectPlaced (GameObject go);
	public ObjectPlaced OnObjectPlaced;

	// Use this for initialization
	void Start () {
		OnObjectPlaced = FindObjectOfType<PowerHUDManager>().ResetPower;
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

				float x = Mathf.Round(hit.point.x / _snapValue) * _snapValue;
				float z = Mathf.Round(hit.point.z / _snapValue) * _snapValue;

                //Snap to grid size according to _snapValue
				Vector3 snapPoint = new Vector3(x, GetComponent<TerrainCollider>().terrainData.GetHeight((int) x, (int) z), z);

                GameObject instance = (GameObject) GameObject.Instantiate(prefab, snapPoint, Quaternion.identity);
				OnObjectPlaced(instance);
			}
		}
	}
}
