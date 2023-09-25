using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSelector : UIScene
{
	private enum Elements
	{
		Items,
	}

	private List<UISkillSlot> _slots = new();

	private List<SkillInfo> _skillInfoList;

	private int _currentIndex = -1;

	private int _columnCount = 3;

	private void Update()
	{
		Direction direction;
		if (Input.GetKeyDown(KeyCode.W))
		{
			direction = Direction.Up;
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			direction = Direction.Left;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			direction = Direction.Down;
		}
		else if (Input.GetKeyDown(KeyCode.D))
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
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (Transform child in items.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}

		InitializeSlots();
	}

	public void SetItems(List<SkillInfo> skills)
	{
		_skillInfoList = skills;
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		_currentIndex = -1;
		_slots?.Clear();

		if (_skillInfoList != null)
		{
			GameObject items = Get<GameObject>((int)Elements.Items);

			foreach (var info in _skillInfoList)
			{
				var slot = CreateSlot(info);
				slot.transform.SetParent(items.transform);
				slot.transform.localScale = Vector3.one;
				_slots.Add(slot);
			}

			_currentIndex = 0;
			_slots.First().Select();
		}
	}

	private UISkillSlot CreateSlot(SkillInfo info)
	{
		GameObject go = Managers.Resource.Instantiate("UI/Scene/UISkillSlot");
		
		var slot = go.GetOrAddComponent<UISkillSlot>();
		slot.SetSkill(info);
		
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

		_slots[_currentIndex].Unselect();

		_currentIndex = row * _columnCount + column;
		_slots[_currentIndex].Select();
	}
}
