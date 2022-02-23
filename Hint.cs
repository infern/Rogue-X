using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField] string text;

    [Header("Data")]    /********/
    bool active = false;



    #endregion


    #region Base Methods


    #endregion


    #region Unique Methods



     void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (player!=null && player.IsAlive() && !active)
        {
            active = true;
            EventManager.Hint(text);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStatus player = collision.GetComponent<PlayerStatus>();
        if (player != null && player.IsAlive() && active)
        {
            active = false;
            EventManager.Hint(text);
        }
    }


    #endregion
}
