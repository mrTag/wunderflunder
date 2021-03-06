﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour {

	public RectTransform ObjectListParent;
	public GameObject HideWhilePlaying;
	public ListObject ObjectPrefab;
	private List<ListObject> _listObjects = new List<ListObject>();
	public GameObject EndScreen_NextLevel;
	public GameObject EndScreen_EndGame;
	public GameObject ControlPanel;
	public UnityEngine.UI.Button PlayButton;
    public UnityEngine.UI.Button StopButton;
    public UnityEngine.UI.Button PauseButton;

	public void ActivateElements(bool objectList, bool controlPanel, bool endScreenNext, bool endScreenEnd){
		HideWhilePlaying.gameObject.SetActive(objectList);
		ControlPanel.gameObject.SetActive(controlPanel);
		EndScreen_NextLevel.SetActive(endScreenNext);
		EndScreen_EndGame.SetActive(endScreenEnd);
	}

	public void AddObject(Sprite objectImage, int count, System.Action clickedAction) {
		var obj = Instantiate(ObjectPrefab);
		obj.transform.SetParent(ObjectListParent, false);
		obj.ObjectImage.sprite = objectImage;
		obj.SetCount(count);
		obj.OnObjectClicked += clickedAction;
		_listObjects.Add(obj);
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ObjectListParent);
	}

	public void ClearList() {
		foreach(var listobject in _listObjects){
			Destroy(listobject.gameObject);
		}
		_listObjects.Clear();
	}

	public void SetObjectCount(int objIndex, int count){
		_listObjects[objIndex].SetCount(count);
	}
}
