using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마나샤워 이름 바꾸기 -> 힐링 샤워로 
public class ManaShower : ActiveSkillBase, IActiveSkill
{
	private ManaShowerData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<ManaShowerData>("Data/ManaShowerData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/ManaShowerData"); return; }

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

		return isEnemyInBox && isEnoughMP;
	}
}
