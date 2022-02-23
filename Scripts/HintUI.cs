using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintUI : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0.1f, 1f)]
    float deactivateDuration=0.4f;
    [SerializeField] AudioClip appearSound;
    [SerializeField] AudioClip disappearSound;

    [Header("Data")]    /********/
    bool active = false;
    float deactivateTimer;
    bool deactivating = false;




    [Header("Components")]    /********/
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    AudioSource aS;
    [SerializeField] TextMeshProUGUI tmp;



    #endregion

    void OnEnable()
    {
        EventManager.ToggleHint += ToggleImage;
    }

    void OnDisable()
    {
        EventManager.ToggleHint -= ToggleImage;
    }

     void Update()
    {
        DeactivateTimer();
    }


    #region Unique Methods

    void DeactivateTimer()
    {
        if (deactivating)
        {
            if (deactivateTimer > 0) deactivateTimer -= Time.deltaTime;
            else
            {
                deactivateTimer = 0;
                deactivating = false;
                panel.SetActive(false);

            }
        }
    }
    void ToggleImage(string hintText)
    {
        if (active)
        {
            deactivating = true;
            anim.Play("disappear");
            deactivateTimer = deactivateDuration;
            aS.clip = disappearSound;
        }
        else
        {
            panel.SetActive(true);
            deactivating = false;
            anim.Play("appear");
            aS.clip = appearSound;
            tmp.text = hintText;
        }

        active = !active;
        aS.Play();
    }

    #endregion
}
