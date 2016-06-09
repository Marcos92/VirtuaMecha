using UnityEngine;
using System.Collections;

public class MechaInit : MonoBehaviour
{
    public int LeftWeapon, RightWeapon, Offensive, Defensive;

    Player player;

    bool stopMeNow = false;

    void Start()
    {
        player = GetComponent<Player>();

        //player.controlable = true;
        player.transform.Find("Body").gameObject.SetActive(false);
        player.transform.Find("Cockpit").gameObject.SetActive(true);
        player.transform.Find("Camera").gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
    }

    void equipWeapons()
    {
        player.leftArm.EquipWeapon(NetworkCharacterManager.instance.weapons[LeftWeapon]);
        player.rightArm.EquipWeapon(NetworkCharacterManager.instance.weapons[RightWeapon]);
        player.EquipEquipment(NetworkCharacterManager.instance.offensive[Offensive]);
        player.EquipEquipment(NetworkCharacterManager.instance.defensive[Defensive]);
        stopMeNow = true;
    }

    void Update()
    {
        if(!stopMeNow)
            equipWeapons();
    }
}
