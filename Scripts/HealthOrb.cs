using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0.1f, 1f)]
    public float disappearDuration = 0.3f;

    [Header("Data")]    /********/
    bool pickedUp = false;


    [Header("Components")]    /********/
    [SerializeField]
    Animator anim;


    #endregion

    #region Base Methods



    #endregion

    #region Unique Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (!pickedUp && player != null && player.HealthCount() < 3 && player.IsAlive())
        {
            player.GainHealth();
            PickUp();
        }
    }


    public void PickUp()
    {
        pickedUp = true;
        anim.Play("disappear");
        Destroy(this.gameObject, disappearDuration);

    }
    #endregion 

}
