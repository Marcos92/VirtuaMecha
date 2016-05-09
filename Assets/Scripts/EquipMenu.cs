using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipMenu : MonoBehaviour 
{
    public Player player;
    public List<Weapon> weapons;
    public List<Equip> offensive;
    public List<Equip> defensive;

    Weapon selectedLeftWeapon, selectedRightWeapon;
    Equip selectedOffensive, selectedDefensive;

    [Header("Equipment description labels")]
    public Text textLW;
    public Text textRW;
    public Text textOff;
    public Text textDef;

	// Use this for initialization
	void Start () 
    {
        selectedLeftWeapon = weapons[0];
        selectedRightWeapon = weapons[0];
        selectedOffensive = offensive[0];
        selectedDefensive = defensive[0];

        textLW.text = selectedLeftWeapon.description + "\nAmmo capacity: " + selectedLeftWeapon.maxAmmo + "\nRate of fire: " + selectedLeftWeapon.timeBetweenShots/1000 + "s";
        textRW.text = selectedRightWeapon.description + "\nAmmo capacity: " + selectedRightWeapon.maxAmmo + "\nRate of fire: " + selectedRightWeapon.timeBetweenShots/1000 + "s";
        textOff.text = selectedOffensive.description + "\nCooldown: " + selectedOffensive.cooldown + "s";
        textDef.text = selectedDefensive.description + "\nCooldown: " + selectedDefensive.cooldown + "s";
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player.leftArm.weapon == null) player.leftArm.EquipWeapon(weapons[0]);
        if (player.rightArm.weapon == null) player.rightArm.EquipWeapon(weapons[0]);
        if (player.offensive == null) player.EquipEquipment(offensive[0]);
        if (player.defensive == null) player.EquipEquipment(defensive[0]);

        RotateModel();
	}

    public void RotateModel()
    {
        player.transform.Rotate(Input.GetAxisRaw("VerticalRight") * 0f, Input.GetAxisRaw("HorizontalRight") * 3f, 0);
    }

    public void CycleLeftWeapon(int direction)
    {
        int i = weapons.IndexOf(selectedLeftWeapon); //Checks index of current weapon

        if (i + direction < 0) i = weapons.Count - 1; //Checks if index is 0
        else if (i + direction > weapons.Count - 1) i = 0;
        else i += direction;

        selectedLeftWeapon = weapons[i];
        player.leftArm.EquipWeapon(selectedLeftWeapon); //Equips weapon

        textLW.text = selectedLeftWeapon.description + "\nAmmo capacity: " + selectedLeftWeapon.maxAmmo + "\nRate of fire: " + selectedLeftWeapon.timeBetweenShots + "ms";
    }

    public void CycleRightWeapon(int direction)
    {
        int i = weapons.IndexOf(selectedRightWeapon); //Checks index of current weapon

        if (i + direction < 0) i = weapons.Count - 1; //Checks if index is 0
        else if (i + direction > weapons.Count - 1) i = 0;
        else i += direction;

        selectedRightWeapon = weapons[i];
        player.rightArm.EquipWeapon(selectedRightWeapon); //Equips weapon

        textRW.text = selectedRightWeapon.description + "\nAmmo capacity: " + selectedRightWeapon.maxAmmo + "\nRate of fire: " + selectedRightWeapon.timeBetweenShots + "ms";
    }

    public void CycleOffensive(int direction)
    {
        int i = offensive.IndexOf(selectedOffensive); //Checks index of current equip

        if (i + direction < 0) i = offensive.Count - 1; //Checks if index is 0
        else if (i + direction > offensive.Count - 1) i = 0;
        else i += direction;

        selectedOffensive = offensive[i];
        player.EquipEquipment(selectedOffensive); //Equips equipment

        textOff.text = selectedOffensive.description + "\nCooldown: " + selectedOffensive.cooldown + "s";
    }

    public void CycleDefensive(int direction)
    {
        int i = defensive.IndexOf(selectedDefensive); //Checks index of current equip

        if (i + direction < 0) i = defensive.Count - 1; //Checks if index is 0
        else if (i + direction > defensive.Count - 1) i = 0;
        else i += direction;

        selectedDefensive = defensive[i];
        player.EquipEquipment(selectedDefensive); //Equips equipment

        textDef.text = selectedDefensive.description + "\nCooldown: " + selectedDefensive.cooldown + "s";
    }

    public void Confirm()
    {
        player.controlable = true;
        player.transform.Find("Body").gameObject.SetActive(false);
        player.transform.Find("Cockpit").gameObject.SetActive(true);
        player.transform.Find("Camera").gameObject.SetActive(true);

        SceneManager.LoadScene("TestScene");
    }
}
