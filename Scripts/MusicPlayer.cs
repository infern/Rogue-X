using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource aS;

    void Awake()
    {
        aS.time = PlayerPrefs.GetFloat("musicTime", 0);
    }




}
