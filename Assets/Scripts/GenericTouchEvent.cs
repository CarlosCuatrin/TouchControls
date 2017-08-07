using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericTouchEvent : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,IBeginDragHandler, IDragHandler, IEndDragHandler  
{
    //Touch Button Modes
	public enum TouchMode { ModeA, ModeB, ModeC, ModeD}
	[Tooltip("ModeA: Here a description of what the ModeA does. \n ModeB: Here a description of what the ModeB does. ")]
	public TouchMode touchMode;

    //Event Handlers
	public delegate void InputActionEventHandler(object sender, InputActionEventArgs args);
	public static event InputActionEventHandler Jump, RunLeft, RunRight, Fire, ChangeWeapon, Weapon1, Weapon2, Weapon3, Weapon4, NextWeapon, PreviousWeapon, Hook;

    //Input Actions (each time the player press a button it wants to execute a InputActions
	public enum InputAction {Jump, RunLeft, RunRight, Fire, ChangeWeapon, Weapon1, Weapon2, Weapon3, Weapon4, NextWeapon, PreviousWeapon, Hook}
	public InputAction inputAction;


	public enum ButtonStatus{Off, On, Dragging}
	[Space(10)]
	public ButtonStatus buttonStatus;

    public InputActionEventArgs inputActionEventArgs = new InputActionEventArgs();

	private enum TouchEvent {PointerDown, PointerUp, PointerEnter, PointerExit, BeginDrag, Drag, EndDrag } 

    [Space(10)]
    [Header("On Feedback")]
    public AudioClip audioClipOn;
    [Range (-3f,3f)]
    public float pitchOn = 1.4f;
    [Range (0f,1f)]
    public float volumeOn = 0.03f;
    [Range (0f,50f)]
    public long vibrationOn = 10;
  
    [Space(10)]
    [Header("Off Feedback")]
    public AudioClip audioClipOff;
    [Range (-3f,3f)]
    public float pitchOff = 2f;
    [Range (0f,1f)]
    public float volumeOff = 0.015f;
    [Range (0f,50f)]
    public long vibrationOff = 5;

    private AudioSource audioSourceOn;
    private AudioSource audioSourceOff;
    private Image buttonImage;
	private bool activatedByDragginIn = false;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        audioSourceOn = gameObject.AddComponent<AudioSource>();
        audioSourceOn.clip = audioClipOn;
        audioSourceOn.pitch = pitchOn;
        audioSourceOn.volume = volumeOn;
        //audioSourceDown.time = 0.003f; //TODO: invalid seek position

        audioSourceOff = gameObject.AddComponent<AudioSource>();
        audioSourceOff.clip = audioClipOff;
        audioSourceOff.pitch = pitchOff;
        //audioSourceDown.time = 0.05f; //TODO: invalid seek position
        audioSourceOff.volume = volumeOff;
    }


//Start, Update or OnGOU Makes the trick to show the Enable/disable checkbox in the inspector, that this scrip use to disable Events (events doesn't stop working with enable = off by default).
	void Start()
	{	
	}
		
//EVENTS//////////////////////////////////////////////////////////////////////////////////////////
	public void OnPointerDown (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. In theory By default unity doesn disabled methods other than Start, Update and OnGUI 
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.PointerDown, eventData);

	}

	public void OnPointerUp (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.PointerUp, eventData);
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.PointerEnter, eventData);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.PointerExit, eventData);
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.BeginDrag, eventData);
	}

	public void OnDrag (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.Drag, eventData);
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		//if(enabled) is used to turn off this event in the case the script is disabled in the inspector. By default unity doesn disabled methods other than Start, Update and OnGUI
		//TODO(Sapo): Research in wich conditions enabled doesn't turn off functionalities. Watch Lynda tutorial of 2D platform.
		if(enabled) SwitchBetweenModes(touchMode, TouchEvent.EndDrag, eventData);
	}


//METHODS//////////////////////////////////////////////////////////////////////////////////////////
	void SwitchBetweenModes( TouchMode Mode, TouchEvent touchEvent, PointerEventData eventData)
	{
		switch (Mode) 
		{
			case TouchMode.ModeA: 	ModeA(touchEvent, eventData); break;
			case TouchMode.ModeB: 	ModeB(touchEvent, eventData); break;
			case TouchMode.ModeC: 	ModeC(touchEvent, eventData); break;
			case TouchMode.ModeD: 	ModeD(touchEvent, eventData); break;
			default: 
				Debug.LogError("TouchMode Enum has returned a velue greater than what is considering the Switch statement. Please check the TouchMode declaration or the switch cases");
				/*If TouchMode enum list gets bigger, default should cover that case with an error*/
				break; 
		}
	}

