using System.Collections;
using UnityEngine;

public class Block : SkillBase, IActiveSkill
{
	private BlockData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<BlockData>("Data/BlockData");
		if (_data == null){ Debug.LogWarning($"Fail load Data/BlockData"); return;  }

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestrictMoving  = _data.IsRestrictMoving;

		Duration		  = _data.Duration;

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
		StartCoroutine(PlayEffect(Owner.transform));


		// TODO: _data.Duration 초간 무적,CC면역, 
		// DamageManager에서 함수 제공 예정


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	
	IEnumerator PlayEffect(Transform pos)
	{
		GameObject effect = null;
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name);
			effect.transform.position = Owner.transform.position;
		}

		yield return new WaitForSeconds(_data.Duration); // 이펙트 재생 시간

		Managers.Resource.Release(effect);
	}


	public override bool CheckCanUse() //CheckCanUse
	{
		Character enemy = Owner.GetTarget().GetComponent<Character>();

		if(enemy              == null){ return false; }
		if(enemy.CurrentSkill == null){ return false; }

		bool isEnemyInBeforeDelay = ((SkillBase)enemy.CurrentSkill).IsBeforeDelay; 

		return isEnemyInBeforeDelay;
	}
}

