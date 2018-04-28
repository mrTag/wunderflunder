using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour {

	public float RotationSpeed;

	void Update()
	{
		transform.Rotate(0, RotationSpeed * Time.unscaledDeltaTime, 0);
	}

}
