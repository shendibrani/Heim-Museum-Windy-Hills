using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogOfWarManager : MonoBehaviour {

	[SerializeField] GameObject fogPiecePrefab;
	[SerializeField] int rows, columns;
	[SerializeField] float xOffset, yOffset;

	HashSet<FogOfWarPiece> pieces;

	// Use this for initialization
	void Start () 
	{
		pieces = new HashSet<FogOfWarPiece>();
		BuildFoW();
	}
		
	void Update()
	{
		if(Input.GetMouseButtonDown(0)){
			Reset();
			ShowArea(Input.mousePosition, 200);
		} else if (Input.GetMouseButtonDown(1)){
			Reset();
		}
	}

	void BuildFoW()
	{
		for(int x = 0; x < columns; x++){
			for(int y = 0; y < rows; y++){
				GameObject instance = (GameObject) Instantiate(fogPiecePrefab);
				instance.GetComponent<RectTransform>().parent = GetComponent<RectTransform>();
				instance.GetComponent<RectTransform>().anchoredPosition = new Vector2 (x*xOffset, y*yOffset);
				instance.GetComponent<FogOfWarPiece>().Initialize();
				pieces.Add(instance.GetComponent<FogOfWarPiece>());
			}
		}
	}

	public void ShowArea(Vector2 center, float radius)
	{
		foreach (FogOfWarPiece p in pieces){
			if(Vector2.Distance(p.originalPosition, center) <= radius){
				Debug.Log("Move");
				p.MoveAway(center, radius);
			}
		}
	}

	public void Reset()
	{
		foreach (FogOfWarPiece p in pieces){
			p.MoveBack();
		}
	}
}
