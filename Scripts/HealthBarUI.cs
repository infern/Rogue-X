using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField] [Range(0.1f, 1f)]
    public float disappearDuration = 0.3f;
    [SerializeField] AudioClip gainSound;

    [Header("Data")]    /********/
    [SerializeField]
    List<bool> hearts = new List<bool>();
    int healthTotal = 3;



    [Header("Components")]    /********/
    [SerializeField]
    List<Animator> anims = new List<Animator>();
    [SerializeField]
    AudioSource aS;




    #endregion

    void OnEnable()
    {
        EventManager.UpdateHealthUI += UpdateHealthImage;
    }

    void OnDisable()
    {
        EventManager.UpdateHealthUI -= UpdateHealthImage;
    }

    void Start()
    {
        healthTotal = hearts.Count;
    }

    #region Unique Methods

     void UpdateHealthImage(int value)
    {
        value--;
        for (int i = 0; i < healthTotal; i++)
        {
            if (hearts[i] == false && i <= value)
            {
                hearts[i] = true;
                anims[i].Play("gain");
                aS.clip = gainSound;
                aS.Play();
            }

            else if (hearts[i] == true && i > value)
            {
                hearts[i] = false;
                anims[i].Play("lose");
            }
        }
    }

    #endregion
}
