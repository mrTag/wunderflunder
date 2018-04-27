using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPan : MonoBehaviour {

	public int PanMouseButton = 1;
	public float MovementToPanFactor = 1;

	private Transform _cameraTransform;
	private bool _panning;
	private Vector3 _panStartScreenPos;
	private Vector3 _panStartWorldPos;

	void Awake () {
		_cameraTransform = transform.GetChild(0);
		_panning = false;
	}
	
	void Update () {
		if(!_panning){
			if(Input.GetMouseButton(PanMouseButton)) {
				_panning = true;
				_panStartScreenPos = Input.mousePosition;
				_panStartWorldPos = transform.position;
			}
		} else {
			if(!Input.GetMouseButton(PanMouseButton)){
				_panning = false;
			} else {
				Vector3 screenOffset = Input.mousePosition - _panStartScreenPos;
				Vector3 worldOffset = new Vector3(-screenOffset.x, 0, -screenOffset.y);
				_cameraTransform.TransformDirection(worldOffset);
				worldOffset.y = 0;
				worldOffset.Normalize();
				worldOffset *= screenOffset.magnitude * MovementToPanFactor;
				transform.position = _panStartWorldPos + worldOffset;
			}
		}
	}
}
