using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstoppable : PassiveSkillBase
{
	Character _character;
	CharacterStatus _status;

	UnstoppableData _data;

	public override void Init()
	{
		_character = GetComponent<Character>();
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<UnstoppableData>("Data/UnstoppableData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/HeavyStrikeData"); return; }

		Id = _data.Id;

		_status.HastRatio += _data.SpeedUpRatio;
	}
}
