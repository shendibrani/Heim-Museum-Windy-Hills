using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField] CameraPosition[] Waypoints;
	[SerializeField] float Margin = 0.1f;
	[SerializeField] [Range(0,1)] float Speed = 0.5f;
	[SerializeField] float ZoomOut = 0;

	Vector3 _offset;
	bool _reachedGoal = true;
	Vector3 _origin;
	Vector3 _target;
	int _progress = 0;
	float _move = 0;
	float _size = 0;
    Camera _main;

    [System.Serializable]
    struct CameraPosition
    {
        public Transform waypoint;
        public float zoom;
    }

	void Start ()
	{
        _main = Camera.main;
		_offset = _main.transform.position - Waypoints [_progress].waypoint.position;
		_size = _main.GetComponent<Camera> ().orthographicSize;

    }

    public void SetProgress()
    {
        if (_reachedGoal && _progress < Waypoints.Length)
        {
            _origin = Waypoints[_progress].waypoint.position;

            _progress++;
            if (_progress < Waypoints.Length)
            {
                _target = Waypoints[_progress].waypoint.position;
                _move = 0;

                _reachedGoal = false;
            }
            else
            {
                GetComponent<TutorialProgression>().SetComplete();
            }
        }
    }

	void Update ()
	{
		if (!_reachedGoal && Vector3.Distance(_main.transform.position,_target + _offset) < Margin)
		{
            PlaceObjectOnClick.Instance.SetDirty(false);
            _reachedGoal = true;
		}

		if (!_reachedGoal && _move < 1)
		{
			_move += Speed;

			_main.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size, ZoomOut, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move) ) + 1)*0.5f );

            _main.transform.position = Vector3.Lerp (_origin + _offset, _target + _offset, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move/2) ) + 1)*0.5f);
		}
	}
}
