using UnityEngine;

public class TriggerAttackerBonusShot : MonoBehaviour
{
    [Header("충돌 시 데미지를 주는 Attacker")]
    public bool isDestroyedOnCollision = false;
    public bool useKnockback = false;

    public Character owner;
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.GetComponent<Character>()             == null             ){ return; }
        if (collision.GetComponent<Character>().playerIndex == owner.playerIndex){ return; }
		
        if (isDestroyedOnCollision)
        {
            // TODO: 이전에 피격된 데미지(SManagers.Stat 값 가져오기?)의 20% 데미지
            damage = 1; // 임시 설정
            Managers.Stat.GiveDamage(1 - owner.playerIndex, damage);

            owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(1f, 30f, transform.position);

            Destroy(this.gameObject);
        }
    
    }
}
