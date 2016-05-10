using System;

public class Monitored<T>
{
	T _value;

	public T value {
		get{
			return _value;
		}
		set{
			if(OnValueChanged != null){
				OnValueChanged(_value, value);
			}
			_value = value;
		}
	}

	public Monitored()
	{
	}

	public Monitored(T pValue)
	{
		value = pValue;
	}

	public delegate void ValueChanged (T oldValue, T newValue);
	public ValueChanged OnValueChanged;

	public static implicit operator T(Monitored<T> instance){
		return instance.value;
	}
}

