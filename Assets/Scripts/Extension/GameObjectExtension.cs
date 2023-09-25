using UnityEngine;

public static class GameObjectExtension
{
	public static T GetOrAddComponent<T>(this GameObject gameObject)
		where T : Component
	{
		if (!gameObject.TryGetComponent(out T component))
		{
			component = gameObject.AddComponent<T>();
		}

		return component;
	}

	public static GameObject FindChild(this GameObject gameObject, string name = null, bool recursive = true)
	{
		if (gameObject.FindChild<Transform>(name, recursive) is Transform transform)
		{
			return transform.gameObject;
		}

		return null;
	}

	public static T FindChild<T>(this GameObject gameObject, string name = null, bool recursive = true)
		where T : Object
	{
		if (gameObject == null)
		{
			return null;
		}

		if (recursive)
		{
			foreach (T component in gameObject.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
				{
					return component;
				}
			}
		}
		else
		{
			for (int index = 0; index < gameObject.transform.childCount; index++)
			{
				Transform transform = gameObject.transform.GetChild(index);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					if (transform.TryGetComponent(out T component))
					{
						return component;
					}
				}
			}
		}

		return null;
	}
}