using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackerSlow : MonoBehaviour
{
    [Header("충돌 시 데미지를 주는 Attacker")]
    public bool isDestroyedOnCollision = false;
    public bool useKnockback = false;

    public Character owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if(owner == collision.GetComponent<Character>())
        {
            return;
        }

        //설정한 damageLayer와 충돌체의 레이어가 같으면 피해를 줍니다.
        if( collision.gameObject.layer == LayerMask.NameToLayer("EnemyMJ"))
        {
            //DamageToTarget(collision.gameObject);
            if (isDestroyedOnCollision)
            {
                // 피격 판정.

                // TODO: 데미지 추가

                // 슬로우 효과
                collision.gameObject.GetComponent<CharacterStatus>().SetSlowEffect(5f, 0.5f);

                Destroy(this.gameObject);
            }
        }
    }
}
