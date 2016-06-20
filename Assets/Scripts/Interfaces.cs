using System;
using UnityEngine;

public interface ITouchSensitive
{
	void OnTouch(Touch t, RaycastHit hit, Ray ray);
}

public interface IMouseSensitive
{
	void OnClick(ClickState state, RaycastHit hit, Ray ray);
}

public interface IWindSensitive
{
	void OnEnterWindzone ();
    void OnStayWindzone ();
    void OnExitWindzone();
}

public enum ClickState {
	Down, Pressed, Up
}