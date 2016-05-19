using UnityEngine;
using System.Collections;

public abstract class EventButtons : MonoBehaviour, ITouchSensitive, IMouseSensitive {

    public void OnTouch(Touch t, RaycastHit hit)
    {

    }
    public void OnClick(ClickState state, RaycastHit hit)
    {

    }
}
