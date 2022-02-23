using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField] [Range(0f, 10f)] float force = 3f;
    [SerializeField] [Range(0f, 3f)] float cooldownDuration;
    [SerializeField]
    Vector2 spawnPointOffset = Vector2.zero;
    [SerializeField]
    AudioClip castSound;
    [SerializeField]
    [Range(0.01f, 10f)]
    float knockbackStrength = 0.1f;

    [Header("Data")]    /********/
    float cooldownTimer;



    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;
    [SerializeField]
    GameObject projectile;

    #endregion


    #region Base Methods

    void Start()
    {

    }
    void Update()
    {
        CooldownTimer();
    }



    #endregion


    #region Unique Methods

    public void ButtonDown()
    {
        BeginCasting();
    }



    void BeginCasting()
    {
        if (player.statusScript.ActionPossibleM() && player.statusScript.EnergyCount()>0)
        {
            player.rb.AddForce(new Vector2(knockbackStrength * -transform.right.x, 0f), ForceMode2D.Impulse);
            player.statusScript.LoseEnergy();
            player.anim.Play("special");
            player.statusScript.BeginCasting(1, cooldownDuration);
            cooldownTimer = cooldownDuration;
            GameObject spawned = ObjectPool.SharedInstance.GetEnergyFromPool();
            spawned.transform.position = (Vector2)transform.position + spawnPointOffset;

            Quaternion rotation = player.transform.rotation;
            spawned.transform.rotation = rotation;
  
            spawned.SetActive(true);
            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
            Vector2 direction = transform.right * force;
            rb.AddForce(direction, ForceMode2D.Impulse);
            player.PlaySound(0, castSound);

        }
    }


    void CooldownTimer()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        else cooldownTimer = 0f;
    }



    #endregion
}
