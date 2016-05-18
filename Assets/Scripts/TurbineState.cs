using UnityEngine;
using System.Collections;

[System.Serializable]
public static class TurbineStateManager {

	public static TurbineState lowFireState {get; private set;}
	public static TurbineState highFireState {get; private set;}
	public static TurbineState saboteurState {get; private set;}
	public static TurbineState brokenState {get; private set;}
	public static TurbineState dirtyState {get; private set;}

	public static bool Initialize()
	{
        return true;
	}
}

public class TurbineState : IMouseSensitive, ITouchSensitive, IWindSensitive
{
	#region Json serialized values

	public string name;

    [SerializeField]
    float timer;
    [SerializeField]
    float  _efficiencyMultiplyer = 1;

	[SerializeField] bool negativeEffect, timed, endsOnTap, endsOnWind, winzoneDependent, setsOnHighFire, breaksTurbine, dirtiesTurbine;

	#endregion

	#region Properties

	protected TurbineObject owner {get; private set;}

	public float efficiencyMultiplyer {get {return _efficiencyMultiplyer;}}

	#endregion

	#region Building Functions

	public TurbineState(){}

	/// <summary>
	/// Sets the owner of the state and adds it to its HashSet.
	/// Call this after cloning a static instance.
	/// </summary>
	/// <param name="pOwner">The owner.</param>
	public void SetOwner(TurbineObject pOwner)
	{
		owner = pOwner;
		owner.AddState(this);
	}
		
	#endregion

	#region Forwared calls

	public virtual void Update () 
	{
		if(timed){
			timer -= Time.deltaTime;

			if (timer <= 0) {
				End (false);
			}
		}
	}

	public void OnClick (ClickState state, RaycastHit hit)
	{
		if(endsOnTap){
			if(state == ClickState.Down) End(true);
		}
	}

	public void OnTouch(Touch t, RaycastHit hit)
	{
		if(endsOnTap){
			if(t.phase == TouchPhase.Ended) End(true);
		}
	}

	public void OnEnterWindzone ()
	{
		if(endsOnWind) End(true);
	}
	public void OnExitWindzone ()
	{
		if(winzoneDependent) End(true);
	}

	#endregion

	public void End (bool solved)
	{
		if(solved) {
			owner.RemoveState(this);
		} else {
			if(negativeEffect) Fail();
			owner.RemoveState(this);
		}
	}

	public void Fail() 
	{
		TurbineState state;

		if(setsOnHighFire){
			state = TurbineStateManager.highFireState;
			state.SetOwner(owner);
		}

		if(breaksTurbine){
			state = TurbineStateManager.brokenState;
			state.SetOwner(owner);
		}

		if(dirtiesTurbine){
			state = TurbineStateManager.dirtyState;
			state.SetOwner(owner);
		}
	}
}
