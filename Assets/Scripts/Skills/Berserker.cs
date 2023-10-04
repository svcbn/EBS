using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : PassiveSkillBase
{
	CharacterStatus _status;

	BerserkerData _data;

	bool _isActived;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<BerserkerData>("Data/BerserkerData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/BerserkerData"); return; }

		Id = _data.Id;
	
		Managers.Stat.onTakeDamage += Excute;
	}

	public override void Reset()
	{
		base.Reset();

		_isActived = false;
	}

	void Excute(int playerIndex, int finalDamage)
	{
		if ((Managers.Stat.GetCurrentHp(Owner.playerIndex) / (float)Managers.Stat.GetMaxHp(Owner.playerIndex)) < 0.5f && _isActived == false)
		{
			Debug.Log($"{Managers.Stat.GetCurrentHp(Owner.playerIndex)}, {Managers.Stat.GetMaxHp(Owner.playerIndex)}");
			_isActived = true;

			IsEnabled = true;

			_status.HasteRatio += _data.SpeedUpRatio;
			_status.CooldownChange += _data.CooldownRatio;

			Modifier modifier = Managers.Stat.GetDamageModifier(Owner.playerIndex, GetType().Name);
			modifier.value = _data.PowerUpRatio;
		}
	}
}
