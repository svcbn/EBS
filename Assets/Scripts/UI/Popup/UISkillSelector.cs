using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UISkillSelector : UIPopup
{
	private enum Elements
	{
		Items,
	}

	private enum Texts
	{
		PickerText
	}
	
	private static readonly Color[] s_Colors = new[] { Color.red, Color.blue };
	
	private int _playerIndex = 0;
	
	private readonly List<UISkillSlot> _slots = new();

	private SkillSelectorInput _input;

	private SkillSelector _selector;

	private UISkillDescriptor _descriptor;

	private TextMeshProUGUI _pickerText;

	private int _currentIndex = -1;

	private int _columnCount = 3;

	private void Update()
	{
		if (_selector.Input == null)
		{
			return;
		}
		
		_pickerText.text = _input.Owner;
		HandleDirectionInput();
		HandleSelect();
	}

	private void OnDisable()
	{
		ClearSlots();
		if (_descriptor != null)
		{
			Managers.UI.ClosePopupUI(_descriptor);
			_descriptor = null;
		}
	}

	private void HandleSelect()
	{
		if (_currentIndex != -1 && Input.GetKeyDown(_input.Select))
		{
			var slot = _slots[_currentIndex];
			if (slot.IsEnabled)
			{
				_selector.SelectSkill(_currentIndex);
				slot.SetBorderColor(s_Colors[_playerIndex]);
				slot.ShowChoiceEffect();
				slot.Disable();
			}
		}
	}

	private void HandleDirectionInput()
	{
		Direction direction;
		if (Input.GetKeyDown(_input.Up))
		{
			direction = Direction.Up;
		}
		else if (Input.GetKeyDown(_input.Left))
		{
			direction = Direction.Left;
		}
		else if (Input.GetKeyDown(_input.Down))
		{
			direction = Direction.Down;
		}
		else if (Input.GetKeyDown(_input.Right))
		{
			direction = Direction.Right;
		}
		else
		{
			return;
		}

		Move(direction);
	}

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		Bind<TextMeshProUGUI, Texts>();

		_pickerText = Get<TextMeshProUGUI>((int)Texts.PickerText);
		
		InitializeSlots();
	}

	public void SetSelector(SkillSelector selector)
	{
		_selector = selector;
		_selector.InputChanged += input =>
		{
			if (_input != _selector.Input)
			{
				_playerIndex++;
				_playerIndex %= s_Colors.Length;
				_input = _selector.Input;
			}
			
			_slots[_currentIndex].SetBorderColor(s_Colors[_playerIndex]);
		};
		_input = selector.Input;
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		_currentIndex = -1;
		ClearSlots();

		if (_selector?.Skills?.Any() is true)
		{
			GameObject items = Get<GameObject>((int)Elements.Items);

			foreach (var info in _selector.Skills)
			{
				var slot = CreateSlot(info);
				slot.transform.SetParent(items.transform);
				slot.transform.localScale = Vector3.one;
				_slots.Add(slot);
			}

			SelectItem(0);
		}
	}

	private void ClearSlots()
	{
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (Transform child in items.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}
		_slots?.Clear();
	}

	private UISkillSlot CreateSlot(SkillInfo info)
	{
		GameObject go = Managers.Resource.Instantiate("UI/Popup/UISkillSlot");
		
		var slot = go.GetOrAddComponent<UISkillSlot>();
		slot.SetInfo(info);
		
		return slot;
	}

	private void Move(Direction direction)
	{
		if (_slots?.Any() is not true)
		{
			return;
		}

		int childCount = _slots.Count;
		int row = _currentIndex / _columnCount;
		int column = _currentIndex % _columnCount;

		switch (direction)
		{
			case Direction.Left:
				column -= 1;
				break;
			case Direction.Right:
				column += 1;
				break;
			case Direction.Up:
				row -= 1;
				break;
			case Direction.Down:
				row += 1;
				break;
			case Direction.None:
				return;
		}

		// Check out of bounds
		if (row < 0 || row >= childCount / (float)_columnCount || column < 0 || column >= _columnCount)
		{
			return;
		}

		// Check out of range
		if (row * _columnCount + column >= childCount)
		{
			return;
		}

		int newIndex = row * _columnCount + column;
		SelectItem(newIndex);
	}

	private void SelectItem(int newIndex)
	{
		if (_currentIndex != -1)
		{
			_slots[_currentIndex].Unselect();
		}

		_currentIndex = newIndex;
		_slots[_currentIndex].SetBorderColor(s_Colors[_playerIndex]);
		_slots[_currentIndex].Select();

		ShowDescriptionUI();
	}

	private void ShowDescriptionUI()
	{
		if (_descriptor != null)
		{
			Managers.UI.ClosePopupUI(_descriptor);
		}

		var items = Get<GameObject>((int)Elements.Items);
		if (!items.TryGetComponent<RectTransform>(out var parent))
		{
			return;
		}

		_descriptor = Managers.UI.ShowPopupUI<UISkillDescriptor>(usePool: true);
		var card = _descriptor.gameObject.FindChild("Card");
		if (!card.TryGetComponent<RectTransform>(out var child))
		{
			return;
		}

		float width = (parent.rect.width / 2f) + (child.rect.width / 2f);
		float space = 10f;
		child.transform.localPosition = new(parent.localPosition.x + width + space, 0, 0);
		_descriptor.SetSkillInfo(_slots[_currentIndex].SkillInfo);
	}
}
