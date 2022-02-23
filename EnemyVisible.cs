using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisible : MonoBehaviour
{
    [SerializeField] EnemyPatrol enemy;
    void OnBecameVisible()
    {
        enemy.visible = true;
    }
}
