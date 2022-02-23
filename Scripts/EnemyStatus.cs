using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0, 10)]
    int maxHealth = 2;
    [SerializeField]
    [Range(0, 2)]
    float dyingDuration = 0.4f;
    [SerializeField]
    [Range(0, 1)]
    float stunDuration = 0.4f;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;


    [Header("Data")]    /********/
    bool alive = true;
    int currentHealth;
    float dyingTimer;
    float stunTimer;


    [Header("Components")]    /********/
    [SerializeField]
    EnemyComponents enemy;


    #endregion


    #region Base Methods

    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        StunTimer();
        DyingTimer();
    }

    #endregion


    #region Unique Methods

    public void ReceiveDamage(int value, Vector2 knockbackDirection, float knockBackStrength)
    {
        if (alive)
        {
            currentHealth -= value;
            if (currentHealth > 0)
            {
                stunTimer = stunDuration;
                enemy.sharedAnim.Play("hurt");
                    enemy.rb.velocity = Vector2.zero;
                    enemy.rb.AddForce(new Vector2(knockbackDirection.x * knockBackStrength, enemy.rb.velocity.y), ForceMode2D.Impulse);
                enemy.PlaySound(0, hurtSound);

            }
            else
            {
                    enemy.rb.velocity = Vector2.zero;
                    enemy.rb.AddForce(new Vector2(knockbackDirection.x * knockBackStrength * 0.7f, enemy.rb.velocity.y), ForceMode2D.Impulse);
                Death();
            }
        }
    }

    void StunTimer()
    {
        if (stunTimer > 0) stunTimer -= Time.deltaTime;
        else stunTimer = 0;
    }

    void Death()
    {
        enemy.PlaySound(1, deathSound);
        enemy.anim.Play("death");
        dyingTimer = dyingDuration;
        alive = false;
        EventManager.EnemyKilled();
    }

    void DyingTimer()
    {
        if (!alive)
        {
            if (dyingTimer > 0) dyingTimer -= Time.deltaTime;
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public bool IsStunned()
    {
        if (stunTimer > 0) return true;
        else return false;
    }

    public bool IsAlive()
    {
        if (currentHealth >0) return true;
        else return false;
    }

    public bool CanMove()
    {
        if (!IsStunned() && IsAlive()) return true;
        else return false;
    }
    #endregion
}
