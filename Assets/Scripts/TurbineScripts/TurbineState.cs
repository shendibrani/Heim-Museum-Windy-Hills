using UnityEngine;
using System.Collections;

[System.Serializable]
public static class TurbineStateManager {

	public static TurbineState lowFireState {get; private set;}
	public static TurbineState highFireState {get; private set;}
	public static TurbineState saboteurState {get; private set;}
	public static TurbineState brokenState {get; private set;}
	public static TurbineState dirtyState {get; private set;}
	public static TurbineState occupiedState { get; private set; }
    public static TurbineState tutorialLowFireState { get; private set; }
	public static TurbineState turbineCooldownState { get; private set; }

    public static bool Initialize()
	{
		string JSONAggregate = Resources.Load<TextAsset>("States").text;
		string[] objects = JSONAggregate.Split('#');

		lowFireState = JsonUtility.FromJson<TurbineState>(objects[0]);
		highFireState = JsonUtility.FromJson<TurbineState>(objects[1]); 
		saboteurState = JsonUtility.FromJson<TurbineState>(objects[2]);
		brokenState = JsonUtility.FromJson<TurbineState>(objects[3]);
		dirtyState = JsonUtility.FromJson<TurbineState>(objects[4]);
        occupiedState = JsonUtility.FromJson<TurbineState>(objects[5]);
		tutorialLowFireState = JsonUtility.FromJson<TurbineState>(objects[6]);
		turbineCooldownState = JsonUtility.FromJson<TurbineState>(objects[7]);

        Debug.Log(lowFireState.name);
		Debug.Log(highFireState.name);
		Debug.Log(saboteurState.name);
		Debug.Log(brokenState.name);
		Debug.Log(dirtyState.name);
        Debug.Log(occupiedState.name);
		Debug.Log (tutorialLowFireState.name);
		Debug.Log(turbineCooldownState.name);

        return true;
	}

	public static void CreateTemplate ()
	{
		lowFireState = new TurbineState();

		string str = JsonUtility.ToJson(lowFireState,true) + "\n#\n" + 
			JsonUtility.ToJson(lowFireState,true) + "\n#\n" + 
			JsonUtility.ToJson(lowFireState,true) + "\n#\n" + 
			JsonUtility.ToJson(lowFireState,true) + "\n#\n" + 
			JsonUtility.ToJson(lowFireState,true);

		System.IO.File.WriteAllText(@"C:\Users\Giulio Robecchi\Desktop\States.txt", str);
	}
}

public class TurbineState : IMouseSensitive, ITouchSensitive, IWindSensitive
{
	#region Json serialized values

	public string name;

	[SerializeField] public float timer, _efficiencyMultiplyer;

    [SerializeField] bool negativeEffect, timed, endOnTap, endOnWind, winzoneDependent, setsOnHighFire, breaksTurbine, dirtiesTurbine, setsOccupied;

	[SerializeField] bool endOnPolice, endOnFiremen, endOnRepair, endOnCleanup, isCooldown;

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
	void SetOwner(TurbineObject pOwner)
	{
		owner = pOwner;
		owner.state.value = this;
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

	public void OnClick (ClickState state, RaycastHit hit, Ray ray)
	{
		if(endOnTap){
			if(state == ClickState.Down) End(true);
		}
	}

	public void OnTouch(Touch t, RaycastHit hit, Ray ray)
	{
		if(endOnTap){
			if(t.phase == TouchPhase.Ended) End(true);
		}
	}

	public void OnEnterWindzone ()
	{
		if(endOnWind) End(true);
	}

	public void OnStayWindzone () { if (endOnWind) End(true); }

	public void OnExitWindzone ()
	{
		if(winzoneDependent) End(true);
	}

	#endregion

	#region Menu Callbacks

	public void OnPolice()
	{
		if(endOnPolice) End(true);
	}

	public void OnFiremen()
	{
		if(endOnFiremen) End(true);
	}

	public void OnRepair()
	{
		if(endOnRepair) End(true);
	}

	public void OnCleanup()
	{
		if(endOnCleanup) End(true);
	}

	#endregion

	public void End (bool solved)
	{
	
		if (!solved && negativeEffect) {
			Debug.Log ("[State] State " + name + " failed at " + Time.time);
			Fail ();
		} else {
			Debug.Log ("[State] State " + name + " ended  with " + timer + "seconds to spare at " + Time.time);
			//owner.state.value = null;
			TurbineStateManager.turbineCooldownState.Copy(owner, true);
			//owner.state.value = null;
		}
	}

	public void Fail() 
	{
		if(setsOnHighFire){
			TurbineStateManager.highFireState.Copy(owner, true);
		}

		if(breaksTurbine){
			TurbineStateManager.brokenState.Copy(owner);
		}

		if(dirtiesTurbine){
			TurbineStateManager.dirtyState.Copy(owner);
		}

        if (setsOccupied)
        {
            TurbineStateManager.occupiedState.Copy(owner, true);
        }
		if (isCooldown)
		{
			owner.state.value = null;
		}
	}

	/// <summary>
	/// Returns a copy of this instance, with the specificed owner.
	/// forceThrough forces state, ignoring pause and other states.
	/// </summary>
	/// <param name="pOwner">The owner of the new instance.</param>
	public TurbineState Copy(TurbineObject pOwner, bool forceThrough = false)
	{
		if (!forceThrough && TutorialProgression.Instance.ProgressPause)
		{
			Debug.Log("no forced through, blocked by pause.");
			return null;
		}
		else if (!forceThrough && pOwner.state.value != null)
		{
			Debug.Log("no forced through, blocked by null.");
			return null;
		}
		else {
			Debug.Log("not blocked");
			TurbineState ts = new TurbineState();

			ts.name = this.name;

			ts.timer = this.timer;
			ts._efficiencyMultiplyer = this._efficiencyMultiplyer;

			ts.negativeEffect = this.negativeEffect;
			ts.timed = this.timed;
			ts.endOnTap = this.endOnTap;
			ts.endOnWind = this.endOnWind;
			ts.winzoneDependent = this.winzoneDependent;
			ts.setsOnHighFire = this.setsOnHighFire;
			ts.breaksTurbine = this.breaksTurbine;
			ts.dirtiesTurbine = this.dirtiesTurbine;
			ts.setsOccupied = this.setsOccupied;

			ts.endOnCleanup = this.endOnCleanup;
			ts.endOnFiremen = this.endOnFiremen;
			ts.endOnPolice = this.endOnPolice;
			ts.endOnRepair = this.endOnRepair;
			ts.isCooldown = this.isCooldown;

			ts.SetOwner(pOwner);

			Debug.Log("[State] State " + name + " started at " + Time.time);
			return ts;
		}
	}
}
