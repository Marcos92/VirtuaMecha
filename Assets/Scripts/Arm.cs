using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour 
{
    public Weapon weapon;
    public Transform slot;
    public float maxHealth, currentHealth;
    public Player player;
    bool flip;
    public bool enabled = true;
    
	void Start ()
    {
        //Associate player to arm
        player = transform.parent.transform.GetComponent<Player>();
	}
	
	void Update ()
    {
	
	}

    public void ChangeHealth(float value)
    {
        if (value < 0 && player.immune) return; //Life does not decrease while immune

        currentHealth += value; //Add value to current life

        if (currentHealth > maxHealth) currentHealth = maxHealth; //Make sure current health isn't bigger than max health
        else if (currentHealth <= 0) //Check if is disabled
        {
            currentHealth = 0;
            enabled = false;
        }
        else if (currentHealth > 0 && !enabled) //Check if it got healed if disabled
        {
            if (gameObject.name.Contains("Left")) transform.localRotation = player.rightArm.transform.localRotation;
            else transform.localRotation = player.leftArm.transform.localRotation;

            enabled = true;
        }

        if(gameObject.name.Contains("Left")) player.hud.leftHealth.text = currentHealth.ToString("0");
        else player.hud.rightHealth.text = currentHealth.ToString("0");

        if (currentHealth <= 0)
        {
            ChangeHUDColor(new Color(0, 0, 0, 100));
            if (gameObject.name.Contains("Left"))
            {
                player.hud.leftHealth.gameObject.SetActive(false);
                player.hud.leftAmmo.gameObject.SetActive(false);
            }
            else
            {
                player.hud.rightHealth.gameObject.SetActive(false);
                player.hud.rightAmmo.gameObject.SetActive(false);
            }
        }
        else if (currentHealth < maxHealth / 8) ChangeHUDColor(player.dangerColor);
        else if (currentHealth >= maxHealth / 8 && currentHealth < maxHealth / 4) ChangeHUDColor(player.alertColor);
        else if (currentHealth >= maxHealth / 4 && currentHealth < maxHealth / 2) ChangeHUDColor(player.cautionColor);
        else if (currentHealth >= maxHealth / 2) ChangeHUDColor(player.normalColor);

        if(currentHealth > 0)
        {
            if (gameObject.name.Contains("Left"))
            {
                player.hud.leftHealth.gameObject.SetActive(true);
                player.hud.leftAmmo.gameObject.SetActive(true);
            }
            else
            {
                player.hud.rightHealth.gameObject.SetActive(true);
                player.hud.rightAmmo.gameObject.SetActive(true);
            }
        }

        if (gameObject.name.Contains("Left"))
        {
            Vector3 hb = player.hud.leftHealthBar.transform.localScale;
            player.hud.leftHealthBar.transform.localScale = new Vector3(currentHealth * 10 / maxHealth, hb.y, hb.z);
        }
        else
        {
            Vector3 hb = player.hud.rightHealthBar.transform.localScale;
            player.hud.rightHealthBar.transform.localScale = new Vector3(currentHealth * 10 / maxHealth, hb.y, hb.z);
        }
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

        if (flip) player.hud.leftWeapon.sprite = w.icon;
        else player.hud.rightWeapon.sprite = w.icon;

        weapon = w;
    }

    void ChangeHUDColor(Color newColor)
    {
        if (gameObject.name.Contains("Left")) player.hud.leftArm.color = newColor;
        else player.hud.rightArm.color = newColor;
    }

    void DisableArmHUD()
    {

    }
}
