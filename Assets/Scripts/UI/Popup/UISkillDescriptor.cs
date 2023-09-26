using TMPro;
using UnityEngine;

public class UISkillDescriptor : UIPopup
{
	private enum Elements
	{
		Name,
		SkillType,
		Cooldown,
		Description
	}

	private static readonly Vector3 s_InitialScale = new(0.95f, 0.95f, 0.95f);

	private SkillInfo _skillInfo;

	private void OnEnable()
	{
		var card = gameObject.FindChild<RectTransform>("Card");
		if (card != null)
		{
			Utility.Lerp(s_InitialScale, Vector3.one, 0.1f, scale => card.localScale = scale);
		}
	}

	public override void Init()
	{
		base.Init();
		Bind<TextMeshProUGUI, Elements>();
		SetText();
	}

	public void SetSkillInfo(SkillInfo skillInfo)
	{
		_skillInfo = skillInfo;
		SetText();
	}

	private void SetText()
	{
		if (_skillInfo == null)
		{
			return;
		}

		Get<TextMeshProUGUI>((int)Elements.Name).text = _skillInfo.Name;
		Get<TextMeshProUGUI>((int)Elements.SkillType).text = _skillInfo.SkillType;
		Get<TextMeshProUGUI>((int)Elements.Cooldown).text = $"쿨타임: {_skillInfo.CoolDown}초";
		Get<TextMeshProUGUI>((int)Elements.Description).text = _skillInfo.Description;
	}
}
