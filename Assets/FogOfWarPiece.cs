using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class FogOfWarPiece : MonoBehaviour {

	public Vector2 originalPosition {get; private set;}

	Vector2 currentTargetPositon;

	public void Initialize () 
	{
		originalPosition = GetComponent<RectTransform>().anchoredPosition;
		currentTargetPositon = originalPosition;
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

		Debug.Log(point);
	}

	public void MoveBack()
	{
		currentTargetPositon = originalPosition;
	}
}
