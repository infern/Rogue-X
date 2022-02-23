using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class CoinUI : MonoBehaviour
{
    #region Variables
    [Header("Settings")]    /********/
    [SerializeField]
    List<AudioClip> pickUpSounds = new List<AudioClip>();

    [Header("Data")]    /********/
    public int collectedCoins = 0;
    int pickUpSoundQueue = 0;

    [Header("Components")]    /********/
    [SerializeField]
    TextMeshProUGUI count;
    [SerializeField]
    Animator anim;
    [SerializeField] AudioSource aS;

    #endregion

    void OnEnable()
    {
        EventManager.UpdateCoinUI += PickUp;
    }

    void OnDisable()
    {
        EventManager.UpdateCoinUI -= PickUp;
    }

    #region Unique Methods
     void PickUp()
    {
        anim.Play("pickUp", -1, 0f);
        collectedCoins++;
        count.text = collectedCoins.ToString();
        if (pickUpSoundQueue < pickUpSounds.Count-1) pickUpSoundQueue++;
        else pickUpSoundQueue = 0;
        aS.clip= pickUpSounds[pickUpSoundQueue];
        aS.Play();
    }

    #endregion
}
