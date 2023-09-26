using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
	public static Coroutine Lerp(float from, float to, float duration, System.Action<float> action)
	{
		return Managers.Instance.StartCoroutine(LerpCoroutine(from, to, duration, action));
	}

	public static Coroutine Lerp(Vector3 from, Vector3 to, float duration, System.Action<Vector3> action)
	{
		return Managers.Instance.StartCoroutine(LerpCoroutine(from, to, duration, action));
	}

	private static IEnumerator LerpCoroutine(float from, float to, float duration, System.Action<float> action)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime;
			float value = Mathf.Lerp(from, to, time / duration);
			action?.Invoke(value);
			yield return null;
		}
		action?.Invoke(to);
	}

	public static IEnumerator LerpCoroutine(Vector3 from, Vector3 to, float duration, System.Action<Vector3> action)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime;
			Vector3 value = Vector3.Lerp(from, to, time / duration);
			action?.Invoke(value);
			yield return null;
		}
		action?.Invoke(to);
	}
}
