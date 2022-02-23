using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]   [Range(1f, 22f)]
    float accelerationSpeed = 4f;
    [SerializeField] [Range(0.1f, 22f)] float maxSpeed = 1.3f;

    [Header("Data")]    /********/
    [HideInInspector]
    public Vector2 direction;
    Vector2 savedDirection;
    bool movingRight = true;


    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;

    #endregion


    #region Base Methods

    void Start()
    {

    }
    void Update()
    {
        Flip();
    }

    void FixedUpdate()
    {
        Run();
    }

    #endregion


    #region Unique Methods

    void Run()
    {
        if(player.statusScript.ActionPossibleM())
        {
            if (direction != Vector2.zero)
            {
                savedDirection = direction.normalized;
                player.anim.SetBool("running", true);
            }
            else player.anim.SetBool("running", false);

            Vector2 move = direction * accelerationSpeed;
            bool movingAtMaxSpeed = (player.rb.velocity.x >= maxSpeed);
            if (!movingAtMaxSpeed) player.rb.AddForce(new Vector2(move.x, player.rb.velocity.y), ForceMode2D.Force);
        }

    }

    void Flip()
    {
        bool movesAwayFromFacing = (direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight);
        if (player.statusScript.ActionPossibleM() && movesAwayFromFacing)
        {
            float rotation = movingRight ? 180f : 0f;
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            movingRight = !movingRight;
        }
    }
}

#endregion

