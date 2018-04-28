using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public UnityEngine.UI.Image ObjectImage;
	public UnityEngine.UI.Text CountText;

	public System.Action OnObjectClicked = delegate {};

	public void SetCount(int count){
		CountText.text = count.ToString();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		OnObjectClicked();
    }

    public void OnPointerDown(PointerEventData eventData){}
    public void OnPointerUp(PointerEventData eventData){}
}
