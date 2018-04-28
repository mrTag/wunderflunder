using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbieColl : MonoBehaviour {

	public System.Action<Collision> OnColl = delegate{};

	void OnCollisionEnter(Collision other)
	{
		OnColl(other);	
	}

}
