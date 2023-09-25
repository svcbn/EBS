using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillBase
{
	private SlashData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SlashData>("Data/SlashData");
	}

	public override void Execute()
	{
		base.Execute();

		if (_data == null)
		{
			// TODO:
		}

		var boxes = Physics2D.OverlapBoxAll((Vector2)Owner.transform.right + _data.HitBoxCenter, _data.HitBoxSize, 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			character.TakeDamage(1);
			Debug.Log(character.name + "���� �������� �������ϴ�.");
		}
	}

	public override void OnDrawGizmos(Transform character)
	{
		base.OnDrawGizmos(character);

		// var data = Resources.Load<SlashData>("Data/SlashData");
		// Gizmos.color = Color.red;
		//Gizmos.DrawCube((Vector2)character.position + data.HitBoxCenter, data.HitBoxSize);
	}
}

[CreateAssetMenu(fileName = nameof(SlashData), menuName = "ScriptableObjects/Skills/" + nameof(SlashData))]
public class SlashData : ScriptableObject
{
	[Header("OverlapBox")]
	[Header("Check")]
	[SerializeField]
	private Vector2 _checkBoxCenter;

	[SerializeField]
	private Vector2 _checkBoxSize;

	[Header("HitBox")]
	[SerializeField]
	private Vector2 _hitBoxCenter;

	[SerializeField]
	private Vector2 _hitBoxSize;

	public Vector2 CheckBoxCenter => _checkBoxCenter;

	public Vector2 CheckBoxSize => _checkBoxSize;

	public Vector2 HitBoxCenter => _hitBoxCenter;

	public Vector2 HitBoxSize => _hitBoxSize;
}