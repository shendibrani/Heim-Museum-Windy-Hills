using UnityEngine;
using System.Collections;

public class UICallbacksSystem : MonoBehaviour
{
	public static UIState currentState;

    [SerializeField]
    Highlightable police, firemen, repair, cleanup;

    public void OnPolice()
    {
        if (ClearCheck(UIState.Police))
        {
            police.SetHighlight(true);
            currentState = UIState.Police;
        }
    }

    public void OnFiremen()
    {
        if (ClearCheck(UIState.Firemen))
        {
            firemen.SetHighlight(true);
            currentState = UIState.Firemen;
        }
    }

    public void OnRepair()
    {
        if (ClearCheck(UIState.Repair))
        {
            repair.SetHighlight(true);
            currentState = UIState.Repair;
        }
    }

    public void OnCleanup()
    {
        if (ClearCheck(UIState.Cleanup))
        {
            cleanup.SetHighlight(true);
            currentState = UIState.Cleanup;
        }
    }

    public void Clear()
    {
        currentState = UIState.None;

        police.SetHighlight(false);
        firemen.SetHighlight(false);
        repair.SetHighlight(false);
        cleanup.SetHighlight(false);
    }

    public bool ClearCheck(UIState state)
    {
        if (currentState == state)
        {
            Clear();
            return false;
        }
        else {
            Clear();
            return true;
        }
    }
}

public enum UIState
{
    None, Police, Firemen, Repair, Cleanup
}
