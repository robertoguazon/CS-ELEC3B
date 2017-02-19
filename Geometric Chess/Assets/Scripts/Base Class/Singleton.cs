using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {

	protected static bool destroyOnLoad = false;

	private static T instance;

	public static T Instance {
		get {
				T foundObject = FindObjectOfType<T>();

				if (instance == null) {
					instance = foundObject;
				} else if (instance != foundObject) {
					Destroy(foundObject);
				}
				
				if (!destroyOnLoad) DontDestroyOnLoad(foundObject);

				return instance;
			}
	}
}
