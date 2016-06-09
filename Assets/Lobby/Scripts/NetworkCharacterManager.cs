using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkCharacterManager : MonoBehaviour
{
    public List<Weapon> weapons;
    public List<Equip> offensive;
    public List<Equip> defensive;
    public GameObject[] Spawns;
    public static NetworkCharacterManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;


    }
}
