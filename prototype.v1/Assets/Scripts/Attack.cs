using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knokback = Vector2.zero;
    public string targetTag = "Enemy";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                bool gotHit = damageable.Hit(attackDamage, knokback); 
                if(gotHit)
                    Debug.Log($"Player hit! Damage: {attackDamage}");
            }
        }
    }
}
