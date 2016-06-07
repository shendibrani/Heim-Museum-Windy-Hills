using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	//[SerializeField] CameraPosition[] Waypoints;
	[SerializeField] [Range(0,1)] float Speed = 0.5f;

	[SerializeField] Transform StartTransform;

	Vector3 _offset;
	bool _reachedGoal = true;
	Vector3 _origin;
	Vector3 _target;
	float _targetZoom;
	float _move = 0;
	float _size = 0;
	bool zoomOut = false;
	float zoomOutSize = 0;

    [System.Serializable]
	public struct CameraPosition
    {
        public Transform waypoint;
    }

	void Start ()
	{
		_offset = Camera.main.transform.position - StartTransform.position;
    }


	public void SetTargetZoom (float pZoom)
	{
		_targetZoom = pZoom;
	}
	public void SetZoomOutSize (float pZoom)
	{
		zoomOutSize = pZoom;
	}

	public void StartMovement(Transform pTarget)
	{
		_size = Camera.main.GetComponent<Camera> ().orthographicSize;
		_origin = Camera.main.transform.position +  _offset;
		_target = pTarget.position + _offset;

		if (zoomOutSize == 0)
		{
			zoomOut = false;
		}
		else
		{
			zoomOut = true;
		}

		_move = 0;
		_reachedGoal = false;
	}



	void Update ()
	{
		if (!_reachedGoal)
		{
			if (_move >= 1)
			{
				_reachedGoal = true;
			}

			if (_move < 1 && zoomOut)
			{
				_move += Speed;

				if (_move >= 0.5)
				{
					_size = _targetZoom;
				}

				Camera.main.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size, zoomOutSize, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move) ) + 1)*0.5f );

				Camera.main.transform.position = Vector3.Lerp (_origin, _target, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f);
			}

			if (_move < 1 && !zoomOut)
			{
				_move += Speed;

				Camera.main.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size,_targetZoom, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f );

				Camera.main.transform.position = Vector3.Lerp (_origin, _target, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f);
			}
		}
	}

	public bool FinishedMoving()
	{
		return _reachedGoal;
	}

	public void GetReference(Cutscene pScript)
	{
		pScript.SetBoolReference (FinishedMoving);
	}
}
