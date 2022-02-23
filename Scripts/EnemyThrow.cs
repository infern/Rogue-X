using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrow : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0, 8)]
    float detectRange = 1.89f;
    [SerializeField]
    [Range(0.5f, 8f)]
    float cooldownDuration = 4f;
    [SerializeField]
    [Range(0f, 1f)]
    float throwDelayDuration = 0.25f;
    [SerializeField]
    Vector2 spawnPointOffset = Vector2.zero;
    [SerializeField]
    [Range(0f, 20f)]
    float force = 3f;
    [SerializeField] AudioClip throwSound;


    [Header("Data")]    /********/
    GameObject trackedTarget;
    [SerializeField]
    bool targetInRange = true;
    float cooldownTimer;
    float throwDelayTimer;
    bool throwing = false;
    [SerializeField] Vector2 startForce = new Vector2(0.06f, 5.58f);
    bool movingRight = true;

    [Header("Components")]    /********/
    [SerializeField]
    EnemyComponents enemy;
    [SerializeField]
    GameObject projectile;


    #endregion


    #region Base Methods

    void Start()
    {
        trackedTarget = GameController.Instance.player;
    }
    void Update()
    {
        CheckIfTargetIsInRange();
        CooldownTimer();
        ThrowDelayTimer();
        Flip();
    }

    #endregion


    #region Unique Methods

    void CheckIfTargetIsInRange()
    {
        if (trackedTarget != null)
        {
            if (Vector2.Distance(transform.position, trackedTarget.transform.position) < detectRange) targetInRange = true;
            else targetInRange = false;
        }
    }
    void Flip()
    {
        bool lookingTowardsTarget = (transform.position.x < trackedTarget.transform.position.x && !movingRight) || (transform.position.x > trackedTarget.transform.position.x && movingRight);
        if (enemy.statusScript.IsAlive() &&lookingTowardsTarget && targetInRange)
        {
            float rotation = movingRight ? 180f : 0f;
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            movingRight = !movingRight;
        }
    }

    void CooldownTimer()
    {
        if (!throwing && targetInRange)
        {
            if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
            else
            {
                cooldownTimer = 0;
                BeginThrowing();
            }
        }

    }

    void BeginThrowing()
    {
        if (enemy.statusScript.CanMove())
        {
            throwing = true;
            cooldownTimer = cooldownDuration;
            throwDelayTimer = throwDelayDuration;
            enemy.anim.Play("throw");
            enemy.PlaySound(1, throwSound);
        }

    }

    void ThrowDelayTimer()
    {
        if (throwing)
        {
            if (throwDelayTimer > 0) throwDelayTimer -= Time.deltaTime;
            else
            {
                throwDelayTimer = 0f;
                CreateProjectile();
            }
        }


        void CreateProjectile()
        {
            if (enemy.statusScript.CanMove())
            {
                throwing = false;
                GameObject spawned = ObjectPool.SharedInstance.GetBombFromPool();
                spawned.transform.position = (Vector2)transform.position + spawnPointOffset;
                spawned.SetActive(true);
                //GameObject spawned = Instantiate(projectile, (Vector2)transform.position + spawnPointOffset, Quaternion.identity);
                Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
                //Vector2 direction = trackedTarget.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
                Vector2 direction = (trackedTarget.transform.position - transform.position).normalized;
                rb.AddForce(startForce, ForceMode2D.Impulse);
                rb.AddForce(direction * (force * Vector2.Distance(trackedTarget.transform.position, transform.position)), ForceMode2D.Impulse);
            }
        }
    }
    #endregion
}

