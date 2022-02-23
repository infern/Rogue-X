using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponents : MonoBehaviour
{
    #region Variables


    [Header("Components")]    /********/
    public EnemyStatus statusScript;
    public EnemyPatrol patrolScript;
    public EnemyThrow throwScript;

    public Rigidbody2D rb;
    public Animator anim;
    public Animator sharedAnim;
    public AudioSource aS1;
    public AudioSource aS2;


    #endregion


    #region Sound

    public void PlaySound(int source, AudioClip clip)
    {
        if (source == 0)
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
