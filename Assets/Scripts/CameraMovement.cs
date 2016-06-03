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
    bool start = true;

	TutorialProgression tutProg;

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
		tutProg = this.GetComponent<TutorialProgression> ();
    }

	public void SetProgress()
    {
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
				tutProg.setCamera (false);
            }
        }
    }

	void Update ()
	{
		if (_move >= 1 && !_reachedGoal)
		{
			if (_progress == 2)
			{
				ZoomOut = 90;
			}
			_progress++;
            _reachedGoal = true;
			tutProg.setCamera (true);
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

	public void NewWaypoint (int pProg, Transform pTrans, int pZoom, bool pZoomout)
	{
		Waypoints [pProg].waypoint = pTrans;
		Waypoints [pProg].zoom = pZoom;
		Waypoints [pProg].zoomOut = pZoomout;
	}
}
