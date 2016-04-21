using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public float movementSpeed, strafeSpeed, rotationSpeed, armRotationSpeed;
    float maxArmRotationX, maxArmRotationY, armRotationX, armRotationY;
    [HideInInspector]
    public Arm leftArm, rightArm;
    Transform leftAxisX, rightAxisX, leftAxisY, rightAxisY;
    bool strafe = false;
    public Equip offensive, defensive;
    public float maxHealth, currentHealth;
    [HideInInspector]
    public bool immune, cantMove, cantRotate;
    public bool controlable;

	// Use this for initialization
	void Start ()
    {
        //Associate arms to player
        leftArm = gameObject.transform.FindChild("LeftArm").transform.GetComponent<Arm>();
        rightArm = gameObject.transform.FindChild("RightArm").transform.GetComponent<Arm>();

        //Define arm rotation axes
        leftAxisY = gameObject.transform.FindChild("LeftAxisY").transform;
        rightAxisY = gameObject.transform.FindChild("RightAxisY").transform;
        leftAxisX = leftArm.transform.FindChild("LeftAxisX").transform;
        rightAxisX = rightArm.transform.FindChild("RightAxisX").transform;

        //Arm rotation
        maxArmRotationX = 60;
        maxArmRotationY = 60;
        armRotationX = 0;
        armRotationY = 0;

        //Equips
        EquipEquipment(offensive);
        EquipEquipment(defensive);

        //Health
        ChangeHealth(maxHealth);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(controlable)
        {
            //Movement
            if (!cantMove)
            {
                transform.Translate(Vector3.forward * Input.GetAxisRaw("VerticalLeft") * Time.deltaTime * movementSpeed);
                if (strafe) transform.Translate(Vector3.right * Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * strafeSpeed);
                else transform.Rotate(0, Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * rotationSpeed, 0);
            }

            //Left weapon
            if (Input.GetButton("LeftWeapon"))
            {
                if (leftArm.currentHealth > 0) leftArm.weapon.Shoot();
            }

            //Right weapon
            if (Input.GetButton("RightWeapon"))
            {
                if (rightArm.currentHealth > 0) rightArm.weapon.Shoot();
            }

            //Offensive equipment
            if (Input.GetAxis("Offensive") > 0 || Input.GetButton("Offensive"))
            {
                offensive.Activate();
            }

            //Defensive equipment
            if (Input.GetAxis("Defensive") > 0 || Input.GetButton("Defensive"))
            {
                defensive.Activate();
            }

            //Melee
            if (Input.GetButton("MeleeLeft") && Input.GetButton("MeleeRight"))
            {
                Debug.Log("Special punch!");
                //Do SpecialMelee action
            }
            else if (Input.GetButton("MeleeLeft"))
            {
                Debug.Log("Left punch!");
                //Do LeftPunch action
            }
            else if (Input.GetButton("MeleeRight"))
            {
                Debug.Log("Right punch!");
                //Do RightPunch action
            }

            //Strafe
            if (Input.GetAxis("Strafe") > 0) strafe = true;
            else strafe = false;

            //Aim
            //Limit rotation
            armRotationX = Mathf.Clamp(armRotationX, -maxArmRotationX, maxArmRotationX);
            armRotationY = Mathf.Clamp(armRotationY, -maxArmRotationY, maxArmRotationY);

            //Player input
            if (!cantRotate)
            {
                armRotationX += Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed;
                armRotationY += Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed;
            }

            //Vertical aim
            if (armRotationY > -maxArmRotationY && armRotationY < maxArmRotationY)
            {
                leftArm.transform.RotateAround(leftAxisX.position, leftAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
                rightArm.transform.RotateAround(rightAxisX.position, rightAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
            }

            //Horizontal aim
            if (armRotationX > -maxArmRotationX && armRotationX < maxArmRotationX)
            {
                leftArm.transform.RotateAround(leftAxisY.position, leftAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
                rightArm.transform.RotateAround(rightAxisY.position, rightAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
            }
        }
    }

    public void ToggleMovement(bool on)
    {
        if (on)
        {
            cantMove = false;
        }
        else
        {
            cantMove = true;
        }
    }

    public void ToggleRotation(bool on)
    {
        if (on)
        {
            cantRotate = false;
        }
        else
        {
            cantRotate = true;
        }
    }

    public void EMPEffect(float time)
    {
        StartCoroutine("_EMPEffect", time);
    }

    IEnumerator _EMPEffect(float time)
    {
        ToggleMovement(false);
        ToggleRotation(false);
        yield return new WaitForSeconds(time);
        ToggleRotation(true);
        ToggleMovement(true);
        StopCoroutine("_EMPEffect");
    }

    public void ToggleShield(bool on)
    {
        if(on)
        {
            //draw shield
            immune = true;
        }
        else
        {
            //erase shield
            immune = false;
        }
    }

    public void ChangeHealth(float value)
    {
        if (value < 0 && immune) return; //Life does not decrease while immune

        currentHealth += value; //Add value to current life

        if (currentHealth > maxHealth) currentHealth = maxHealth; //Make sure current health isn't bigger than max health
    }

    public void EquipEquipment(Equip newEquipment)
    {
        if (newEquipment.type == Equip.Type.Offensive) Destroy(gameObject.transform.FindChild(offensive.name));
        else Destroy(gameObject.transform.FindChild(defensive.name));

        Equip e = Instantiate(newEquipment, transform.position, transform.rotation) as Equip;
        e.transform.SetParent(transform);

        if (newEquipment.type == Equip.Type.Offensive) offensive = e;
        else defensive = e;
    }
}
