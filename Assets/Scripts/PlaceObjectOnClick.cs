using UnityEngine;
using System.Collections;

public class PlaceObjectOnClick : MonoBehaviour {

	[SerializeField] GameObject prefab;

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
				GameObject instance = (GameObject) GameObject.Instantiate(prefab, hit.point, Quaternion.identity);
			}
		}
	}
}
