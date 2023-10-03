using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINumberBalloon : UIScene
{
	private string _text;

	private Color _color = Color.black;

	private enum Texts
	{
		BalloonInnerText
	}
	
	private void OnEnable()
	{
		// TODO: Popup 처리
	}

	public override void Init()
	{
		base.Init();
		
		Bind<TextMeshProUGUI, Texts>();
		SetInnerText();
	}
	
	public void InitialPosition(Vector3 position)
	{
		transform.position = position;
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
	}
}
