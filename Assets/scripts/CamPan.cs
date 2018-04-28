using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPan : MonoBehaviour {

	public int PanMouseButton = 1;
	public float MovementToPanFactor = 1;

	private Transform _cameraTransform;
	private bool _panning;
	private bool _disabled = false;
	private Vector3 _panStartScreenPos;
	private Vector3 _panStartWorldPos;

	void Awake () {
		_cameraTransform = GetComponentInChildren<Camera>().transform;
		_panning = false;
		DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		if(_disabled) return;
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
				Vector3 worldOffset = new Vector3(screenOffset.x, 0, screenOffset.y);
				worldOffset = _cameraTransform.TransformDirection(worldOffset);
				worldOffset.y = 0;
				worldOffset.Normalize();
				worldOffset *= screenOffset.magnitude * MovementToPanFactor;
				transform.position = _panStartWorldPos - worldOffset;
			}
		}

		if(Input.mouseScrollDelta.y != 0){
			_cameraTransform.localPosition += new Vector3(0,0,Input.mouseScrollDelta.y * Time.unscaledDeltaTime * 100);
			if(_cameraTransform.localPosition.z < -40) {
				_cameraTransform.localPosition = new Vector3(_cameraTransform.localPosition.x, _cameraTransform.localPosition.y, -40);
			} else if(_cameraTransform.localPosition.z > -8) {
				_cameraTransform.localPosition = new Vector3(_cameraTransform.localPosition.x, _cameraTransform.localPosition.y, -8);
			}
		}
	}

	public void DisableCamPan() {
		_disabled = true;
		_panning = false;
	}

	public void EnableCamPan() {
		_disabled = false;
	}
}
