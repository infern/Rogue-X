using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    bool stationary = false;
    [SerializeField]  [Range(0.1f, 2f)]
    float accelerationSpeed = 1.051f;
    [SerializeField] [Range(0.1f, 3f)] float maxSpeed=1.3f;
    [SerializeField]
    bool patrolToPoint = false;
    [SerializeField]
    bool fallOffPlatforms = false;
    [SerializeField]
    Vector2 wallCheckOffset = new Vector2(0.03f,0f);
    [SerializeField]
    float wallCheckRange=0.09f;
    [SerializeField]
    Vector2 holeCheckOffset= new Vector2(0.03f,0f);
    [SerializeField]
    float holeCheckRange=0.13f;





    [Header("Data")]    /********/
    Vector2 patrolStart;
    Vector2 patrolEnd;
    bool movingRight = true;
    public bool visible = false;


    [Header("Components")]    /********/
    [SerializeField]
    EnemyComponents enemy;
    [SerializeField] Transform patrolPoint;
    [SerializeField]
    LayerMask groundLayer;


    #endregion



    #region Base Methods

    void Start()
    {
        StartingRotation();
    }
    void Update()
    {
        ReachedPointCheck();
        WallCheck();
        HoleCheck();
    }
    void FixedUpdate()
    {
        Patrol();
    }


    #endregion


    #region Unique Methods


    void Patrol()
    {
        bool movingAtMaxSpeed = (enemy.rb.velocity.magnitude >= maxSpeed);
        if (visible && !stationary &&enemy.statusScript.IsAlive() && !enemy.statusScript.IsStunned() && !movingAtMaxSpeed)
        {
            float move = transform.right.x * accelerationSpeed;
            enemy.rb.AddForce(new Vector2(move, enemy.rb.velocity.y), ForceMode2D.Force);

        }
    }

    void StartingRotation()
    {
        if (patrolToPoint && !stationary)
        {
            patrolStart = transform.position;
            patrolEnd = patrolPoint.position;
            if (patrolStart.x > patrolEnd.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                movingRight = false;
            }
        }
        if(stationary) enemy.anim.SetBool("running", false);

    }

    void ReachedPointCheck()
    {
        if (patrolToPoint && ((transform.position.x > patrolEnd.x && movingRight) || (transform.position.x < patrolEnd.x && !movingRight))) Flip();
    }

    void Flip()
    {
        Vector2 tempPosition = patrolStart;
        patrolStart = patrolEnd;
        patrolEnd = tempPosition;
        float rotation = movingRight ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        movingRight = !movingRight;
    }

    void WallCheck()
    {
        Vector2 position = (Vector2)transform.position + wallCheckOffset * transform.right.x;
        RaycastHit2D wallcheck = Physics2D.Raycast(position, transform.right,wallCheckRange  ,groundLayer);
        if (wallcheck.collider != null) Flip();
     
    }

    void HoleCheck()
    {
        if (!fallOffPlatforms)
        {
            Vector2 position = (Vector2)transform.position + holeCheckOffset * transform.right.x;
            RaycastHit2D holeCheck = Physics2D.Raycast(position, Vector2.down, holeCheckRange, groundLayer);
            if (holeCheck.collider == null) Flip();
        }
    }

    void OnDrawGizmosSelected()
    {
        //Wall ray
        //Gizmos.color = Color.blue;
       // Gizmos.DrawRay((Vector2)transform.position + wallCheckOffset, transform.right * wallCheckRange);

        //Pit Ray
        Gizmos.color = Color.blue;
            Gizmos.DrawRay((Vector2)transform.position + holeCheckOffset * transform.right.x, Vector3.down * holeCheckRange);
    }

    #endregion

}


