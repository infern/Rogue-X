using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    #region Variables
    [Header("Settings")]    /********/
    [SerializeField] [Range(1, 3)] int damageAmount = 1;
    [SerializeField]
    [Range(0, 1)]
    float disappearDuration = 0.3f;
    [SerializeField]
    [Range(0.01f, 10f)]
    float knockbackStrength = 0.1f;
    [SerializeField] AudioClip impactSound;

    [Header("Data")]    /********/
    bool disappearing = false;
    float disappearTimer;

    [Header("Components")]    /********/
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource aS;

    #endregion

    #region Base Methods
    void OnEnable()
    {
        disappearing = false;
        anim.Play("flying");
    }


    void Update()
    {
        DisappearTimer();
    }

    #endregion

    #region Unique Methods

    void DisappearTimer()
    {
        if (disappearing)
        {
            if (disappearTimer > 0) disappearTimer -= Time.deltaTime;
            else
            {
                disappearing = false;
                disappearTimer = 0f;

                this.gameObject.SetActive(false);
            }
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyStatus enemy = collision.gameObject.GetComponent<EnemyStatus>();
        if (!disappearing && (enemy == null)) Explode();
        else if (enemy != null && enemy.IsAlive())
        {
            Explode();
            enemy.ReceiveDamage(damageAmount, transform.right, knockbackStrength);
        }

    }


    void Explode()
    {
        rb.velocity = Vector2.zero;
        disappearing = true;
        anim.Play("disappear");
        disappearTimer = disappearDuration;
        aS.clip = impactSound;
        aS.Play();
    }

    #endregion
}





