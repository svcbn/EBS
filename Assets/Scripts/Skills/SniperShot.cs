using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShot : SkillBase, IActiveSkill
{
	private SniperShotData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SniperShotData>("Data/SniperShotData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/SniperShotData"); return; }

		Id = _data.Id;
		Type = _data.Type;
		Priority = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown = _data.Cooldown;
		BeforeDelay = _data.BeforeDelay;
		AfterDelay = _data.AfterDelay;
		RequireMP = _data.RequireMP;
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		GameObject effect2 = null;

		// 애니메이션 재생
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect.transform.position = Owner.transform.position;
		}

		Owner.GetComponent<CharacterStatus>().SetKnockbackEffect(0.1f, 5f, Owner.Target.transform.position);

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

		if (_data.HitEffect != null)
		{
			effect2 = Managers.Resource.Instantiate("Skills/" + _data.HitEffect.name);
			effect2.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect2.transform.position = Owner.Target.transform.position;
		}

		Owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(0.5f, 10f, Owner.transform.position);

		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
		Managers.Resource.Release(effect2);

	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		bool isEnoughMP = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}
