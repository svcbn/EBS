using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

partial class UISkillSlot
{
	private void CheckCooldown()
	{
		Image image = Get<Image>((int)Images.CooldownIndicator);
		image.fillAmount = 1-  _skill.CurrentCooldown / _skill.Cooldown;
	}

	private void RegisterSkillEvents()
	{
		if (_skill is IPassiveSkill passiveSkill)
		{
			passiveSkill.PropertyChanged += OnPassiveSkillPropertyChanged;
		}
	}

	private void UnregisterSkillEvents()
	{
		if (_skill is IPassiveSkill passiveSkill)
		{
			passiveSkill.PropertyChanged -= OnPassiveSkillPropertyChanged;
		}
	}

	private void OnPassiveSkillPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (sender is not IPassiveSkill passiveSkill)
		{
			return;
		}
		
		switch (e.PropertyName)
		{
			case nameof(IPassiveSkill.PresentNumber):
				SetPresentText(passiveSkill);
				break;
			case nameof(IPassiveSkill.IsEnabled):
				if (passiveSkill.IsEnabled)
				{
					ShowEnableEffect();
				}
				break;
		}
	}

	private void ShowEnableEffect()
	{
		const float duration = 0.12f;
		var icon = Get<Image, Images>(Images.Icon);
		Utility.Lerp(Vector3.one, Vector3.one * 1.1f, duration, vector => transform.localScale = vector,
			() => Utility.Lerp(Vector3.one * 1.1f, Vector3.one, duration, vector => transform.localScale = vector));
		Utility.Lerp(1, 0.5f, duration, alpha => icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha),
			() => Utility.Lerp(0.5f, 1f, duration, alpha => icon.color = new(icon.color.r, icon.color.g, icon.color.b, alpha)));
	}

	private void SetPresentText(IPassiveSkill skill)
	{
		TextMeshProUGUI textBox = Get<TextMeshProUGUI, Texts>(Texts.PresentText);
		textBox.text = skill.PresentNumber.ToString();
	}
}
