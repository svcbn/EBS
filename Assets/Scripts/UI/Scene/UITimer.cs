using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : UIScene
{
	private enum Elements
	{
		TimerText
	}
	
	private TextMeshProUGUI _timerText;

	private string _text;

	public override void Init()
	{
		base.Init();
		
		Bind<TextMeshProUGUI, Elements>();
		_timerText = Get<TextMeshProUGUI>((int)Elements.TimerText);
	}
	
	public void SetTimerText(string text)
	{
		if (_text == null)
		{
			return;
		}
		
		_text = text;
		_timerText.text = _text;
	}
}
