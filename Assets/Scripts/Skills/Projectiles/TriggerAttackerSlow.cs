using UnityEngine;

public class TriggerAttackerSlow : MonoBehaviour
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

            // 슬로우 효과
            collision.gameObject.GetComponent<CharacterStatus>().SetSlowEffect(1.5f, 0.9f);

            Destroy(this.gameObject);
        }
    }
}
