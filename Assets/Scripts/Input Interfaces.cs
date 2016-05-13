using System;
using UnityEngine;

public interface ITouchSensitive
{
	void OnTouch(Touch t, RaycastHit hit);
}

public interface IMouseSensitive
{
	void OnClick(RaycastHit hit);
}