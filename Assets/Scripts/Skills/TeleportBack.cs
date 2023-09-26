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
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown		  = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		Duration          = _data.Duration;
		AfterDelay        = _data.AfterDelay;

	}

	public override void Execute()
	{
		if (_data == null){ Debug.LogWarning($"Fail load Data/TeleportBackData"); return;  }
		
		if( !CheckCanUse() ){ return; }
		
		base.Execute();

		Owner.StartCoroutine(ExecuteCo());
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

			if( character == Owner.GetTarget().GetComponent<Character>() ){
				return true;
			}
		}
		return false;
	}


	IEnumerator ExecuteCo()
	{
		PlayTelepotEffect(Owner.transform);
		yield return new WaitForSeconds(BeforeDelay);

		Owner.transform.position = CalcTeleportPos();

		PlayPostEffect(Owner.transform);
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

	void PlayTelepotEffect(Transform pos )
	{
		if(_data.teleportEffect != null){
			_data.teleportEffect.transform.position = pos.position;
			_data.teleportEffect.Play();
		}else{
			Debug.Log("teleportEffect is null");
		}
	}
	void PlayPostEffect(Transform pos)
	{
		if(_data.postEffect != null){
			_data.postEffect.transform.position = pos.position;
			_data.postEffect.Play();
		}else{
			Debug.Log("teleportPostEffect is null");
		}
	}
    
	
	void OnDrawGizmos() 
	{

		Gizmos.color = Color.red;
		Vector3 checkboxPos = Owner.transform.position;
		Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);
    }

}

