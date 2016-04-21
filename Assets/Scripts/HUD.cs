﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    Text health, leftAmmo, rightAmmo, leftHealth, rightHealth, offensive, defensive;
    Player player;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        health = transform.FindChild("Health").GetComponent<Text>();
        rightHealth = transform.FindChild("RightArmHealth").GetComponent<Text>();
        leftHealth = transform.FindChild("LeftArmHealth").GetComponent<Text>();
        rightAmmo = transform.FindChild("RightWeaponAmmo").GetComponent<Text>();
        leftAmmo = transform.FindChild("LeftWeaponAmmo").GetComponent<Text>();
        offensive = transform.FindChild("Offensive").GetComponent<Text>();
        defensive = transform.FindChild("Defensive").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        health.text = "Health\n" + player.currentHealth.ToString("0");

        leftHealth.text = "Health: " + player.leftArm.currentHealth.ToString("0");
        rightHealth.text = "Health: " + player.rightArm.currentHealth.ToString("0");

        leftAmmo.text = "Ammo: " + player.leftArm.weapon.currentAmmo;
        rightAmmo.text = "Ammo: " + player.rightArm.weapon.currentAmmo;

        offensive.text = player.offensive.info;
        defensive.text = player.defensive.info;
	}
}
