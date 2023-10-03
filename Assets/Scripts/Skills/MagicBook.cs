using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class MagicBook : PassiveSkillBase
{

    private MagicBookData _data;

    private static bool isRegistered = false;

	public override void Init()
	{
		_data = Managers.Resource.Load<MagicBookData>("Data/MagicBookData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/MagicBookData"); return; }

		Id               = _data.Id;
		Cooldown         = _data.Cooldown;
        HasPresentNumber = _data.HasPresentNumber; // 사용 횟수 없음

        IsEnabled = true;


        if( !isRegistered )
        {
            if(Owner == null){ Debug.LogError( "Owner is null" ); return; }

            isRegistered = true;
            Owner.OnActiveSkillExcute += OnActiveSkillExecute;
        }
	}

    // Do Something
    void OnActiveSkillExecute()
    {
        if(_data.Effect != null){
            GameObject enemy = Owner.GetTarget();
            PlayEffect(_data.Effect.name, _data.effectDuration, enemy.transform, Vector3.zero );
        }

		Managers.Stat.GiveDamage(1 - Owner.playerIndex, _data.damage);
    }

}
