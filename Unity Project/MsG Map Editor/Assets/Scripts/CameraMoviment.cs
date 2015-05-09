using UnityEngine;
using System.Collections.Generic;

public class CameraMoviment : MonoBehaviour 
{
	#region Inspector
	public float speed = 5;
	#endregion

	#region private data
	private Transform _transform;
	private bool _lockUpdate = false;
	#endregion

	void Start()
	{
		_transform = this.transform;
	}

	void OnApplicationFocus(bool p_focused) 
	{
		_lockUpdate = !p_focused;
	}

	void Update () 
	{
		if(_lockUpdate)
			return;

		var __horizontal = Input.GetAxis("Horizontal");
		var __vertical = Input.GetAxis("Vertical");
	

		Vector3 __newPosition = new Vector3(_transform.position.x + (speed * __horizontal * Time.deltaTime), 
		                                    _transform.position.y,
		                                    _transform.position.z + (speed * __vertical * Time.deltaTime));
		_transform.position = __newPosition;

	}
}
