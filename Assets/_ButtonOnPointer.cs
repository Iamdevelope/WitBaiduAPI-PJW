using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _ButtonOnPointer : MonoBehaviour,IPointerDownHandler,IPointerUpHandler {
	public bool isDown;
	public void OnPointerDown(PointerEventData data){
		isDown=true;
	}
	public void OnPointerUp(PointerEventData data){
		isDown=false;
	}
}
