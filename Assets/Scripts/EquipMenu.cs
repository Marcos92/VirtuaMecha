using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipMenu : MonoBehaviour 
{
    public GameObject loadingScreen;

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

    public Image iconLW;
    public Image iconRW;
    public Image iconOff;
    public Image iconDef;

    int LeftWeapon, RightWeapon, Offensive, Defensive;

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

        iconLW.sprite = selectedLeftWeapon.icon;
        iconRW.sprite = selectedRightWeapon.icon;
        iconOff.sprite = selectedOffensive.icon;
        iconDef.sprite = selectedDefensive.icon;
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
        LeftWeapon = weapons.IndexOf(selectedLeftWeapon); //Checks index of current weapon

        if (LeftWeapon + direction < 0) LeftWeapon = weapons.Count - 1; //Checks if index is 0
        else if (LeftWeapon + direction > weapons.Count - 1) LeftWeapon = 0;
        else LeftWeapon += direction;

        selectedLeftWeapon = weapons[LeftWeapon];
        player.leftArm.EquipWeapon(selectedLeftWeapon); //Equips weapon

        textLW.text = selectedLeftWeapon.description + "\nAmmo capacity: " + selectedLeftWeapon.maxAmmo + "\nRate of fire: " + selectedLeftWeapon.timeBetweenShots + "ms";
        iconLW.sprite = selectedLeftWeapon.icon;
    }

    public void CycleRightWeapon(int direction)
    {
        RightWeapon = weapons.IndexOf(selectedRightWeapon); //Checks index of current weapon

        if (RightWeapon + direction < 0) RightWeapon = weapons.Count - 1; //Checks if index is 0
        else if (RightWeapon + direction > weapons.Count - 1) RightWeapon = 0;
        else RightWeapon += direction;

        selectedRightWeapon = weapons[RightWeapon];
        player.rightArm.EquipWeapon(selectedRightWeapon); //Equips weapon

        textRW.text = selectedRightWeapon.description + "\nAmmo capacity: " + selectedRightWeapon.maxAmmo + "\nRate of fire: " + selectedRightWeapon.timeBetweenShots + "ms";
        iconRW.sprite = selectedRightWeapon.icon;
    }

    public void CycleOffensive(int direction)
    {
        Offensive = offensive.IndexOf(selectedOffensive); //Checks index of current equip

        if (Offensive + direction < 0) Offensive = offensive.Count - 1; //Checks if index is 0
        else if (Offensive + direction > offensive.Count - 1) Offensive = 0;
        else Offensive += direction;

        selectedOffensive = offensive[Offensive];
        player.EquipEquipment(selectedOffensive); //Equips equipment

        textOff.text = selectedOffensive.description + "\nCooldown: " + selectedOffensive.cooldown + "s";
        iconOff.sprite = selectedOffensive.icon;
    }

    public void CycleDefensive(int direction)
    {
        Defensive = defensive.IndexOf(selectedDefensive); //Checks index of current equip

        if (Defensive + direction < 0) Defensive = defensive.Count - 1; //Checks if index is 0
        else if (Defensive + direction > defensive.Count - 1) Defensive = 0;
        else Defensive += direction;

        selectedDefensive = defensive[Defensive];
        player.EquipEquipment(selectedDefensive); //Equips equipment

        textDef.text = selectedDefensive.description + "\nCooldown: " + selectedDefensive.cooldown + "s";
        iconDef.sprite = selectedDefensive.icon;
    }

    public void Confirm()
    {
        //loadingScreen.SetActive(true);
        //DontDestroyOnLoad(loadingScreen.transform.parent);

        player.controlable = true;
        player.transform.Find("Body").gameObject.SetActive(false);
        player.transform.Find("Cockpit").gameObject.SetActive(true);
        player.transform.Find("Camera").gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
        sendCharacter();

        SceneManager.LoadScene("TestScene");
    }

    private void sendCharacter()
    {
        CharacterChoice tmp = GameObject.Find("CharacterChoice").GetComponent<CharacterChoice>();
        tmp.LeftWeapon = LeftWeapon;
        tmp.RightWeapon = RightWeapon;
        tmp.Offensive = Offensive;
        tmp.Defensive = Defensive;
    }
}
