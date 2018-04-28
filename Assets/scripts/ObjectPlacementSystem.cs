using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPlacementSystem : MonoBehaviour {

	public float RotationSpeed = 80.0f;

	private PlacementObject _currentlyActiveObject;
	private Camera _cam;
	private RaycastHit _lastHit;
	private bool _currentlyDragging;
	private bool _disabled = false;
	private List<PlacementObject> _placedObjects = new List<PlacementObject>();

	public System.Action OnObjectPlaced = delegate {};
	
	void Update () {
		if(_disabled) return;
        if (Input.GetKeyDown(KeyCode.F2)) {
            EndPlacement();
        }
        if (_currentlyActiveObject != null){
			if(Input.GetAxisRaw("Rotate") != 0){
				_currentlyActiveObject.transform.Rotate(Vector3.up, RotationSpeed* Time.unscaledDeltaTime * Input.GetAxisRaw("Rotate"));
			}
			if(updateRaycastHit()){
				_currentlyActiveObject.gameObject.SetActive(true);
				_currentlyActiveObject.transform.position = _lastHit.point;
				if(_currentlyDragging){
					if(!Input.GetMouseButton(0)){
						_currentlyDragging = false;
						_currentlyActiveObject.SetInPlacedState();
						_currentlyActiveObject = null;
					}
				} else {
					if(Input.GetMouseButtonDown(0)) {
						var placedObject = Instantiate(_currentlyActiveObject);
						placedObject.SetInPlacedState();
						_placedObjects.Add(placedObject);
						EndPlacement();
						OnObjectPlaced();
					}
				}
			} else {
				_currentlyActiveObject.gameObject.SetActive(false);
			}
		} else {
			if(Input.GetMouseButtonDown(0) && updateRaycastHit()) {
				var placedObject = _lastHit.collider.GetComponent<PlacementObject>();
				if(placedObject != null){
					_currentlyActiveObject = placedObject;
					_currentlyActiveObject.SetInPlacementState();
					_currentlyDragging = true;
				}
			}
		}
	}

    public void StartPlacement(GameObject prefab){
        if (_currentlyActiveObject != null){
            Destroy(_currentlyActiveObject.gameObject);
        }
        var newObject = Instantiate(prefab);
        _currentlyActiveObject = newObject.AddComponent<PlacementObject>();
        _currentlyActiveObject.CreatedFromPrefab = prefab;
        _currentlyActiveObject.SetInPlacementState();
    }

    public void EndPlacement()
    {
        if (_currentlyActiveObject != null)
        {
            Destroy(_currentlyActiveObject.gameObject);
            _currentlyActiveObject = null;
        }
    }

    bool updateRaycastHit() {
		if(!_cam) _cam = Camera.main;

		Ray screenRay = _cam.ScreenPointToRay(Input.mousePosition);
		return Physics.Raycast(screenRay, out _lastHit);
	}

	public void SavePlacedObjectState() {
		foreach(var placedObject in _placedObjects){
			placedObject.CreationPosition = placedObject.transform.position;
			placedObject.CreationOrientation = placedObject.transform.rotation;
		}
	}

	public void RestorePlacedObjectState() {
		var oldObjectList = _placedObjects;
		_placedObjects = new List<PlacementObject>();
		foreach(var placedObject in oldObjectList){
			var newPlacedObject = Instantiate(placedObject.CreatedFromPrefab,
				placedObject.CreationPosition, placedObject.CreationOrientation);
			var newPlacementObj = newPlacedObject.AddComponent<PlacementObject>();
			newPlacementObj.CreatedFromPrefab = placedObject.CreatedFromPrefab;
			newPlacementObj.SetInPlacedState();
			_placedObjects.Add(newPlacementObj);
			Destroy(placedObject.gameObject);
		}
	}

	public void ClearPlacedObjects() {
		foreach(var placedObject in _placedObjects){
			Destroy(placedObject.gameObject);			
		}
		_placedObjects.Clear();
	}

	public void DisableObjectPlacement() {
        if (_currentlyActiveObject != null) {
            Destroy(_currentlyActiveObject.gameObject);
            _currentlyActiveObject = null;
        }
		_disabled = true;
	}

	public void EnableObjectPlacement() {
		_disabled = false;
	}
}
