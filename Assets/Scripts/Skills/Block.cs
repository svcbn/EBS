using System.Collections;
using UnityEngine;

public class Block : ActiveSkillBase
{
	private BlockData _data;

	public override void Init()
	{
		base.Init();

		_data = LoadData<BlockData>();

		Duration = _data.Duration;
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

		bool isEnemyInBeforeDelay = ((ActiveSkillBase)enemy.CurrentSkill).IsBeforeDelay; 

		return isEnemyInBeforeDelay;
	}
}

