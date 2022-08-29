using UnityEngine;

namespace Murgn.Utils
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance = null;

		public static T instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (_instance == null)
					{
						var singletonObj = new GameObject();
						singletonObj.name = typeof(T).ToString();
						_instance = singletonObj.AddComponent<T>();
					}
				}

				return _instance;
			}
		}

		public virtual void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}