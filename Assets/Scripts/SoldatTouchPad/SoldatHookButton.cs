using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldatHookButton :  MonoBehaviour, IDragHandler, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler {

	public Image buttonUp;
	public Image buttonDown;
	public SoldatInputClient inputClient;

	void Update(){
		//this if to show enable checkbox on the inspector
	}

	public void OnPointerDown (PointerEventData eventData){

		if (enabled) {
			inputClient.MouseDown(SoldatInputClient.MouseButton.Right);
			Vibration.Vibrate(7);
			buttonDown.enabled = true;
		}
	}

	public void OnPointerEnter (PointerEventData eventData){

		if (enabled) {
			inputClient.MouseDown(SoldatInputClient.MouseButton.Right);
			Vibration.Vibrate(7);
			buttonDown.enabled = true;
		}
	}

	public void OnPointerUp (PointerEventData eventData){

		if (enabled) {
			inputClient.MouseUp(SoldatInputClient.MouseButton.Right);
			Vibration.Vibrate(7);
			buttonDown.enabled = false;
		}
	}

	public void OnPointerExit (PointerEventData eventData){

		if (enabled) {
			inputClient.MouseUp(SoldatInputClient.MouseButton.Right);
			Vibration.Vibrate(7);
			buttonDown.enabled = false;
		}
	}

	public void OnDrag (PointerEventData eventData){
		//...
	}

}
