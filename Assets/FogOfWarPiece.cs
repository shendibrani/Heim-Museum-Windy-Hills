using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class FogOfWarPiece : MonoBehaviour {

	Vector2 originalPosition;

	Vector2 currentTargetPositon;

	// Use this for initialization
	void Start () 
	{
		originalPosition = GetComponent<RectTransform>().anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, currentTargetPositon, 0.5f);
	}

	public void MoveAway(Vector2 point, float radius)
	{
		Vector2 direction = (GetComponent<RectTransform>().anchoredPosition - point).normalized;

		currentTargetPositon = originalPosition + (direction * radius);
	}

	public void MoveBack()
	{
		currentTargetPositon = originalPosition;
	}
}
