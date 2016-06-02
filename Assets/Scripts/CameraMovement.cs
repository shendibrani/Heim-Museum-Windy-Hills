using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField] CameraPosition[] Waypoints;
	[SerializeField] [Range(0,1)] float Speed = 0.5f;
	[SerializeField] float ZoomOut = 0;

	Vector3 _offset;
	bool _reachedGoal = true;
	Vector3 _origin;
	Vector3 _target;
	float _targetZoom;
	int _progress = 0;
	float _move = 0;
	float _size = 0;
    Camera Camera.main;
	bool start = true;

    [System.Serializable]
    struct CameraPosition
    {
        public Transform waypoint;
        public float zoom;
		public bool zoomOut;
    }

	void Start ()
	{
		_offset = Camera.main.transform.position - Waypoints [_progress].waypoint.position;
		Debug.Log (_offset);
    }

	public void SetProgress()
    {
		Debug.Log (_progress);
        if (_reachedGoal && _progress < Waypoints.Length)
        {
			_size = Camera.main.GetComponent<Camera> ().orthographicSize;
			_origin = Waypoints[_progress].waypoint.position;

            if (_progress < Waypoints.Length)
            {
				_target = Waypoints[_progress+1].waypoint.position;
				_targetZoom = Waypoints [_progress + 1].zoom;
                _move = 0;

                _reachedGoal = false;
            }
        }
    }

	void Update ()
	{
		if (_move >= 1 && !_reachedGoal)
		{
            PlaceObjectOnClick.Instance.SetDirty(false);
			_progress++;
            _reachedGoal = true;
		}

		if (!_reachedGoal && _move < 1 && Waypoints[_progress].zoomOut)
		{
			_move += Speed;

			if (_move >= 0.5)
			{
				_size = _targetZoom;
			}

			Camera.main.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size, ZoomOut, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move) ) + 1)*0.5f );

            Camera.main.transform.position = Vector3.Lerp (_origin + _offset, _target + _offset, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f);
		}

		if (!_reachedGoal && _move < 1 && !Waypoints[_progress].zoomOut)
		{
			_move += Speed;

			Camera.main.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size,_targetZoom, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f );

			Camera.main.transform.position = Vector3.Lerp (_origin + _offset, _target + _offset, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,0.5f*Mathf.PI,_move) ) + 1)*0.5f);
		}
	}
}
