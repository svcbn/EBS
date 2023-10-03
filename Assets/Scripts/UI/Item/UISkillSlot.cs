using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UISkillSlot : UIBase
{
	private enum Images
	{
		Icon,
		Dim,
		CooldownIndicator,
	}

	private enum Texts
	{
		PresentText
	}

	private static readonly Color s_SelectedColor = new(1, 0, 0, 0.5f);

	private static readonly Color s_UnselectedColor = new(1, 1, 1, 0.5f);

	private static readonly float s_SelectedScale = 1.1f;

	private SkillInfo _info;

	private ISkill _skill;

	private Image _border;

	private Color _selectedColor = s_SelectedColor;

	public SkillInfo SkillInfo => _info;

	protected override void Awake()
	{
		base.Awake();

		_border = GetComponent<Image>();
	}

	private void OnEnable()
	{
		IsEnabled = true;
		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		Get<Image>((int)Images.Dim).gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		UnregisterSkillEvents();
		Utility.StopCoroutine(_scaleHandler);
	}

	public override void Init()
	{
		Bind<Image, Images>();
		Bind<TextMeshProUGUI, Texts>();

		if (_info != null)
		{
			SetSkillIcon();
		}

		if (_skill != null)
		{
			RemoveBorderRect();
		}
	}

	public void SetSkill(ISkill skill)
	{
		UnregisterSkillEvents();
		_skill = skill;
		RegisterSkillEvents();
		RemoveBorderRect();

		Get<Image, Images>(Images.CooldownIndicator).fillAmount = _skill is IPassiveSkill ? 0 : 1;
		Get<TextMeshProUGUI, Texts>(Texts.PresentText).enabled = _skill is IPassiveSkill { HasPresentNumber: true };
	}

	public void SetInfo(SkillInfo info)
	{
		_info = info;
		SetSkillIcon();
	}
	
	public void SetBorderColor(Color color, bool overrideAlpha = false)
	{
		if (_border is null)
		{
			return;
		}

		_selectedColor = overrideAlpha ? color : new Color(color.r, color.g, color.b, s_SelectedColor.a);
		_border.color = _selectedColor;
	}

	private void SetSkillIcon()
	{
		var icon = Get<Image>((int)Images.Icon);
		if (_info != null)
		{
			icon.sprite = _info.Sprite;
		}
	}

	private void RemoveBorderRect()
	{
		var rect = Get<Image>((int)Images.Icon).GetComponent<RectTransform>();
		if (rect == null)
		{
			return;
		}

		rect.offsetMin = rect.offsetMax = Vector2.zero;
	}
}
