using System;

public class Monitored<T>
{
	T _value;

	public T value {
		get{
			return _value;
		}
		set{
			OnValueChanged(_value, value);
			_value = value;
		}
	}

	public Monitored()
	{
	}

	public delegate void ValueChanged (T oldValue, T newValue);
	public ValueChanged OnValueChanged;

	public static implicit operator T(Monitored<T> instance){
		return instance.value;
	}
}

