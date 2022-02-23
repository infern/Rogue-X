using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerComponents : MonoBehaviour
{
    #region Variables


    [Header("Components")]    /********/
    public PlayerStatus statusScript;
    public PlayerReadInput inputScript;
    public PlayerMovement movementScript;
    public PlayerJump jumpScript;
    public PlayerAttack attackScript;
    public PlayerSpecial specialScript;

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public Animator anim;
    public Animator sharedAnim;
    public AudioSource aS1;
    public AudioSource aS2;

    private InputMap keyMap;

    #endregion


    #region Sound

    public void PlaySound(int source, AudioClip clip)
    {
        if (source==0)
        {
            aS1.clip = clip;
            aS1.Play();
        }
        else
        {
            aS2.clip = clip;
            aS2.Play();
        }
    }

    #endregion



}
