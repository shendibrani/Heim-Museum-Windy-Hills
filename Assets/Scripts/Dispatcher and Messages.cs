using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Message
{
	public readonly GameObject Sender;

	public Message(GameObject sender) 
	{
		Sender = sender;
	}
}

public class Dispatcher<T> where T: Message
{
    public delegate void DispatcherCallback (T instance);

    static DispatcherCallback Callback;

    public static void Dispatch(T instance)
    {
        if (Callback != null)
        {
            Callback(instance);
        }
    }

    public static void Subscribe(DispatcherCallback d)
    {
        Callback += d;
    }

    public static void Unsubscribe (DispatcherCallback d)
    {
        Callback -= d;
    }

	public static void ClearSubscriptionList()
	{
		Callback = null;
	}
}

public class FiremenMessage : Message
{
	public FiremenMessage (GameObject sender) : base(sender) {}
}

public class PoliceMessage : Message
{
	public PoliceMessage (GameObject sender) : base(sender) {}
}

public class RepairMessage : Message
{
	public RepairMessage (GameObject sender) : base(sender) {}
}

public class CleanupMessage : Message
{
	public CleanupMessage (GameObject sender) : base(sender) {}
}