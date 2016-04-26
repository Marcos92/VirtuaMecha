using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour 
{
    public Weapon weapon;
    public float maxHealth, currentHealth;
    Player player;

	// Use this for initialization
	void Start ()
    {
        //Associate player to arm
        player = transform.parent.transform.GetComponent<Player>(); 

        //Health
        ChangeHealth(maxHealth);

        //Weapon
        EquipWeapon(weapon);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ChangeHealth(float value)
    {
        if (value < 0 && player.immune) return; //Life does not decrease while immune

        currentHealth += value; //Add value to current life

        if (currentHealth > maxHealth) currentHealth = maxHealth; //Make sure current health isn't bigger than max health
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (transform.FindChild(weapon.name) != null) Destroy(transform.FindChild(weapon.name).gameObject); //Destroy previous weapon

        Weapon w = Instantiate(newWeapon, transform.position, transform.rotation) as Weapon;
        w.transform.SetParent(transform);

        weapon = w;
    }
}
