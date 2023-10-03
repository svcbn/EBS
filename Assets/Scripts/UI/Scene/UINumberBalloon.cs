using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINumberBalloon : UIScene
{
	private string _text;

	private Color _color = Color.black;

	private Vector3 _position;

	private Coroutine _positionHandler;
	
	private Coroutine _colorHandler;

	private enum Texts
	{
		BalloonInnerText
	}

	private void OnEnable()
	{
		SetPositionEffect();
	}

	private void OnDisable()
	{
		if (_positionHandler != null)
		{
			Utility.StopCoroutine(_positionHandler);
			_positionHandler = null;
		}

		if (_colorHandler != null)
		{
			Utility.StopCoroutine(_colorHandler);
			_colorHandler = null;
		}
	}

	public override void Init()
	{
		Bind<TextMeshProUGUI, Texts>();
		SetInnerText();
	}
	
	public void SetInitialPosition(Vector3 position)
	{
		_position = position;
		transform.position = _position;

		if (_positionHandler != null)
		{
			Utility.StopCoroutine(_positionHandler);
			SetPositionEffect();
		}
	}

	public void SetText(string text, Color color)
	{
		_text = text;
		_color = color;
		SetInnerText();
	}

	private void SetInnerText()
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.BalloonInnerText);
		textBox.text = _text;
		textBox.color = _color;
		if (_colorHandler != null)
		{
			Utility.StopCoroutine(_colorHandler);
		}
		SetColorEffect();
	}

	private void SetPositionEffect()
	{
		Vector3 targetPosition = _position + Vector3.up * 5f;
		_positionHandler = Utility.Lerp(_position, targetPosition, 1.5f, vector => transform.position = vector, () =>
		{
			Managers.Resource.Release(gameObject);
			_positionHandler = null;
		});
	}

	private void SetColorEffect()
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.BalloonInnerText);
		_colorHandler = Utility.Lerp(1, 0f, 1f, alpha => textBox.color = new(textBox.color.r, textBox.color.g, textBox.color.b, alpha), () => _colorHandler = null);
	}
}
