using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillList : UIScene
{
	private enum Elements
	{
		LeftActive,
		LeftPassive,
		RightActive,
		RightPassive
	}

	private Character _left;

	private Character _right;

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		RegisterEvents();
	}

	public void RegisterCharacter(Character left, Character right)
	{
		_left = left;
		_right = right;
		RegisterEvents();
	}

	private void RegisterEvents()
	{
		if (_left != null)
		{
			_left.CollectionChanged -= OnCollectionChanged;
		}
		if (_right != null)
		{
			_right.CollectionChanged -= OnCollectionChanged;
		}

		_left.CollectionChanged += OnCollectionChanged;
		_right.CollectionChanged += OnCollectionChanged;
	}

	private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		
	}

	private void AddSlotToPanel(Character character, bool isLeft)
	{
		GameObject active, passive;
		if (isLeft)
		{
			active = Get<GameObject>((int)Elements.LeftActive);
			passive = Get<GameObject>((int)Elements.LeftPassive);
		}
		else
		{
			active = Get<GameObject>((int)Elements.RightActive);
			passive = Get<GameObject>((int)Elements.RightPassive);
		}

		foreach (Transform child in active.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}

		//foreach (var skill in character.Skills)
		//{
		//	var info = GameManager.Skill.GetInfo(skill.Id);
		//	var slot = CreateSlot(info);
		//	slot.transform.SetParent(items.transform);
		//	slot.transform.localScale = Vector3.one;
		//}
	}

	private UISkillSlot CreateSlot(SkillInfo info)
	{
		GameObject go = Managers.Resource.Instantiate("UI/Popup/UISkillSlot");

		var slot = go.GetOrAddComponent<UISkillSlot>();
		slot.SetInfo(info);

		return slot;
	}
}
