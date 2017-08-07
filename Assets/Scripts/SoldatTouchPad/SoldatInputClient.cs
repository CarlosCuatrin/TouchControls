using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class SoldatInputClient : MonoBehaviour
{
	public string Host = "192.168.0.5";
	public int Port = 81;
	public Image wifi;
	public InputField ipNumber;
	public InputField portNumber;

	private WWW[] requests = new WWW[10]; //to avoid app freeze for creating a lot of WWW objects
	private int requestIndex = 0;
	private int waitForMouseMoveEnd = 0;

	void Update(){
		if(requests[0]!= null && requests[0].isDone){
			wifi.enabled = !wifi.enabled;
			wifi.color = Color.white;
		}
		
	}

	public enum MouseButton
	{
		Left = 0,
		Right = 1,
		Middle = 2
	};

	public enum Key
	{
		Backspace =         0x08, Tab =               0x09, Clear =             0x0C, Return =            0x0D,
		Shift =             0x10, Control =           0x11, Alt =               0x12, Pause =             0x13,
		CapsLock =          0x14, Escape =            0x1B, Space =             0x20, PageUp =            0x21,
		PageDown =          0x22, End =               0x23, Home =              0x24, Left =              0x25,
		Up =                0x26, Right =             0x27, Down =              0x28, Select =            0x29,
		Print =             0x2A, Execute =           0x2B, PrintScreen =       0x2C, Insert =            0x2D,
		Delete =            0x2E, Help =              0x2F, Digit0 =            0x30, Digit1 =            0x31,
		Digit2 =            0x32, Digit3 =            0x33, Digit4 =            0x34, Digit5 =            0x35,
		Digit6 =            0x36, Digit7 =            0x37, Digit8 =            0x38, Digit9 =            0x39,
		A =                 0x41, B =                 0x42, C =                 0x43, D =                 0x44,
		E =                 0x45, F =                 0x46, G =                 0x47, H =                 0x48,
		I =                 0x49, J =                 0x4A, K =                 0x4B, L =                 0x4C,
		M =                 0x4D, N =                 0x4E, O =                 0x4F, P =                 0x50,
		Q =                 0x51, R =                 0x52, S =                 0x53, T =                 0x54,
		U =                 0x55, V =                 0x56, W =                 0x57, X =                 0x58,
		Y =                 0x59, Z =                 0x5A, LWin =              0x5B, RWin =              0x5C,
		Apps =              0x5D, Sleep =             0x5F, Numpad0 =           0x60, Numpad1 =           0x61,
		Numpad2 =           0x62, Numpad3 =           0x63, Numpad4 =           0x64, Numpad5 =           0x65,
		Numpad6 =           0x66, Numpad7 =           0x67, Numpad8 =           0x68, Numpad9 =           0x69,
		Multiply =          0x6A, Add =               0x6B, Separator =         0x6C, Subtract =          0x6D,
		Decimal =           0x6E, Divide =            0x6F, F1 =                0x70, F2 =                0x71,
		F3 =                0x72, F4 =                0x73, F5 =                0x74, F6 =                0x75,
		F7 =                0x76, F8 =                0x77, F9 =                0x78, F10 =               0x79,
		F11 =               0x7A, F12 =               0x7B, F13 =               0x7C, F14 =               0x7D,
		F15 =               0x7E, F16 =               0x7F, F17 =               0x80, F18 =               0x81,
		F19 =               0x82, F20 =               0x83, F21 =               0x84, F22 =               0x85,
		F23 =               0x86, F24 =               0x87, NumLock =           0x90, Scroll =            0x91,
		LShift =            0xA0, RShift =            0xA1, LControl =          0xA2, RControl =          0xA3,
		LAlt =              0xA4, RAlt =              0xA5, BrowserBack =       0xA6, BrowserForward =    0xA7,
		BrowserRefresh =    0xA8, BrowserStop =       0xA9, BrowserSearch =     0xAA, BrowserFavorites =  0xAB,
		BrowserHome =       0xAC, VolumeMute =        0xAD, VolumeDown =        0xAE, VolumeUp =          0xAF,
		MediaNextTrack =    0xB0, MediaPrevTrack =    0xB1, MediaStop =         0xB2, MediaPlayPause =    0xB3,
		LaunchMail =        0xB4, LaunchMediaSelect = 0xB5, LaunchApp1 =        0xB6, LaunchApp2 =        0xB7,
		Semicolon =         0xBA, Plus =              0xBB, Comma =             0xBC, Minus =             0xBD,
		Period =            0xBE, Question =          0xBF, Tilde =             0xC0, OpenBrackets =      0xDB,
		Pipe =              0xDC, CloseBrackets =     0xDD, Quotes =            0xDE, Oem8 =              0xDF,
		Backslash =         0xE2, ProcessKey =        0xE5, Attn =              0xF6, CrSel =             0xF7,
		ExSel =             0xF8, EraseEOF =          0xF9, Play =              0xFA, Zoom =              0xFB,
		Pa1 =               0xFD, OemClear =          0xFE
	};

	public void SendCommand(string cmd)
	{
		WWW req = requests[requestIndex];

		//to avoid app freeze for creating a lot of WWW objects
		if (req == null || req.isDone){
			requests[requestIndex] = new WWW("http://" + Host + ":" + Port + "/input" + cmd);
			wifi.color = Color.yellow;

		}
		
		requestIndex = (requestIndex + 1) % requests.Length;
	}

	public void MouseUp(MouseButton button) 		{ SendCommand("/mu/" + (int)button); 				}
	public void MouseDownA(MouseButton button) 		{ SendCommand("/md/" + (int)button); 				}
	public void MouseClick (MouseButton button) 	{ SendCommand("/mc/" + (int)button);   				}
	public void KeyDown    (Key key)            	{ SendCommand("/kd/" + (int)key);     				}
	public void KeyDown    (Key key, int msDelay)	{ SendCommand("/kd/" + (int)key + " +" + msDelay);	}
	public void KeyUp      (Key key)            	{ SendCommand("/ku/" + (int)key);					}
	public void KeyUp      (Key key, int msDelay)  	{ SendCommand("/ku/" + (int)key + " +" + msDelay);	}
	public void KeyPress   (Key key)            	{ SendCommand("/kp/" + (int)key);      				}
	public void KeyPress   (Key key, int msDelay)	{ SendCommand("/kp/" + (int)key + " +" + msDelay);	}

//COROUTINES-----------------------------------------------------------------------
	public IEnumerator MouseMove  (int dx, int dy)     { 
		if(waitForMouseMoveEnd == 0){
			SendCommand("/mm/" + dx + "," + dy); 
		}
		yield return null;
	}
	public IEnumerator MouseMove  (int dx, int dy, int frameDelay)     { 
		waitForMouseMoveEnd++;
		yield return new WaitForSeconds(frameDelay/60);
		waitForMouseMoveEnd--;
		SendCommand("/mm/" + dx + "," + dy); 
	}
	public IEnumerator MouseDown  (MouseButton button, int frameDelay) { 
		yield return new WaitForSeconds(frameDelay/60);
		SendCommand("/md/" + (int)button);  
	}
	public IEnumerator MouseDown  (MouseButton button) { SendCommand("/md/" + (int)button);  yield return null; }
	public IEnumerator MouseUpDelayed(MouseButton button, int frameDelay) { 
		yield return new WaitForSeconds(frameDelay/60);
		SendCommand("/mu/" + (int)button); 
	}
//------------------------------------------------------------------------------------

	public void SetIPNumber(){
		Host = ipNumber.text;
	}

	public void SetPortNumber(){
		int number;
		Port = int.TryParse(portNumber.text, out number) ? number : Port;
	}


}


