using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : SkillBase, IActiveSkill
{
	private TeleportBackData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<TeleportBackData>("Data/TeleportBackData");

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestricteMoving = _data.IsRestricteMoving;

		Cooldown          = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		Duration          = _data.Duration;
		AfterDelay        = _data.AfterDelay;
	}

	public override void Execute()
	{
		if (_data == null){ Debug.LogWarning($"Fail load Data/TeleportBackData"); return;  }
		
		
		base.Execute();
		Owner.StartCoroutine(ExecuteCo());
	}

	IEnumerator ExecuteCo()
	{
		// 애니메이션 재생
		if(_data.SpriteEffect != null){
			_data.SpriteEffect.transform.position = Owner.transform.position;
			_data.SpriteEffect.Play();
		}


		// 선딜
		Debug.Log($"텔포 선딜 시작  {BeforeDelay}");
		//yield return new WaitForSeconds(BeforeDelay); // tmp 삭제 for test

		// 캐릭터 이동, 1)일단 한 방향 2)두 방향
		Debug.Log($"텔포 시전 시간  {Duration}");



		Owner.transform.position = CalcTeleportPos();


		// 후딜
		Debug.Log($"텔포 후딜 시작  {AfterDelay}");
		yield return new WaitForSeconds(AfterDelay);
	}

	Vector3 CalcTeleportPos()
	{
		Vector3 destV;


		Transform ownerTrans = Owner.transform;
		Transform enemyTrans = Owner.GetTarget().transform;

		// 적 기준, 좌 우 선택
		Vector3 enemyLeft  = enemyTrans.position + enemyTrans.right * -5;
		Vector3 enemyRight = enemyTrans.position + enemyTrans.right *  5;  // 거리 5칸

		// 막히는 거리까지 판단
		// Vector3 maxLeft  = ;
		// Vector3 maxRight = ;


		// 거리 계산 : 둘 중 나에게서 가장 먼 곳
		float left  = Vector3.Distance( ownerTrans.position, enemyLeft  );
		float right = Vector3.Distance( ownerTrans.position, enemyRight );
		
		if  ( left > right ){ destV = enemyLeft;  }
		else                { destV = enemyRight; }
		
		// 현재 높이 유지
		destV = new Vector3(destV.x, ownerTrans.position.y , destV.z );


		Debug.Log( $"destV : {destV}");

		return destV;
	}




	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 hitboxPos = Owner.transform.position;
		// if ( gameObject != null){
		// 	hitboxPos = transform.position;
		// }

		Debug.Log( $"OnDrawGizmos() | hitboxPos {hitboxPos}  hitBoxCenter {_data.HitBoxCenter}  hitBoxSize {_data.HitBoxSize} " );

		Gizmos.DrawCube(hitboxPos + (Vector3)_data.HitBoxCenter, (Vector3)_data.HitBoxSize);
	}
}

