using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstoppable : PassiveSkillBase
{
	CharacterStatus _status;

	UnstoppableData _data;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<UnstoppableData>("Data/UnstoppableData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/HeavyStrikeData"); return; }

		Id = _data.Id;

		_status.HasteRatio += _data.SpeedUpRatio;
	}
}
