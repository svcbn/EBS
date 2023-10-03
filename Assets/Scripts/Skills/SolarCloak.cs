using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarCloak : PassiveSkillBase
{
	private SolarCloakData _data;
	public bool IsCoolReady { get; protected set; } = true;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SolarCloakData>("Data/SolarCloakData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/SolarCloakData"); return; }

		Id = _data.Id;
		Cooldown = _data.Cooldown;

		PresentNumber = _data.PresentNumber;
		HasPresentNumber = _data.HasPresentNumber;

		GameObject effect = Managers.Resource.Instantiate("Skills/" + _data.DefaultEffect.name);
		effect.transform.SetParent(Owner.transform);

		Managers.Stat.AddMaxHp(Owner.playerIndex);
	}

	private void Update()
	{
		if (IsCoolReady)
		{
			IsCoolReady = false;
			if (_data.Effect != null)
			{
				PlayEffect(_data.Effect.name, 1);
			}
			DoDamage();
			CalculateCoolTime();
		}
	}

	void DoDamage()
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;

		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			//StatManager 쪽에 데미지 연산 요청
			Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.Damage);
		}
	}


	void CalculateCoolTime()
	{
		Owner.StartCoroutine(CoCalculateTime(Cooldown, time => CurrentCooldown = time, () => IsCoolReady = true));
	}

	private static IEnumerator CoCalculateTime(float time, Action<float> onUpdate, Action onFinish)
	{
		float timer = 0;
		while (timer < time)
{
			yield return null;
			timer += Time.deltaTime;
			onUpdate?.Invoke(timer);
		}

		onFinish?.Invoke();
	}

}
