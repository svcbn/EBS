using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStrike : SkillBase, IActiveSkill
{
	private DashStrikeData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<DashStrikeData>("Data/DashStrikeData");

		if (_data == null) { Debug.LogWarning($"Fail load Data/DashStrikeData"); return; }

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
		// 선딜
		yield return new WaitForSeconds(BeforeDelay);

		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		GameObject effect = null;
		// 애니메이션 재생
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/" + _data.Effect.name);
			effect.transform.localScale = new Vector3(x, Owner.transform.localScale.y, Owner.transform.localScale.z);
			effect.transform.position = Owner.transform.position;
		}

		// 이동
		Vector2 dir = (Owner.Target.transform.position - Owner.transform.position).normalized;
		transform.position += new Vector3(dir.x, dir.y, 0) * 4f;
		if(Owner.transform.position.y < 1f)
		{
			Owner.transform.position = new Vector3(Owner.transform.position.x, 1f, Owner.transform.position.z);
		}

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

			// Todo : statmanager 쪽에 데미지 연산 요청

			// Todo : charactorstatus 쪽에 스턴 요청
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
