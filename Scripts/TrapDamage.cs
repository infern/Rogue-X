using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{

    [SerializeField]
    [Range(1, 3)]
    int damageValue = 1;
    [Range(0.01f, 10f)]
    float damageKnockbackStrength = 1.49f;



    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (player != null)
        {
            Vector2 knockbackDirection = player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
            player.ReceiveDamage(damageValue, knockbackDirection, damageKnockbackStrength);

        }

    }

}
