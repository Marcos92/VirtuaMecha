using UnityEngine;
using System.Collections;

public class CharacterChoice : MonoBehaviour
{
    [HideInInspector]
    public int LeftWeapon, RightWeapon, Offensive, Defensive;
    public static CharacterChoice instance;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (instance == null)
            instance = this;
    }
}