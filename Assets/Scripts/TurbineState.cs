using UnityEngine;
using System.Collections;

public abstract class TurbineState : IMouseSensitive, ITouchSensitive, IWindSensitive
{
	protected TurbineObject owner {get; private set;}

	float timer;

	public TurbineState(TurbineObject pOwner)
	{
		owner = pOwner;
		owner.AddState(this);
	}

	// Update is called once per frame
	public virtual void Update () 
	{
		timer -= Time.deltaTime;

		if (timer <= 0) {
			End (false);
		}
	}

	public virtual void OnClick (ClickState state, RaycastHit hit){}
	public virtual void OnTouch(Touch t, RaycastHit hit) {}
	public virtual void OnEnterWindzone (){}

	public virtual void End (bool solved)
	{
		if(solved) {
			owner.RemoveState(this);
		} else {
			
			owner.RemoveState(this);
		}
	}

	public abstract void Fail();
}

public class LowFireState : TurbineState
{
	public LowFireState(TurbineObject pOwner) : base(pOwner){}

	public override void OnEnterWindzone ()
	{
		End(true);
	}

	public override void Fail ()
	{
		owner.AddState(new HighFireState(owner));
	}
}

public class HighFireState : TurbineState
{
	int touchcount = 5;

	public HighFireState(TurbineObject pOwner) : base(pOwner){}

	public override void OnTouch (Touch t, RaycastHit hit)
	{
		if(t.phase == TouchPhase.Ended) {
			touchcount--;
			if(touchcount <=0) {
				End(true);
			}
		}
	}

	public override void OnClick (ClickState state, RaycastHit hit)
	{
		if(state == ClickState.Down) {
			touchcount--;
			if(touchcount <=0) {
				End(true);
			}
		}
	}

	public override void Fail ()
	{
		owner.Damage();
	}
}

public class SaboteurState : TurbineState
{
	public SaboteurState(TurbineObject pOwner) : base(pOwner){}

	public override void OnClick (ClickState state, RaycastHit hit)
	{
		if(state == ClickState.Down)	End(true);
	}

	public override void OnTouch (Touch t, RaycastHit hit)
	{
		if(t.phase == TouchPhase.Ended)	End(true);
	}

	public override void Fail ()
	{
		owner.Damage();
	}
}

public class BrokenState : TurbineState
{
	public BrokenState(TurbineObject pOwner) : base(pOwner){}

	public override void OnEnterWindzone ()
	{
		End(true);
	}

	public override void Fail ()
	{
		owner.AddState(new HighFireState(owner));
	}
}