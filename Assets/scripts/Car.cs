using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	public float MotorTorque = 50;

	private WheelCollider[] _wheelColliders;

	void Start () {
		_wheelColliders = GetComponentsInChildren<WheelCollider>();
	}
	
	void FixedUpdate () {
		for(int collIndex=0; collIndex < _wheelColliders.Length; ++collIndex){
			_wheelColliders[collIndex].motorTorque = MotorTorque;
		}
	}
}
