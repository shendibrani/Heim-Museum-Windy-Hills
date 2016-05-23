using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField] Transform[] Waypoints;
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

	void Start ()
	{
		_offset = this.transform.position - Waypoints [_progress].position;
		_size = GetComponent<Camera> ().orthographicSize;
	}

	void Update ()
	{
		if (_reachedGoal && Input.GetKeyDown(KeyCode.Space))
		{
			_origin = Waypoints[_progress].position;

			_progress++;
			_target = Waypoints [_progress].position;
			_move = 0;

			_reachedGoal = false;
		}

		if (!_reachedGoal && Vector3.Distance(this.transform.position,_target + _offset) < Margin)
		{
			_reachedGoal = true;
		}

		if (!_reachedGoal && _move < 1)
		{
			_move += Speed;

			this.GetComponent<Camera> ().orthographicSize = Mathf.Lerp (_size, ZoomOut, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move) ) + 1)*0.5f );

			this.transform.position = Vector3.Lerp (_origin + _offset, _target + _offset, (Mathf.Sin(Mathf.Lerp(-0.5f*Mathf.PI,1.5f*Mathf.PI,_move/2) ) + 1)*0.5f);
		}
	}
}
