using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldatChangeWeaponButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler {

	public Image buttonUp;
	public Image buttonDown;
	public SoldatInputClient inputClient;


	void Update(){
		//this if to show enable checkbox on the inspector
	}

	public void OnPointerDown(PointerEventData data) {
		if(enabled){
			inputClient.KeyDown(SoldatInputClient.Key.E);
			Vibration.Vibrate(5);
			buttonDown.enabled = true;
		}
	}
	public void OnPointerEnter (PointerEventData eventData){
		if(enabled){
			inputClient.KeyDown(SoldatInputClient.Key.E);
			Vibration.Vibrate(5);
			buttonDown.enabled = true;
		}
	}
	
	public void OnPointerUp (PointerEventData eventData){
		if(enabled){
			inputClient.KeyUp(SoldatInputClient.Key.E);
			Vibration.Vibrate(5);
			buttonDown.enabled = false;
		}
	}	
	
	public void OnPointerExit (PointerEventData eventData){
		if(enabled){
			inputClient.KeyUp(SoldatInputClient.Key.E);
			Vibration.Vibrate(5);
			buttonDown.enabled = false;
		}
	}			
	
	public void OnDrag (PointerEventData eventData)
	{
		if(enabled){
			//...
		}
	}
}
