using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class YinYangCore : PassiveSkillBase
{
    private YinYangCoreData _data;

    float CooldownTimer;

    bool effectOn;
    GameObject effect;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<YinYangCoreData>("Data/YinYangCoreData");
		if (_data == null) { Debug.LogWarning($"Fail load Data/YinYangCoreData"); return; }

		Id               = _data.Id;
		Cooldown         = _data.Cooldown;         // 15초 
        HasPresentNumber = _data.HasPresentNumber; // false: 사용 횟수 없음

        EnableYinYangCore();
	}

  
    private void Update() {
        if( IsEnabled ) return;

        if( CooldownTimer > 0 ){
            CooldownTimer -= Time.deltaTime;
            return;
        }

        EnableYinYangCore();

        CooldownTimer = Cooldown;
    }

    private void EnableYinYangCore()
    {
        OnEffect();
        IsEnabled = true;

        // TODO: Call statmanager Function to double damage
    }

    
    private void DisableYinYangCore() // TODO: Call by statmanager Function after Skill Use.
    {
        OffEffect();
        IsEnabled = false;
    }


    private void OnEffect()
    {
        if(_data.Effect == null){ return; }

        effectOn = true;

        effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name, Owner.transform ); // paraent를 character.gameObject로
        
        if(effect){
            effect.transform.localPosition = Vector3.zero;
        }else{
            Debug.LogError($"effect is null. effName :{_data.Effect.name}");
        }

        effect.transform.localPosition += new Vector3( 0f, 0.5f, 0f); // offset
        effect.transform.localScale     = new Vector3( 0.3f, 0.3f, 0.3f ); // 크기 30%
            
    }
    private void OffEffect()
    {
        if(effectOn){
            effectOn = false;
            Managers.Resource.Release(effect);
        }
    }
}
