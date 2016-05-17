using UnityEngine;
using System.Collections;

public class CloudGameplayObject : MonoBehaviour, IMouseSensitive, ITouchSensitive {

	bool cloudSelect = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTouch(Touch t, RaycastHit hit)
	{
		//IncreaseEfficiency();
	}

	public void OnClick(ClickState state, RaycastHit hit)
	{	
		if (state == ClickState.Pressed) CloudMove(hit.point.x);
		if (state == ClickState.Down && hit.collider == this) OnCloudSelect(true);
		if (state == ClickState.Up) OnCloudSelect(false);
	}

	void OnCloudSelect(bool state){
			cloudSelect = state;
	}

	void CloudMove(float x){
		if (cloudSelect) this.transform.position.x = x;
	}
}
