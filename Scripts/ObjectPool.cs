using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> bombPool;
    public GameObject bombsToPool;
    public int amountOfBombs;

    public List<GameObject> energyPool;
    public GameObject energyToPool;
    public int amountOEnergy;

    void Awake()
    {
        SharedInstance = this;
    }

     void Start()
    {
        bombPool = new List<GameObject>();
        GameObject tmp;
        for(int i=0; i< amountOfBombs; i++)
        {
            tmp = Instantiate(bombsToPool);
            tmp.SetActive(false);
            bombPool.Add(tmp);
        }

        energyPool = new List<GameObject>();
        for (int i = 0; i < amountOEnergy; i++)
        {
            tmp = Instantiate(energyToPool);
            tmp.SetActive(false);
            energyPool.Add(tmp);
        }

    }

    public GameObject GetBombFromPool()
    {
        for (int i = 0;  i < amountOfBombs; i++)
        {
            if (!bombPool[i].activeInHierarchy) return bombPool[i];
        }
        return null;
    }

    public GameObject GetEnergyFromPool()
    {
        for (int i = 0; i < amountOEnergy; i++)
        {
            if (!energyPool[i].activeInHierarchy) return energyPool[i];
        }
        return null;
    }

}
