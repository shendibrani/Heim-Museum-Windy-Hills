using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Cached<T> where T : MonoBehaviour
{
	IEnumerable<T> all {get;}
}