//TOUCH MODES LOGIC //////////////////////////////////////////////////////////////////////////////////////////

	void ModeA(TouchEvent touchEvent, PointerEventData eventData)
	{
		//Description of Mode: Simple Mechanical Button. If you dragin activates it. If you dragout desactivates it.
		switch (touchEvent) 
		{
			//On
            case TouchEvent.PointerDown:
				//Pass through
            case TouchEvent.PointerEnter:
                if (buttonStatus != ButtonStatus.On)
                {
                    buttonStatus = ButtonStatus.On;
                    inputActionEventArgs.Activation = true;
                    FeedbackOn();
                    ExecuteInputAction (inputAction, inputActionEventArgs);
                }
				
                break;
			//Off
            case TouchEvent.PointerUp:
				//Pass through
            case TouchEvent.PointerExit:
                if (buttonStatus != ButtonStatus.Off)
                {
                    buttonStatus = ButtonStatus.Off;
                    inputActionEventArgs.Activation = false;
                    FeedbackOff();
                    ExecuteInputAction(inputAction, inputActionEventArgs);
                }				
                break;
			default: 
				break;
		}
		
	}

	void ModeB(TouchEvent touchEvent, PointerEventData eventData)
	{
		//Description of Mode: Pass through. If you drag out, you don desactivate it. And if you dragin into another button you can activate that button.
		switch (touchEvent) 
		{

			//DraggIn Case
			case TouchEvent.PointerEnter:
				
				if (buttonStatus == ButtonStatus.Off)
				{
					activatedByDragginIn = true;

					buttonStatus = ButtonStatus.On;
					inputActionEventArgs.Activation = true;
					FeedbackOn();
					ExecuteInputAction (inputAction, inputActionEventArgs);

				}

				break;

			//DraggOut Case
			case TouchEvent.PointerExit:
				if (activatedByDragginIn)
				{
					buttonStatus = ButtonStatus.Off;
					inputActionEventArgs.Activation = false;
					FeedbackOff();
					ExecuteInputAction(inputAction, inputActionEventArgs);

					activatedByDragginIn = false;
					
				}
				break;
			//On
			case TouchEvent.PointerDown:

				buttonStatus = ButtonStatus.On;
				inputActionEventArgs.Activation = true;
				FeedbackOn();
				ExecuteInputAction (inputAction, inputActionEventArgs);
				activatedByDragginIn = false;
			
				break;
			//Off
			case TouchEvent.PointerUp:
				
				buttonStatus = ButtonStatus.Off;
				inputActionEventArgs.Activation = false;
				FeedbackOff();
				ExecuteInputAction(inputAction, inputActionEventArgs);

				activatedByDragginIn = false;
						
				break;
			default: 
				break;
		}
	}

	void ModeC(TouchEvent touchEvent, PointerEventData eventData)
	{
		//Description of Mode: 
		switch (touchEvent) 
		{
			case TouchEvent.PointerDown:

				DebugString.text += "Pointer ID: " + eventData.pointerId.ToString() + "\n";

				for (int i = 0; i < Input.touches.Length; i++)
				{
					DebugString.text +=  "Finger ID: i=" + i.ToString() + " id=" + Input.touches[i].fingerId.ToString() + "\n";

					if (Input.touches[i].fingerId == eventData.pointerId)
					{
						DebugString.text +=  "FingerId y Pointer ID Radious =" + Input.touches[i].radius.ToString() + "\n"; 
					}
				}

				break;
			case TouchEvent.Drag:
				break;
			case TouchEvent.PointerUp:
				break;
			default: 
				break;
		}
	}

	void ModeD(TouchEvent touchEvent, PointerEventData eventData)
	{
		//Description of Mode: 
		switch (touchEvent) 
		{
			case TouchEvent.PointerDown:

			case TouchEvent.PointerUp:

			default: 
				break;
		}
	}

//METHODS //////////////////////////////////////////////////////////////////////////////////////////
    void ExecuteInputAction(InputAction inputAction, InputActionEventArgs args)
	{
		switch (inputAction)
		{
			case InputAction.Jump:
				if (Jump != null)
				{
                    Jump(this, args);
				}
				break;
			case InputAction.RunLeft:
				if (RunLeft != null)
				{
                    RunLeft(this, args);
				}
				break;
			case InputAction.RunRight:
				if (RunRight != null)
				{
                    RunRight(this, args);
				}
				break;
			case InputAction.Fire:
				if (Fire != null)
				{
                    Fire(this, args);
				}
				break;
			case InputAction.Hook:
				if (Hook != null)
				{
                    Hook(this, args);
				}
				break;
			case InputAction.ChangeWeapon:
				if (ChangeWeapon != null)
				{
                    ChangeWeapon(this, args);
				}
				break;
			case InputAction.NextWeapon:
				if (NextWeapon != null)
				{
                    NextWeapon(this, args);
				}
				break;
			case InputAction.PreviousWeapon:
				if (PreviousWeapon != null)
				{
                    PreviousWeapon(this, args);
				}
				break;
			case InputAction.Weapon1:
				if (Weapon1 != null)
				{
                    Weapon1(this, args);
				}
				break;
			case InputAction.Weapon2:
				if (Weapon2 != null)
				{
                    Weapon2(this, args);
				}
				break;
			case InputAction.Weapon3:
				if (Weapon3 != null)
				{
                    Weapon3(this, args);
				}
				break;
			case InputAction.Weapon4:
				if (Weapon4 != null)
				{
                    Weapon4(this, args);
				}
				break;
			default:
				break;
		}
	}

    void FeedbackOn()
    {
        audioSourceOn.Play();
        Vibration.Vibrate(vibrationOn);
        buttonImage.color = Color.red;
    }

    void FeedbackOff()
    {
        audioSourceOff.Play();
        Vibration.Vibrate(vibrationOff);
        buttonImage.color = Color.white;
    }


}

    




//CLASSES//////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// WARNING: This data could change if another InputAction Event is Raised.
/// </summary>
public class InputActionEventArgs: EventArgs
{
    public bool Activation;


}

