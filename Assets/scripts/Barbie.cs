using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbie : MonoBehaviour {

	public static System.Action<Vector3> OnBarbiesTouched = delegate {};

	void Awake() {
		foreach(var coll in GetComponentsInChildren<Collider>()){
			coll.gameObject.tag = "barbie";
			coll.gameObject.AddComponent<BarbieColl>().OnColl += OnCollisionEnter;
		}
	}

	void OnCollisionEnter(Collision coll){
		if(coll.gameObject.CompareTag("barbie")){
			var otherBarbie = coll.gameObject.GetComponentInParent<Barbie>();
			if(otherBarbie != this){
				OnBarbiesTouched(coll.contacts[0].point);
			}
		}
	}
	
}
