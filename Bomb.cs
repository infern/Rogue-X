using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0.1f, 3f)]
    float burnDuration = 0.5f;
    [SerializeField]
    [Range(0.1f, 3f)]
    public float pulsingDuration = 0.3f;






    [Header("Data")]    /********/
    float burnTimer;
    float pulsingTimer;
    bool burning = false;
    bool pulsing = false;





    [Header("Components")]    /********/
    [SerializeField]
    Animator anim;
    [SerializeField] ExplosionDamage explosion;
    [SerializeField] Rigidbody2D rb;



    #endregion

    #region Unique Methods

    void OnEnable()
    {
        pulsing = false;
        pulsingTimer = 0f;
        rb.drag = 1f;
        burnTimer = burnDuration;
        burning = true;
        anim.Play("idle");

    }

    void Update()
    {
        BurnTimer();
        PulsingTimer();
    }

    #endregion

    #region Unique Methods

    void BurnTimer()
    {
        if (burning)
        {
            if (burnTimer > 0) burnTimer -= Time.deltaTime;
            else
            {
                burnTimer = 0;
                burning = false;
                pulsingTimer = pulsingDuration;
                pulsing = true;
                anim.Play("pulsing");

            }
        }

    }

    void PulsingTimer()
    {
        if (pulsing)
        {
            if (pulsingTimer > 0) pulsingTimer -= Time.deltaTime;
            else
            {
                pulsingTimer = 0;
                pulsing = false;
                explosion.StartTimer();
            }
        }
    }


    public void BurnOff()
    {
        burning = false;
    }





    #endregion
}
