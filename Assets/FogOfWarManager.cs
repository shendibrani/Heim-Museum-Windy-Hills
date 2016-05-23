using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogOfWarManager : MonoBehaviour {

	HashSet<FogOfWarPiece> pieces;

	// Use this for initialization
	void Start () {
		pieces = new HashSet<FogOfWarPiece>(GetComponentsInChildren<FogOfWarPiece>());
	}

	public void ShowArea(Vector2 center, float radius)
	{
		foreach (FogOfWarPiece p in pieces){
			if(Vector2.Distance(p.transform.position, center) < radius){
				p.MoveAway(center, radius);
			}
		}
	}
}
