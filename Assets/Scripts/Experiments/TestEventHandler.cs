using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;

public class TestEventHandler : MonoBehaviour {

	private ITestEvent interfaceTestEvent;

	void Awake () {

		var assembly = Assembly.GetExecutingAssembly();
		var types = assembly.GetTypes().Where(x => x.GetInterface("ITestEvent") != null);
		foreach (var type in types) {
			
			if(type.IsInstanceOfType(type)){
                #if UNITY_EDITOR
                    Debug.Log(type.Name);
				#endif
                var method = type.GetMethod("OnTouchTest");
				method.Invoke(type, null);

			}

		}


	
	}
}
