using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnim : MonoBehaviour {

	public GameObject Icon;
	public Shadow IconShadow;
	private Vector3 IconStartPos;

	void Awake () {
		IconStartPos = Icon.transform.localPosition;
	}

	public void ButtonPressed() {
		Icon.transform.DOKill();
		Icon.transform.localPosition= IconStartPos;
		Icon.transform.DOPunchPosition(new Vector3(5,-5,0),0.2f,2).OnComplete(ShowShadow).SetUpdate(true);
		IconShadow.enabled = false;
	}

	private void ShowShadow() {
		IconShadow.enabled = true;
	}
}
