using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaShower : ActiveSkillBase, IActiveSkill
{
	private ManaShowerData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<ManaShowerData>("Data/ManaShowerData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/ManaShowerData"); return; }

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

		// Todo : statmanager 쪽에 마나 회복 요청
		Managers.Stat.GiveHeal(Owner.playerIndex, _data.Amount);
		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = !CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		bool isEnoughMP = CheckEnoughMP(RequireMP);

		return isEnemyInBox && isEnoughMP;
	}
}
