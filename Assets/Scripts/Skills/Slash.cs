using System.Collections;
using UnityEngine;

public class Slash : SkillBase, IActiveSkill
{
	private SlashData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SlashData>("Data/SlashData");
		if (_data == null){ Debug.LogWarning($"Fail load Data/SlashData"); return;  }

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown          = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		AfterDelay        = _data.AfterDelay;
		RequireMP		  = _data.RequireMP;
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		// 애니메이션 재생
		if (_data.Effect != null)
		{
			PlayEffect(_data.Effect.name, 1, _data.Offset, Owner.transform.localScale.x);

			//effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name);
			//effect.transform.position = Owner.transform.position;
		}

		yield return new WaitForSeconds(0);
		// 실제 피해
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		DebugRay(centerInWorld, _data.HitBoxSize);

		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			//StatManager 쪽에 데미지 연산 요청
			Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		bool isEnoughMP = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}

