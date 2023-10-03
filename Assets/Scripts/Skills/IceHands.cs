using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHands : PassiveSkillBase
{
	CharacterStatus _status;

	IceHandsData _data;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<IceHandsData>("Data/IceHandsData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/IceHandsData"); return; }

		Id = _data.Id;

		Managers.Stat.onTakeDamage += Excute;
	}

	void Excute(int playerIndex)
	{
		if (Owner.playerIndex != playerIndex)
		{
			IsEnabled = true;

			Owner.Target.GetComponent<CharacterStatus>().SetSlowEffect(_data.EffectDuration, _data.SpeedDownRatio);
		}
	}
}
