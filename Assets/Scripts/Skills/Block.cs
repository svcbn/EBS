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
		if (_data.Effect != null){
			Vector3 effectScale = new(0.4f, 0.4f, 0.4f);
			PlayEffect("Block", Duration, Vector2.zero, effectScale: effectScale); 
		}

		Managers.Stat.BeInvincible(Owner.playerIndex ,Duration);

		yield return new WaitForSeconds(AfterDelay);
	}

	
	public override bool CheckCanUse() //CheckCanUse
	{
		Character enemy = Owner.GetTarget().GetComponent<Character>();

		if(enemy              == null){ return false; }
		if(enemy.CurrentSkill == null){ return false; }

		bool isEnemyInBeforeDelay = ((ActiveSkillBase)enemy.CurrentSkill).IsBeforeDelay;
		float delayLeft = enemy.CurrentSkill.Cooldown - enemy.CurrentSkill.CurrentCooldown;
		if (isEnemyInBeforeDelay && delayLeft <= _data.beforehandTime)
		{
			return true;
		}

		return false;
	}
}

