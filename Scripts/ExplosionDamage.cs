using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    int damageValue = 1;
    [SerializeField]
    [Range(0.1f, 1f)]
    public float explosionDuration = 0.5f;
    [SerializeField]
    [Range(0.1f, 1f)]
    public float damageDelayDuration = 0.15f;
    [SerializeField]
    [Range(0.01f, 10f)]
    float damageKnockbackStrength = 1.49f;
    public bool explodeOnImpact;
    [SerializeField]
    [Range(0.1f, 1f)]
    public float disableDuration = 0.2f;
    [SerializeField] AudioClip explosionSound;



    [Header("Data")]    /********/
    float damageDelayTimer;
    float explosionTimer;
    bool exploding = false;
    bool delay = false;
    float disableTimer;
    bool disabling = false;

    Vector2 explosionOffset = new Vector2(0,0.0021f);
    Vector2 explosionSize = new Vector2(0.34f,0.34f);
    Vector2 objectOffset = new Vector2(0, 0.004f);
    Vector2 objectSize = new Vector2(0.08f, 0.07f);

    [Header("Components")]    /********/
    [SerializeField]
    Bomb bomb;
    [SerializeField]
    Animator anim;
    [SerializeField]
    CapsuleCollider2D capsuleCollider;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField] AudioSource aS;

    #endregion

    #region Base Methods
    void Update()
    {
        ExplosionTimer();
        DamageDelayTimer();
        DisableTimer();
    }
    #endregion

    #region Unique Methods

    void OnEnable()
    {
        disableTimer = 0f;
        disabling = false;
        explosionTimer = 0f;
        exploding = false;
        damageDelayTimer = 0;
        delay = false;
        capsuleCollider.offset = objectOffset;
        capsuleCollider.size = objectSize;

    }
    public void StartTimer()
    {
        damageDelayTimer = damageDelayDuration;
        delay = true;
        anim.Play("explosion");
        rb.drag = 5f;

    }

    void DamageDelayTimer()
    {
        if (delay)
        {
            if (damageDelayTimer > 0) damageDelayTimer -= Time.deltaTime;
            else
            {
                delay = false;
                explosionTimer = explosionDuration;
                exploding = true;
                damageDelayTimer = 0f;
                capsuleCollider.offset = explosionOffset;
                capsuleCollider.size = explosionSize;
                aS.clip = explosionSound;
                aS.Play();
            }
        }
    }

    void ExplosionTimer()
    {
        if (exploding)
        {
            if (explosionTimer > 0) explosionTimer -= Time.deltaTime;
            else
            {
                explosionTimer = 0f;
                exploding = false;
                disableTimer = disableDuration;
                disabling = true;
   }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (exploding && damageDelayTimer<=0 &&  player != null)
        {
            Vector2 knockbackDirection = player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
            player.ReceiveDamage(damageValue, knockbackDirection, damageKnockbackStrength);
        }
        else if (explodeOnImpact && !exploding && damageDelayTimer==0 && disableTimer<=0 && player != null)
        {
            Vector2 knockbackDirection = player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
            player.ReceiveDamage(damageValue, knockbackDirection, damageKnockbackStrength);
            bomb.BurnOff();
            OnEnable();
            StartTimer();
        }

    }

    void DisableTimer()
    {
        if (disabling)
        {
            if (disableTimer > 0) disableTimer -= Time.deltaTime;
            else
            {
                disabling = false;
                transform.parent.gameObject.SetActive(false);
            }
        }

    }


}

    #endregion

