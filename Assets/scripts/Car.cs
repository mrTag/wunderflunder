using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	public enum DriverType {
		Babsi,
		Bob
	}
	
	public DriverType Driver;
	public Transform Spawnpoint;
	public GameObject BabsiPrefab;
	public GameObject BobPrefab;
	public float MotorTorque = 50;
	public GameObject ChassisBob;
	public GameObject ChassisBabsi;

	private WheelCollider[] _wheelColliders;

	void Start () {
		_wheelColliders = GetComponentsInChildren<WheelCollider>();
		switch (Driver) {
			case DriverType.Babsi:
				GameObject Babsi = Instantiate(BabsiPrefab, Spawnpoint.transform.position, Spawnpoint.transform.rotation);
				ChassisBob.SetActive(false);
				ChassisBabsi.SetActive(true);
			break;
			case DriverType.Bob:				
				GameObject Bob = Instantiate(BobPrefab, Spawnpoint.transform.position, Spawnpoint.transform.rotation);

				ChassisBob.SetActive(true);
				ChassisBabsi.SetActive(false);
			break;
		}
	}
	
	void FixedUpdate () {
		for(int collIndex=0; collIndex < _wheelColliders.Length; ++collIndex){
			_wheelColliders[collIndex].motorTorque = MotorTorque;
		}
	}
}
