using UnityEngine;
using System.Collections;

public class TogglePanel : MonoBehaviour {

	public void DisableEnablePanel(GameObject panel){
		panel.SetActive(!panel.activeSelf);
	}
}
