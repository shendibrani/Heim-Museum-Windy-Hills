using System;
using UnityEngine;

public interface ITouchSensitive
{
	void OnTouch(Touch t, RaycastHit hit);
}

public interface IMouseSensitive
{
	void OnClick(ClickState state, RaycastHit hit);
}

public interface IWindSensitive
{
	void OnEnterWindzone ();
    void OnExitWindzone();
}

public enum ClickState {
	Down, Pressed, Up
}