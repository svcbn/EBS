using UnityEngine;

public class TriggerAttacker : MonoBehaviour
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
            Managers.Stat.GiveDamage(1 - owner.playerIndex, damage);

            owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(1f, 30f, transform.position);

            Destroy(this.gameObject);
        }
    }
}
