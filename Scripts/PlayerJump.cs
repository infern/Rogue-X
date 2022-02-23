using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField] [Range(0.05f, 5f)]
    float lowJumpForce = 1.07f;
    [SerializeField][Range(0f, 1f)]
    float jumpGracePeriodDuration = 0.265f;
    [SerializeField] [Range(0f, 1f)]
    float jumpCooldownDuration = 0.368f;
    [SerializeField] [Range(0f, 1f)]
    float velocityXslow = 0.7f;
    [SerializeField] [Range(0f, 5f)]
    float highJumpForce = 0.38f;
    [SerializeField] [Range(0f, 1f)]
    float highJumpMaxDuration = 0.14f;
    [SerializeField] [Range(0.05f, 5f)]
    float doubleJumpForce = 3f;
    [SerializeField] [Range(0f, 1f)]
    float doubleJumpCastingDuration = 0f;
    [SerializeField]
    AudioClip jumpSound;
    [SerializeField]
    AudioClip doubleJumpSound;



    [Header("Data")]    /********/
    [SerializeField][Range(0.05f, 2f)]
    float highJumpMaxTimer = 0;
    bool buttonHeld = false;
    [SerializeField]
    public float jumpGracePeriodTimer;
    bool jumpTrigger = false;
    [SerializeField]
    float jumpCooldownTimer;
    bool doubleJumpAvailable = false;

    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;

    #endregion


    #region Base Methods

    void Update()
    {
        player.anim.SetFloat("velocityY", player.rb.velocity.y);
        JumpCooldownTimer();
        HighJumpTimer();
        JumpGracePeriod();
        LowJump();

    }

    void FixedUpdate()
    {
        HighJumpForce();
    }
    #endregion


    #region Unique Methods

    public void ButtonDown()
    {
        if (!player.statusScript.grounded)
        {
            if (!doubleJumpAvailable) jumpGracePeriodTimer = jumpGracePeriodDuration;
            else DoubleJump();
        }
        else jumpTrigger = true;
    }

    public void ButtonHeld()
    {
        buttonHeld = true;
    }

    public void ButtonUp()
    {
        highJumpMaxTimer = 0f;
        buttonHeld = false;
    }

    void LowJump()
    {
        bool grounded = (player.statusScript.grounded || player.statusScript.groundedGracePeriodTimer > 0);
        bool jumpAvailable = jumpTrigger || (jumpGracePeriodTimer > 0 && jumpCooldownTimer <= 0);
            if ((player.statusScript.ActionPossibleM() && jumpAvailable) && grounded )
        {
            jumpCooldownTimer = jumpCooldownDuration;
            doubleJumpAvailable = true;
            jumpTrigger = false;
            player.statusScript.groundedGracePeriodTimer = 0f;
            player.anim.Play("jump");
            player.rb.velocity = new Vector2(player.rb.velocity.x*velocityXslow, lowJumpForce);
            highJumpMaxTimer = highJumpMaxDuration;
            player.PlaySound(0, jumpSound);
        }
    }

    void HighJumpTimer()
    {
        if (buttonHeld)
        {
            if (highJumpMaxTimer > 0) highJumpMaxTimer -= Time.deltaTime;
            else highJumpMaxTimer = 0f;
        }
    }

    void HighJumpForce()
    {
        if (highJumpMaxTimer > 0 && buttonHeld)
        {
            player.rb.AddForce(new Vector2(0f, highJumpForce), ForceMode2D.Impulse);
        }
    }

    //This timer allows player to jump if he pressed button right before getting grounded
    void JumpGracePeriod()
    {
        if (jumpGracePeriodTimer > 0) jumpGracePeriodTimer -= Time.deltaTime;
        else jumpGracePeriodTimer = 0;
    }

    void JumpCooldownTimer()
    {
        if (jumpCooldownTimer > 0) jumpCooldownTimer -= Time.deltaTime;
        else jumpCooldownTimer = 0;
    }


    void DoubleJump()
    {
        if ((player.statusScript.ActionPossibleM())){
            player.statusScript.BeginCasting(0, doubleJumpCastingDuration);
            doubleJumpAvailable = false;
            highJumpMaxTimer = 0f;
            player.anim.Play("doubleJump");
            player.rb.velocity = new Vector2(player.rb.velocity.x * velocityXslow, doubleJumpForce);
            player.PlaySound(0, doubleJumpSound);

        }

    }

    public void DisableDoubleJump()
    {
        doubleJumpAvailable = false;
    }

}





#endregion

