using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField][Range(0, 10)]
    int damageAmount = 1;
    [SerializeField]
    [Range(0f, 1f)]
    float attackCooldownDuration = 0.3f;
    [SerializeField]
    [Range(0.01f, 1f)]
    float damageStartDuration = 0.1f;
    [SerializeField][Range(0.01f, 1f)]
    float damageWindowDuration = 0.25f;
    [SerializeField] [Range(0f, 1f)]
    float attackDashRange = 1f;
    [SerializeField] [Range(0.01f, 10f)]
    float knockbackStrength = 0.1f;
    [SerializeField]
    Vector2 areaOfEffectPosition = new Vector2(0.09f,0f);
    [SerializeField]
    Vector2 areaOfEffectSize = new Vector2(0.21f,0.09f);
    [SerializeField]
    float areaOfEffectRange;
    [SerializeField]
    List<AudioClip> attackSounds = new List<AudioClip>();



    [Header("Data")]    /********/
    float attackCooldownTimer = 0f;
    float damageWindowTimer;
    float damageStartTimer;
    bool damageTrigger;
    List<GameObject> recentlyHit = new List<GameObject>();
    int attackSoundQueue = 0;



    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;
    [SerializeField]
    LayerMask enemyLayer;

    #endregion


    #region Base Methods

    void Start()
    {

    }
    void Update()
    {
        AttackCooldownTimer();
        DamageStartTimer();
        DamageArea();
    }



    #endregion


    #region Unique Methods

    public void ButtonDown()
    {
        Attack();
    }



    void Attack()
    {
          if (player.statusScript.ActionPossibleN() && attackCooldownTimer <= 0)
        {
            if(recentlyHit.Capacity>0)  recentlyHit.Clear();
            player.rb.AddForce(new Vector2(attackDashRange * transform.right.x, 0f), ForceMode2D.Impulse);
            player.statusScript.BeginCasting(1,attackCooldownDuration);
            attackCooldownTimer = attackCooldownDuration;
            damageStartTimer = damageStartDuration;
            player.anim.Play("attack", -1, 0f);
            damageTrigger = true;
            if (attackSoundQueue < attackSounds.Count-1) attackSoundQueue++;
            else attackSoundQueue = 0;
            player.PlaySound(0, attackSounds[attackSoundQueue]);


        }
    }

    //Time before player can attack again
    void AttackCooldownTimer()
    {
        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;
        else attackCooldownTimer = 0;
    }

    //Time in which boxcast will detect enemies that should get hurt
    void DamageStartTimer()
    {
        if (damageTrigger)
        {
            if (damageStartTimer > 0) damageStartTimer -= Time.deltaTime;
            else
            {
                damageStartTimer = 0;
                damageTrigger = false;
                damageWindowTimer = damageWindowDuration;
            }
        }
    }

    void DamageArea()
    {
        if (damageWindowTimer > 0 && player.statusScript.IsAlive())
        {
            damageWindowTimer -= Time.deltaTime;
            Vector2 offset = new Vector2(transform.position.x + (areaOfEffectPosition.x * transform.right.x), transform.position.y + areaOfEffectPosition.y);
            RaycastHit2D[] areaOfEffect = Physics2D.BoxCastAll(offset, areaOfEffectSize, 0, transform.right, areaOfEffectRange, enemyLayer);
            foreach (RaycastHit2D target in areaOfEffect)
            {
                //Add enemy hit to a list to prevent it from getting hit twice by the same attack
                if (!recentlyHit.Contains(target.collider.gameObject))
                {
                    EnemyStatus enemy = target.collider.GetComponent<EnemyStatus>();
                    enemy.ReceiveDamage(damageAmount, transform.right, knockbackStrength);
                    recentlyHit.Add(enemy.gameObject);
                }
            }
        }

    }


    /* Visualize attack area
    void OnDrawGizmosSelected()
    {
        Vector2 offset = new Vector2(transform.position.x + (areaOfEffectPosition.x*transform.right.x), transform.position.y + areaOfEffectPosition.y);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(offset, areaOfEffectSize);
    }

    */
    #endregion

}

