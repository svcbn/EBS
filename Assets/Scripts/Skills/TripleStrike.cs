using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleStrike : SkillBase, IActiveSkill
{
	private TripleStrikeData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<TripleStrikeData>("Data/TripleStrikeData");

		Id = _data.Id;
		Type = _data.Type;
		Priority = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown = _data.Cooldown;
		BeforeDelay = _data.BeforeDelay;
		Duration = _data.Duration;
		AfterDelay = _data.AfterDelay;
		RequireMP = _data.RequireMP;
	}

	public override void Execute()
	{
		if (_data == null) { Debug.LogWarning($"Fail load Data/TripleStrikeData"); return; }


		base.Execute();
		Owner.StartCoroutine(ExecuteCo());
	}

	IEnumerator ExecuteCo()
	{
		// 선딜
		Debug.Log($"선딜 시작  {BeforeDelay}");
		yield return new WaitForSeconds(BeforeDelay);

		// 애니메이션 재생
		if (_data.Effect != null)
		{
			_data.Effect.transform.position = Owner.transform.position;
			_data.Effect.SetActive(true);
		}

		for (int i = 0; i < 3; i++)
		{
			// 실제 피해
			Debug.Log($"실제 피해 ");
			Debug.Log($"시전 시간  {Duration}");

			float x = Owner.transform.localScale.x < 0 ? -1 : 1;
			Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
			var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
			foreach (var box in boxes)
			{
				if (!box.TryGetComponent<Character>(out var character) || character == Owner)
				{
					continue;
				}

				character.TakeDamage(1);
				Debug.Log(character.name + "에게 피해를 입힘.");
			}
		}


		// 후딜
		Debug.Log($"후딜 시작  {AfterDelay}");
		yield return new WaitForSeconds(AfterDelay);

		_data.Effect.SetActive(false);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);
		return isEnemyInBox;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 hitboxPos = Owner.transform.position;
		// if ( gameObject != null){
		// 	hitboxPos = transform.position;
		// }

		//Debug.Log( $"OnDrawGizmos() | hitboxPos {hitboxPos}  hitBoxCenter {_data.HitBoxCenter}  hitBoxSize {_data.HitBoxSize} " );

		//Gizmos.DrawCube(hitboxPos + (Vector3)_data.HitBoxCenter, (Vector3)_data.HitBoxSize);
	}
}
