using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour {

	public RectTransform ObjectListParent;
	public GameObject HideWhilePlaying;
	public ListObject ObjectPrefab;
	private List<ListObject> _listObjects = new List<ListObject>();

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
	
	public void SetPlayingState(){
		HideWhilePlaying.SetActive(false);
	}

	public void SetPlacingState(){
		HideWhilePlaying.SetActive(true);
	}
}
