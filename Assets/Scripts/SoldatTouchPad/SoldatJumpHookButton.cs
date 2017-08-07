using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldatJumpHookButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler {

	public Image buttonUp;
	public Image buttonDown;
	public SoldatInputClient inputClient;
	public float hookDragDistance = 50; //in pixels
	/// <summary>
	/// Amount of seconds after the hook will be released after a fast slide (in the hook or jump button).
	/// </summary>
	public float hookReleaseTime = 0.3f;


	private float sqrtHookDragDistance = 2500f; //50^2 pixels
	private Vector2 hookDrag;	//The amount of drag the player makes on the screen with his finger.
	private bool hooked = false;

	private AudioSource beep;


	void Start(){
		sqrtHookDragDistance = hookDragDistance * hookDragDistance; // hookDragDistance^2 = sqrtHookDragDistance
		beep = GetComponent<AudioSource>();
	}

	void Update(){
		//this if to show enable checkbox on the inspector
	}
	
	public void OnPointerEnter (PointerEventData eventData){
		if(enabled){												//why this? if the gameobject is enable? or the script?
			if(!eventData.dragging){
				inputClient.KeyDown(SoldatInputClient.Key.Space);
				Vibration.Vibrate(10);
				buttonDown.enabled = true;
			}
		}
	}

	public void OnPointerDown (PointerEventData eventData){
		if(enabled){
			if(!eventData.dragging){
				inputClient.KeyDown(SoldatInputClient.Key.Space);
				Vibration.Vibrate(10);
				buttonDown.enabled = true;
			}
		}
	}

	public void OnPointerUp (PointerEventData eventData){
		if(enabled){
			inputClient.KeyUp(SoldatInputClient.Key.Space);
			Vibration.Vibrate(10);
			buttonDown.enabled = false;
		}
	}
	public void OnPointerExit (PointerEventData eventData){
		if (enabled) {
			inputClient.KeyUp(SoldatInputClient.Key.Space);
			Vibration.Vibrate(10);
			buttonDown.enabled = false;
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		hookDrag = new Vector2(0f,0f);
	}


	public void OnDrag (PointerEventData eventData){

		//Update hookDrag amplitud
		hookDrag = new Vector2 (hookDrag.x + eventData.delta.x, hookDrag.y + eventData.delta.y);
		
		//If hookDrag is big enough to be consider a drag (sqrtHookDragDistance defines that)
		if (hookDrag.sqrMagnitude > sqrtHookDragDistance) {
			
			//shoot hook in the hookDrag angle
			inputClient.KeyPress(SoldatInputClient.Key.K); //K key is a running macro that place cursor at 0,0 on the screen in teeworld
			StartCoroutine(inputClient.MouseMove ((int)(hookDrag.x * 10), (int)(hookDrag.y * -10),1)); //Wait for 1/60 seconds and execute MouseMove in the correct angle
			StartCoroutine(inputClient.MouseDown(SoldatInputClient.MouseButton.Right,3)); // Wait for 3/60 seconds and execute MouseDown
			
			//activate flags to notify that you just shot the hook
			hooked = true;
		}
		
		
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if(hooked){	//it can be OnEndDrag without aver hooking because !(hookDrag.sqrMagnitude > sqrtHookDragDistance)
			StartCoroutine(
				UnHookDelayed(60)
			);

		}
	}


	/// <summary>
	/// Routine to Release hook (unpress mouse button and turn off flags after a delay)
	/// </summary>
	IEnumerator UnHookDelayed(int frameDelay){
		yield return new WaitForSeconds( frameDelay/60 );
		inputClient.MouseUp( SoldatInputClient.MouseButton.Right);
		hooked = false;
		beep.volume = 0.1f;
		beep.Play();


	}

}

