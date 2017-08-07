using UnityEngine;
using System.Collections;

public class DebuggingKeyControlServer : MonoBehaviour {

	public SoldatInputClient inputClient; 
	public SoldatInputClient.Key key = SoldatInputClient.Key.B;
	public int delay = 3000;
	public enum KeyAction{Press, Up, Down};
	public KeyAction keyAction;

	public void PressKey(){

		switch (keyAction) {

			case KeyAction.Down:
				if(delay > 0 )
					inputClient.KeyDown(key, delay); 
				else 
					inputClient.KeyDown(key);
				break;
			case KeyAction.Up:
				if(delay > 0 ) 
					inputClient.KeyUp(key, delay); 
				else 
					inputClient.KeyUp(key);
				break;
			case KeyAction.Press:
				if(delay > 0 ) 
					inputClient.KeyPress(key, delay);
				else
				inputClient.KeyPress(key);
				break;
			default:
				break;
		}

	}
}
