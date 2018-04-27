using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour {

	public GameObject CreatedFromPrefab;
	public Vector3 CreationPosition {get;set;}
	public Quaternion CreationOrientation {get;set;}

	public void SetInPlacementState() {
		foreach(var collider in GetComponentsInChildren<Collider>()){
			collider.enabled = false;
		}
		var rb = GetComponentInChildren<Rigidbody>();
		if(rb){
			rb.isKinematic = true;
		}
	}

	public void SetInPlacedState() {
        foreach (var collider in GetComponentsInChildren<Collider>()){
            collider.enabled = true;
        }
        var rb = GetComponentInChildren<Rigidbody>();
        if (rb){
            rb.isKinematic = false;
        }
	}

}
