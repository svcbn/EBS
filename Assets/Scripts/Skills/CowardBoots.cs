using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardBoots : PassiveSkillBase
{
	CharacterStatus _status;

	CowardBootsData _data;

	private Coroutine _hasteCR;

	private float _activeTime;

	public override void Init()
	{
		_status = GetComponent<CharacterStatus>();

		_data = Managers.Resource.Load<CowardBootsData>("Data/CowardBootsData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/CowardBootsData"); return; }

		Id = _data.Id;

		Managers.Stat.onTakeDamage += Excute;
	}

	void Excute(int playerIndex)
	{
		if (Owner.playerIndex == playerIndex)
		{
			_activeTime = 0;

			if (_hasteCR == null)
				_hasteCR = StartCoroutine(nameof(HasteEffect));
		}
	}

	IEnumerator HasteEffect()
	{
		_status.HasteRatio += _data.SpeedUpRatio;

		while (_activeTime < _data.EffectDuration)
		{
			_activeTime += Time.deltaTime;
			yield return null;
		}

		_status.HasteRatio -= _data.SpeedUpRatio;
		_hasteCR = null;
	}
}
