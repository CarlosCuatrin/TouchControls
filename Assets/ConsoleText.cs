using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleText : MonoBehaviour {

	public Text text;

	// Update is called once per frame
	void Update() 
	{
		if (DebugString.text != "")
		{
			text.text = DebugString.text;

			DebugString.text = "";

		}

		if (Input.touches.Length == 1 && Input.touches[0].radius > 0 )
		{
			text.text += "Radius > 0 ";
		}
	}
}
