using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldatRightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler  {

	public Image buttonUp;
	public Image buttonDown;
	public SoldatInputClient inputClient;


	void Update(){
		//this if to show enable checkbox on the inspector
	}

	public void OnPointerDown(PointerEventData data) 
	{
		if(enabled){
			inputClient.KeyDown(SoldatInputClient.Key.D);
			Vibration.Vibrate(10);
			buttonDown.enabled = true;
		}
	}
	public void OnPointerEnter (PointerEventData eventData)
	{
		if(enabled){
			inputClient.KeyDown(SoldatInputClient.Key.D);
			Vibration.Vibrate(10);
			buttonDown.enabled = true;
		}
	}
	
	public void OnPointerUp (PointerEventData eventData)
	{
		if(enabled){
			inputClient.KeyUp(SoldatInputClient.Key.D);
			Vibration.Vibrate(10);
			buttonDown.enabled = false;
		}
	}	
	
	public void OnPointerExit (PointerEventData eventData)
	{
		if(enabled){
			inputClient.KeyUp(SoldatInputClient.Key.D);
			Vibration.Vibrate(10);
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
