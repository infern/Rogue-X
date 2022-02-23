using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField] [Range(0, 10)]
    int maxHealth = 3;
    [SerializeField] [Range(0, 2)]
    float dyingDuration = 0.4f;
    [SerializeField] [Range(0, 1)]
    float stunDuration = 0.4f;
    [SerializeField]
    [Range(0, 2)]float invulnerableDuration = 0.4f;
    [SerializeField] [Range(0f, 2f)]
    float groundRayRange = 0.2f;
    [SerializeField] [Range(0f, 1f)]
    float groundedGracePeriodDuration = 0.175f;
    [SerializeField]
    [Range(0, 3)]
    int maxEnergy = 3;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip landSound;



    [Header("Data")]    /********/
    [SerializeField]
    int currentHealth;
    bool alive = true;
    public bool grounded;
    public float groundedGracePeriodTimer;
    bool groundedGracePeriodActive;
    float dyingTimer;
    float stunTimer;
    float castingNTimer;
    float castingMTimer;
    bool landed = false;
    int currentEnergy=0;
    float invulnerableTimer;


    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] HealthBarUI healthBarUI;

    #endregion


    #region Base Methods

    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        GroundCheck();
        GroundedGracePeriod();
        DyingTimer();
        StunTimer();
        CastingNoMoveTimer();
        CastingMoveTimer();
        Land();
        InvulnerableTimer();
    }

    #endregion


    #region Ground Detection
    void GroundCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(player.boxCollider.bounds.center, player.boxCollider.bounds.size, 0f, Vector2.down, groundRayRange, groundLayer);
        if (raycastHit.collider != null)
        {
            grounded = true;
            groundedGracePeriodActive = false;
        }
        else
        {
            landed = false;
            grounded = false;
        }

        if (!grounded && !groundedGracePeriodActive)
        {
            groundedGracePeriodActive = true;
            groundedGracePeriodTimer = groundedGracePeriodDuration;
        }
        player.anim.SetBool("grounded", grounded);
    }

    //Timer that allows player to jump if he fell of the platform and pressed the button a moment too late
    void GroundedGracePeriod()
    {
        if (groundedGracePeriodActive)
        {
            if (groundedGracePeriodTimer > 0) groundedGracePeriodTimer -= Time.deltaTime;
            else
            {
                groundedGracePeriodTimer = 0;
            }
        }
    }

    void Land()
    {
        if (!landed && grounded && IsAlive())
        {
            landed = true;
            player.jumpScript.DisableDoubleJump();
            player.PlaySound(1, landSound);

        }
    }

    #endregion

    #region Combat
    public void ReceiveDamage(int value, Vector2 knockbackDirection, float knockBackStrength)
    {
        if (invulnerableTimer <= 0 && dyingTimer == 0)
        {
            if (currentHealth > 0)
            {
                castingNTimer = 0f;
                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(new Vector2(knockbackDirection.x * knockBackStrength, knockBackStrength * 0.5f), ForceMode2D.Impulse);
            }
            currentHealth -= value;
            EventManager.HealthChange(currentHealth);
            if (currentHealth > 0)
            {
                stunTimer = stunDuration;
                player.sharedAnim.Play("hurt");
                player.PlaySound(1, hurtSound);
                invulnerableTimer = invulnerableDuration;

            }
            else
            {
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
        player.PlaySound(1, deathSound);
        player.anim.Play("death");
        dyingTimer = dyingDuration;
        alive = false;
    }

    void DyingTimer()
    {
        if (!alive)
        {
            if (dyingTimer > 0) dyingTimer -= Time.deltaTime;
            else
            {
                GameController.Instance.GameOver();
            }
        }
    }

    void InvulnerableTimer()
    {
        if (invulnerableTimer > 0) invulnerableTimer -= Time.deltaTime;
        else invulnerableTimer = 0;
    }
    #endregion


    #region Casting
    public void BeginCasting(int move, float duration)
    {
        if (move == 0) castingNTimer = duration;
        else castingMTimer = duration;

    }
    void CastingNoMoveTimer()
    {
        if (castingNTimer > 0) castingNTimer -= Time.deltaTime;
        else castingNTimer = 0;
    }

    void CastingMoveTimer()
    {
        if (castingMTimer > 0) castingMTimer -= Time.deltaTime;
        else castingMTimer = 0;
    }


    public bool ActionPossibleN()
    {
        if (stunTimer <= 0 && currentHealth > 0 && castingNTimer <= 0) return true;
        else return false;
    }

    public bool ActionPossibleM()
    {
        if (stunTimer <= 0 && currentHealth > 0 && castingMTimer <= 0) return true;
        else return false;
    }
    #endregion

    #region Status
    public int HealthCount()
    {
        return currentHealth;
    }
    public int EnergyCount()
    {
        return currentEnergy;
    }
  
        public bool IsAlive()
    {
        if (currentHealth > 0) return true;
        else return false;
    }
    public void LoseEnergy()
    {
        if (currentEnergy>0)
        {
            currentEnergy--;
            EventManager.EnergyChange(currentEnergy);
        }
    }
    public void GainEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy++;
            EventManager.EnergyChange(currentEnergy);
        }
    }

    public void GainHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            EventManager.HealthChange(currentHealth);

        }
    }
    #endregion 

}





