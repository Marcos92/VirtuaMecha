using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    Text health, leftAmmo, rightAmmo, leftHealth, rightHealth, shield;
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
        shield = transform.FindChild("Shield").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        health.text = "Health\n" + player.currentHealth;

        leftHealth.text = "Health: " + player.leftArm.currentHealth;
        rightHealth.text = "Health: " + player.rightArm.currentHealth;

        leftAmmo.text = "Ammo: " + player.leftArm.weapon.currentAmmo;
        rightAmmo.text = "Ammo: " + player.rightArm.weapon.currentAmmo;

        if (player.immune) shield.text = "SHIELDS UP";
        else shield.text = "";
	}
}
