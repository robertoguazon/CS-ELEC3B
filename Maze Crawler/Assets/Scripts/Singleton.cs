using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {

	public bool destroyOnLoad;
	protected static bool _destroyOnLoad;

	private static T instance;

	void Awake() {
		_destroyOnLoad = destroyOnLoad;
		FindObject();
		if (!destroyOnLoad) DontDestroyOnLoad(instance);
	}

	public static T Instance {
		get {
				FindObject();
				return instance;
			}
	}

	public static T FindObject() {
		T foundObject = FindObjectOfType<T>();

		if (instance == null) {
			instance = foundObject;
		} else if (instance != foundObject) {
			Destroy(foundObject);
		}
		return foundObject;
	}
}
