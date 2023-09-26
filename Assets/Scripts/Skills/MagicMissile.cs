using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : SkillBase, IActiveSkill
{
	private MagicMissileData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<MagicMissileData>("Data/MagicMissileData");
		if (_data == null){ Debug.LogWarning($"Fail load Data/MagicMissileData"); return;  }

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestrictMoving  = _data.IsRestrictMoving;

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
        int colCount = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(Owner.transform.position, _data.range, _data.targetLayer);
        if (cols.Length == 0) // 타겟이 없을때 
        {
            for (int i = 0; i < _data.missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = _data.bezierDelta;
                g.bezierDelta2 = _data.bezierDelta2;
                g.Init(Owner);
            }
            yield break;
        }

		Debug.Log($"cols: {cols.Length}");


		List<Collider2D> validTargets = new List<Collider2D>();
		foreach (var col in cols)
		{
			if (col.GetComponent<Character>() != Owner)
			{
				validTargets.Add(col);
			}
		}

		if( validTargets.Count > 0)
		{
			for(int i = 0; i < _data.missileCount; i++)
			{
				if (i%validTargets.Count==0) { colCount = 0; }
				GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);
				g.GetComponent<TriggerAttacker>().owner = Owner;
				g.GetComponent<TriggerAttacker>().damage = _data.Damage;

				g.target = validTargets[colCount].transform;
				g.bezierDelta  = _data.bezierDelta;  // 상대 원
				g.bezierDelta2 = _data.bezierDelta2; // 나 원
				g.Init(Owner);

				colCount += 1;            
			}
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	
	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);

		return isEnemyInBox;
	}

	private void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);	
	}
}

