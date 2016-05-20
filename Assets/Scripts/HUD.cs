using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    [HideInInspector]
    public Text health, leftAmmo, rightAmmo, leftHealth, rightHealth, offensive, defensive;
    [HideInInspector]
    public Image body, leftArm, rightArm, leftLeg, rightLeg, healthBar, leftHealthBar, rightHealthBar, leftWeapon, rightWeapon;
    Player player;

	// Use this for initialization
	void Start () 
    {
        player = transform.parent.GetComponent<Player>();

        health = transform.FindChild("Health").GetComponent<Text>();
        rightHealth = transform.FindChild("RightArmHealth").GetComponent<Text>();
        leftHealth = transform.FindChild("LeftArmHealth").GetComponent<Text>();
        rightAmmo = transform.FindChild("RightWeapon").GetComponent<Text>();
        leftAmmo = transform.FindChild("LeftWeapon").GetComponent<Text>();
        offensive = transform.FindChild("Offensive").GetComponent<Text>();
        defensive = transform.FindChild("Defensive").GetComponent<Text>();

        rightWeapon = transform.FindChild("RightWeapon").GetChild(0).GetComponent<Image>();
        leftWeapon = transform.FindChild("LeftWeapon").GetChild(0).GetComponent<Image>();

        healthBar = transform.FindChild("Health").GetChild(0).GetComponent<Image>();
        rightHealthBar = transform.FindChild("RightArmHealth").GetChild(0).GetComponent<Image>();
        leftHealthBar = transform.FindChild("LeftArmHealth").GetChild(0).GetComponent<Image>();

        //Mech blueprint
        Transform mech = transform.FindChild("Mech");
        body = mech.FindChild("Body").GetComponent<Image>();
        leftArm = mech.FindChild("ArmLeft").GetComponent<Image>();
        rightArm = mech.FindChild("ArmRight").GetComponent<Image>();
        leftLeg = mech.FindChild("LegLeft").GetComponent<Image>();
        rightLeg = mech.FindChild("LegRight").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
}
