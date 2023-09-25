using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
	private readonly Dictionary<Type, UnityEngine.Object[]> _objects = new();

	public void RegisterEvent(Action<PointerEventData> handler, UIEventType eventType = UIEventType.Click)
	{
		var eventHandlerComponent = gameObject.GetOrAddComponent<UIEventHandler>();
		switch (eventType)
		{
			case UIEventType.Click:
				eventHandlerComponent.Clicked -= handler;
				eventHandlerComponent.Clicked += handler;
				break;
			case UIEventType.PointerEnter:
				eventHandlerComponent.PointerEntered -= handler;
				eventHandlerComponent.PointerEntered += handler;
				break;
			case UIEventType.PointerExit:
				eventHandlerComponent.PointerExited -= handler;
				eventHandlerComponent.PointerExited += handler;
				break;
		}
	}

	protected void Bind<T>(Type type)
		where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(typeof(T));
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);
		for (int index = 0; index < names.Length; index++)
		{
			objects[index] = typeof(T) == typeof(GameObject) ? gameObject.FindChild(names[index], true) : gameObject.FindChild<T>(names[index], true);

			if (objects[index] == null)
			{
				Debug.Log($"Failed to bind: {names[index]}");
			}
		}
	}

	protected T Get<T>(int index)
		where T : UnityEngine.Object
	{
		if (!_objects.TryGetValue(typeof(T), out var objects))
		{
			return null;
		}

		return objects[index] as T;
	}
}