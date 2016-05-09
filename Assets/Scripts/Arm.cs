using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour 
{
    public Weapon weapon;
    public Transform slot;
    public float maxHealth, currentHealth;
    Player player;
    bool flip;

	// Use this for initialization
	void Start ()
    {
        //Associate player to arm
        player = transform.parent.transform.GetComponent<Player>(); 

        //Health
        ChangeHealth(maxHealth);
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
        bool flip = false;
        if (gameObject.name.Contains("Left")) flip = true;

        if (weapon != null) Destroy(transform.FindChild(weapon.name).gameObject); //Destroy previous weapon

        Weapon w = Instantiate(newWeapon, slot.position, slot.rotation) as Weapon;
        w.transform.SetParent(transform);
        w.transform.localScale = transform.localScale;

        if(flip) 
        {
            Vector3 v = w.transform.localScale;
            w.transform.localScale = new Vector3(-v.x, v.y, v.z);
        }

        weapon = w;
    }
}
