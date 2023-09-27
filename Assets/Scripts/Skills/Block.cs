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
		if (_data.Effect != null){ PlayEffect("Block_Green", Duration, Vector2.zero); }

		Managers.Stat.BeInvincible(Owner.playerIndex ,Duration);

		// 후딜
		yield return new WaitForSeconds(AfterDelay);
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

