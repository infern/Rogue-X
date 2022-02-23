using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] EnemyComponents enemy;
    [SerializeField] [Range(1, 3)]
    int damageValue=1;
    [Range(0.01f, 10f)]
    float damageKnockbackStrength = 1.49f;
    [Range(0.01f, 10f)]
    float selfKnockbackStrength = 0.209f;


    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (player != null && enemy.statusScript.IsAlive() && !enemy.statusScript.IsStunned())
        {

            Vector2 knockbackDirection = player.transform.position.x > enemy.statusScript.transform.position.x ? Vector2.right : Vector2.left;
            player.ReceiveDamage(damageValue,knockbackDirection,damageKnockbackStrength);
            enemy.rb.velocity = Vector2.zero;
            enemy.rb.AddForce((-knockbackDirection* selfKnockbackStrength), ForceMode2D.Impulse);

        }

    }

}
