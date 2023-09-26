using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : UIBase
{
	private enum Elements
	{
		Icon,
		Dim
	}

	private static readonly Color s_SelectedColor = new(1, 0, 0, 0.5f);

	private static readonly Color s_UnselectedColor = new(1, 1, 1, 0.5f);

	private static readonly float s_SelectedScale = 1.1f;

	private SkillInfo _info;

	private Image _border;

	private Coroutine _scaleHandler;

	public SkillInfo SkillInfo => _info;

	protected override void Awake()
	{
		base.Awake();

		_border = GetComponent<Image>();
	}

	private void OnEnable()
	{
		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		Get<GameObject>((int)Elements.Dim).SetActive(false);
	}

	public override void Init()
	{
		Bind<GameObject, Elements>();

		if (_info != null)
		{
			SetSkillInfo();
		}
	}

	public void SetSkill(SkillInfo info)
	{
		_info = info;
		SetSkillInfo();
	}

	public void Disable()
	{
		enabled = false;
		Get<GameObject>((int)Elements.Dim).SetActive(true);
	}

	public void ShowChoiceEffect()
	{
		Vector3 scale = transform.localScale;
		Vector3 targetScale = Vector3.one * 0.95f;
		float duration = 0.07f;
		System.Action callback = () => _scaleHandler = Utility.Lerp(targetScale, scale, duration, vector2 => transform.localScale = vector2);
		_scaleHandler = Utility.Lerp(scale, targetScale, duration, vector => transform.localScale = vector, callback);
	}

	public void Select()
	{
		if (_scaleHandler != null)
		{
			Managers.Instance.StopCoroutine(_scaleHandler);
			_scaleHandler = null;
		}

		if (_border != null)
		{
			_border.color = s_SelectedColor;
		}
		_scaleHandler = Utility.Lerp(Vector3.one, Vector3.one * s_SelectedScale, 0.1f, vector => transform.localScale = vector);
	}

	public void Unselect()
	{
		if (_scaleHandler != null)
		{
			Managers.Instance.StopCoroutine(_scaleHandler);
			_scaleHandler = null;
		}

		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		transform.localScale = Vector3.one;
	}

	private void SetSkillInfo()
	{
		var iconRoot = Get<GameObject>((int)Elements.Icon);
		if (iconRoot.TryGetComponent<Image>(out var icon))
		{
			// TODO : 아이콘 이미지 변경
			if (_info != null)
			{
				icon.sprite = _info.Sprite;
			}
		}
	}
}
