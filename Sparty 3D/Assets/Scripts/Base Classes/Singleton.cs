using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {

	public bool destroyOnLoad;
	protected static bool _destroyOnLoad;

	void Awake() {
		_destroyOnLoad = destroyOnLoad;
		if (!_destroyOnLoad) {
			DontDestroyOnLoad(gameObject);
		}
	}

	private static T instance;

	public static T Instance {
		get {
				CheckInstance();

				return instance;
			}
	}

	static void CheckInstance() {
		T foundObject = FindObjectOfType<T>();

		if (instance == null) {
			instance = foundObject;
		} else if (instance != foundObject) {
			Destroy(foundObject);
		}
	}
}
