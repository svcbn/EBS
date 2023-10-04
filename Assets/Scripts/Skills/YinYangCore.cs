using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class YinYangCore : PassiveSkillBase
{
    private YinYangCoreData _data;

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

        Managers.Stat.onTakeDamage += Excute;

	}


	void Excute(int playerIndex)
    {
		if (Owner.playerIndex != playerIndex)
        {
            
        }

    }

  
    private void Update() {
        if( IsEnabled ) return;

        if( CurrentCooldown < Cooldown ){
            CurrentCooldown += Time.deltaTime;
            return;
        }

        EnableYinYangCore();

        //CurrentCooldown = 0;
    }


    private void EnableYinYangCore()
    {
        OnEffect();
        IsEnabled = true;

        Managers.Stat.isYinYangCore[Owner.playerIndex] = true;
    }

    
    public void DisableYinYangCore() // Call by statmanager Function after Skill Use.
    {
        CurrentCooldown = 0;

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
