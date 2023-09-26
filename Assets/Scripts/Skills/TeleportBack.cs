using System.Collections;
using UnityEngine;

public class TeleportBack : SkillBase, IActiveSkill
{
	private TeleportBackData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<TeleportBackData>("Data/TeleportBackData");
		if (_data == null){ Debug.LogWarning($"Fail load Data/TeleportBackData"); return; }

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown		  = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		AfterDelay        = _data.AfterDelay;

	}

	public override void Execute()
	{		
		base.Execute();
		StartCoroutine(PlayTelepotEffect(Owner.transform)); // TBD: 이펙트 재생 위치 및 시간 고려필요
	}

	public override IEnumerator ExecuteImplCo()
	{
		Owner.transform.position = CalcTeleportPos();

		StartCoroutine(PlayPostEffect(Owner.transform));
		yield return new WaitForSeconds(AfterDelay);
	}

	Vector3 CalcTeleportPos()
	{
		Vector3 destV;

		Camera mainCamera = Camera.main;
		Vector3 centerV = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));

		Transform ownerTrans = Owner.transform;
		Transform enemyTrans = Owner.GetTarget().transform;

		// 적 기준, 좌 우 텔포위치
		Vector3 enemyLeft  = enemyTrans.position + enemyTrans.right * -_data.telpoDistance;
		Vector3 enemyRight = enemyTrans.position + enemyTrans.right *  _data.telpoDistance;


		if( enemyTrans.position.x - centerV.x > 0){ // 중앙 오른쪽에 있으면
			destV = enemyLeft; // 텔포 위치는 적의 왼쪽
		}else{
			destV = enemyRight;
		}

		// 현재 높이 유지
		destV = new Vector3(destV.x, ownerTrans.position.y , destV.z );
		return destV;
	}

	IEnumerator PlayTelepotEffect(Transform pos)
	{
		GameObject effect = null;
		if (_data.teleportEffect != null)
		{
			string effName = "Teleport_Before";
			effect = Managers.Resource.Instantiate("Skills/"+effName);
			effect.transform.position = Owner.transform.position;
		}

		yield return new WaitForSeconds(0.5f); // 이펙트 재생 시간

		Managers.Resource.Release(effect);
	}

	IEnumerator PlayPostEffect(Transform pos)
	{
		GameObject effect = null;
		if (_data.postEffect != null)
		{
			string effName = "Teleport_After";
			effect = Managers.Resource.Instantiate("Skills/"+effName);
			effect.transform.position = Owner.transform.position;
		}

		yield return new WaitForSeconds(0.5f); // 이펙트 재생 시간

		Managers.Resource.Release(effect);
	}
    
	
	void OnDrawGizmos() 
	{
		// Gizmos.color = Color.red;
		// Vector3 checkboxPos = Owner.transform.position;
		// Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);
    }

	public override bool CheckCanUse()
	{
		Vector3 center = (Vector2)Owner.transform.position + _data.CheckBoxCenter;
		Vector3 size =  _data.CheckBoxSize;

		var boxes = Physics2D.OverlapBoxAll( center, size , 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			if ( character == Owner.GetTarget().GetComponent<Character>() ){
				return true;
			}
		}
		return false;
	}

}

