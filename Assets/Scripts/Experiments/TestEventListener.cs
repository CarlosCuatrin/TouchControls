using UnityEngine;
using System.Collections;

public class TestEventListener : MonoBehaviour, ITestEvent {



	public void OnTouchTest ()
	{
        #if UNITY_EDITOR
            Debug.Log("EVENT LISTENER WORKS!!");
        #endif
	}

}
