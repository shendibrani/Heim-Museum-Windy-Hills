using UnityEngine;
using System.Collections;

public class PlaceObjectOnClick : MonoBehaviour {

	[SerializeField] GameObject prefab;

    float _snapValue = 5f;

	// Use this for initialization
	void Start () {
	
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

                //Snap to grid size according to _snapValue
                Vector3 snapPoint = new Vector3(Mathf.Round(hit.point.x / _snapValue) * _snapValue,
                             Mathf.Round(hit.point.y / _snapValue) * _snapValue,
                             Mathf.Round(hit.point.z / _snapValue) * _snapValue);

                GameObject instance = (GameObject) GameObject.Instantiate(prefab, snapPoint, Quaternion.identity);
                FindObjectOfType<PowerHUDManager>().ResetPower();
			}
		}
	}
}
