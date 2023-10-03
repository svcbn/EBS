using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingShower : ActiveSkillBase
{
	private HealingShowerData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<HealingShowerData>("Data/HealingShowerData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/HealingShowerData"); return; }

		Id               = _data.Id;
		Type             = _data.Type;
		Priority         = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown         = _data.Cooldown;
		BeforeDelay      = _data.BeforeDelay;
		AfterDelay       = _data.AfterDelay;
		RequireMP        = _data.RequireMP;

		Duration         = _data.effectDuration;
	}

	public override void Execute()
	{
		base.Execute();
	}

	public override IEnumerator ExecuteImplCo()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		// 애니메이션 재생
		if (_data.Effect != null){
			PlayEffect( _data.Effect.name, Duration, Vector3.zero, x );
		}

		// Todo : statmanager 쪽에 마나 회복 요청
		Managers.Stat.GiveHeal(Owner.playerIndex, _data.Amount);

		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = !CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);
		bool isEnoughMP   = CheckEnoughMP(RequireMP);
		bool isFullHP	  = CheckFullHp();

		return isEnemyInBox && isEnoughMP && !isFullHP ;
	}

	private bool CheckFullHp()
	{
		int CurrentHp = Managers.Stat.GetCurrentHp(Owner.playerIndex );
		int MaxHp     = Managers.Stat.GetMaxHp(Owner.playerIndex );

		if ( CurrentHp == MaxHp)
		{
			return true;
		}

		return false;
	}
}
