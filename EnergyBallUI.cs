using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallUI : MonoBehaviour
{
    #region Variables

    [Header("Settings")]    /********/
    [SerializeField]
    [Range(0.1f, 1f)]
    public float disappearDuration = 0.3f;
    [SerializeField] AudioClip gainSound;

    [Header("Data")]    /********/
    [SerializeField]
    List<bool> energy = new List<bool>();
    int energyTotal = 3;



    [Header("Components")]    /********/
    [SerializeField]
    List<Animator> anims = new List<Animator>();
    [SerializeField]
    AudioSource aS;


    #endregion

    void OnEnable()
    {
        EventManager.UpdateEnergyUI += UpdateEnergyImage;
    }

    void OnDisable()
    {
        EventManager.UpdateEnergyUI -= UpdateEnergyImage;
    }


    void Start()
    {
        energyTotal = energy.Count;
    }

    #region Unique Methods

     void UpdateEnergyImage(int value)
    {
        value--;
        for (int i = 0; i < energyTotal; i++)
        {
            if (energy[i] == false && i<=value)
            {
                energy[i] = true;
                anims[i].Play("gain");
                aS.clip = gainSound;
                aS.Play();
            }
            
            else if(energy[i]==true && i > value)
            {
                energy[i] = false;
                anims[i].Play("lose");

            }
        }
    }

    #endregion
}
