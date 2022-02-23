using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComboUI : MonoBehaviour
{

    #region Variables
    [Header("Settings")]    /********/
    [Range(0.5f, 4f)]
    float comboDuration=2.4f;
    [SerializeField] [Range(0.1f, 3f)]
    float disappearDuration = 1f;
    [SerializeField] AudioClip popUpSound;

    [Header("Data")]    /********/
    int comboValue=0;
    float comboTimer;
    bool comboActive = false;
    float disappearTimer;
    bool disappearing = false;

    [Header("Components")]    /********/
    [SerializeField] GameObject barObject;
    [SerializeField] GameObject totalCountObject;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] TextMeshProUGUI totalCount;
    [SerializeField] Image barImage;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aS;

    #endregion

    #region Base Methods

    void OnEnable()
    {
        EventManager.UpdateComboUI += AddComboCount;
    }

    void OnDisable()
    {
        EventManager.UpdateComboUI -= AddComboCount;
    }
    void Update()
    {
        ComboTimer();
        UpdateBarImage();
        DisappearTimer();
    }

    #endregion

    #region Unique Methods

    void AddComboCount()
    {
        comboValue++;
        comboTimer = comboDuration;
        comboActive = true;
        if (comboValue == 2 && !barObject.activeInHierarchy) barObject.SetActive(true);
        if (comboValue >= 2)
        {
            anim.Play("countUp", -1, 0);
            count.text = ("x" + comboValue);
        }
    }

   void ComboTimer()
    {
        if (comboActive)
        {
            if (comboTimer > 0) comboTimer -= Time.deltaTime;
            else
            {
                comboActive = false;
                comboTimer = 0;
                ComboDone();
            }
        }


    }

    void UpdateBarImage()
    {
        barImage.fillAmount = comboTimer / comboDuration;
    }

    void ComboDone()
    {
        if (barObject.activeInHierarchy) barObject.SetActive(false);
        if (comboValue >= 3)
        {
            totalCount.text = ("Combo x" + comboValue.ToString() +"!");
            totalCountObject.SetActive(true);
            disappearTimer = disappearDuration;
            disappearing = true;
            aS.clip = popUpSound;
            aS.Play();

        }
        comboValue = 0;

    }

   void DisappearTimer()
    {
        if (disappearing)
        {
            if (disappearTimer > 0) disappearTimer -= Time.deltaTime;
            else
            {
                disappearTimer = 0;
                disappearing = false;
                totalCountObject.SetActive(false); 
            }
        }
    }

    #endregion
}
