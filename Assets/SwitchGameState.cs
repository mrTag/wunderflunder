using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGameState : MonoBehaviour {

	public void SetGameState_Playing() {
		GameManager.Instance.LoadLevel(0);
	}
}
