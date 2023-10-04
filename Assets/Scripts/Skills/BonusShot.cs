using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BonusShot : PassiveSkillBase
{
	private BonusShotData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<BonusShotData>("Data/BonusShotData");
		if (_data == null){ Debug.LogWarning($"Fail load Data/BonusShotData"); return;  }

		Id                = _data.Id;
		Cooldown          = _data.Cooldown;

		Managers.Stat.onTakeDamage += Execute; // 누가 맞았음.

	}


	// 1조건 : 적을 타격시
	// 2조건 : 적이 범위 안에 없을때
	void Execute(int playerIndex)
	{
		if (Owner.playerIndex != playerIndex) // 적이 맞음
		{
			bool isEnemyInBox = CheckEnemyInBox(_data.checkBoxCenter, _data.checkBoxSize);
			if( isEnemyInBox ){ return; } // 박스안에 없어야 됨
			
			StartCoroutine(ShotBonusArrow());
		}
	}


	IEnumerator ShotBonusArrow()
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

				if( _data.missilePrefab == null){ Debug.LogWarning("_data.missilePrefab is null"); yield break;}

				GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

				if(g == null){
					Debug.LogWarning("g is null");
				}

				g.GetComponent<TriggerAttackerBonusShot>().owner = Owner;
				g.GetComponent<TriggerAttackerBonusShot>().damage = 1; // todo: 20% damage

				g.target = validTargets[colCount].transform;
				g.bezierDelta  = _data.bezierDelta;  // 상대 원
				g.bezierDelta2 = _data.bezierDelta2; // 나 원
				g.Init(Owner);

				colCount += 1;            
			}
		}


	}

	

	protected virtual bool CheckEnemyInBox(Vector2 center, Vector2 size)
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;
		
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * center.x, center.y);

		var boxes = Physics2D.OverlapBoxAll(centerInWorld, size, 0);
		bool flag = boxes.Length != 0;
		return boxes.Any(box => box.TryGetComponent<Character>(out var character) && character != Owner);
	}

	private void OnDrawGizmos() 
	{
		Gizmos.color = new Color(128f/255f, 0, 128f/255f);//Color.purple;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.checkBoxCenter, (Vector3)_data.checkBoxSize);	
	}



}

