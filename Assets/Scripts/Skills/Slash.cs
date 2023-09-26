using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillBase, IActiveSkill
{
	private SlashData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SlashData>("Data/SlashData");

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestricteMoving = _data.IsRestricteMoving;

		Cooldown          = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		Duration          = _data.Duration;
		AfterDelay        = _data.AfterDelay;
	}

	public override void Execute()
	{
		if (_data == null){ Debug.LogWarning($"Fail load Data/SlashData"); return;  }
		
		
		base.Execute();
		Owner.StartCoroutine(ExecuteCo());
	}

	IEnumerator ExecuteCo()
	{
		// 애니메이션 재생
		_data.SpriteEffect.Play();

		// 선딜
		Debug.Log($"선딜 시작  {BeforeDelay}");
		yield return new WaitForSeconds(BeforeDelay);

		// 실제 피해
		Debug.Log($"실제 피해 ");
		Debug.Log($"시전 시간  {Duration}");


		var boxes = Physics2D.OverlapBoxAll((Vector2)Owner.transform.right + _data.HitBoxCenter, _data.HitBoxSize, 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			character.TakeDamage(1);
			Debug.Log(character.name + "에게 피해를 입힘.");
		}

		// 후딜
		Debug.Log($"후딜 시작  {AfterDelay}");
		yield return new WaitForSeconds(AfterDelay);


	}



	public void OnDrawGizmos()
	{
		var data = Resources.Load<SlashData>("Data/SlashData");
		Gizmos.color = Color.red;
		Gizmos.DrawCube((Vector2)transform.position + data.HitBoxCenter, data.HitBoxSize);
	}
}

