using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CHButtonSound :  MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler {

	public AudioClip downKey;

	[Range (-3f,3f)]
	public float downPitch;

	[Range (0f,1f)]
	public float downVolumen;


	public AudioClip upKey;

	[Range (-3f,3f)]
	public float upPitch;

	[Range (0f,1f)]
	public float upVolumen;

	private AudioSource audioSourceDown;
	private AudioSource audioSourceUp;

	void Awake(){

		audioSourceDown = gameObject.AddComponent<AudioSource>();
		audioSourceDown.clip = downKey;
		audioSourceDown.pitch = downPitch;
		audioSourceDown.volume = downVolumen;
		//audioSourceDown.time = 0.003f; //TODO: invalid seek position

		audioSourceUp = gameObject.AddComponent<AudioSource>();
		audioSourceUp.clip = upKey;
		audioSourceUp.pitch = upPitch;
		//audioSourceDown.time = 0.05f; //TODO: invalid seek position
		audioSourceUp.volume = upVolumen;
	}

	public void OnPointerEnter (PointerEventData eventData){
		audioSourceDown.Play();

	}
	public void OnPointerDown (PointerEventData eventData){
		audioSourceDown.Play();
	}

	public void OnPointerUp (PointerEventData eventData){
		audioSourceUp.Play();
	}
	public void OnPointerExit (PointerEventData eventData){
		audioSourceUp.Play();
	}

}
