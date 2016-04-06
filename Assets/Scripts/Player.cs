using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float movementSpeed, strafeSpeed, rotationSpeed, armRotationSpeed;
    float startingMovementSpeed, startingStrafeSpeed, startingRotationSpeed, startingArmRotationSpeed;
    float maxArmRotationX, maxArmRotationY, armRotationX, armRotationY;
    public GameObject leftArm, rightArm;
    public Transform leftAxisX, rightAxisX, leftAxisY, rightAxisY;
    bool strafe = false;
    public Gun leftWeapon, rightWeapon;
    public Equip offensive, defensive;
    bool immune;

	// Use this for initialization
	void Start ()
    {
        startingMovementSpeed = movementSpeed;
        startingStrafeSpeed = strafeSpeed;
        startingRotationSpeed = rotationSpeed;
        startingArmRotationSpeed = armRotationSpeed;

        maxArmRotationX = 60;
        maxArmRotationY = 60;
        armRotationX = 0;
        armRotationY = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Movement
        transform.Translate(Vector3.forward * Input.GetAxisRaw("VerticalLeft") * Time.deltaTime * movementSpeed);

        if (strafe) transform.Translate(Vector3.right * Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * strafeSpeed);
        else transform.Rotate(0, Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * rotationSpeed, 0);

        //Left weapon
        if (Input.GetButton("LeftWeapon"))
        {
            leftWeapon.Shoot();
        }

        //Right weapon
        if (Input.GetButton("RightWeapon"))
        {
            rightWeapon.Shoot();
        }

        //Special weapon
        if (Input.GetAxis("SpecialWeapon") > 0 || Input.GetButton("SpecialWeapon"))
        {
            offensive.Activate();
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
        armRotationX += Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed;
        armRotationY += Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed;
        
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

    void ToggleMovement(bool on)
    {
        if(on)
        {
            movementSpeed = startingMovementSpeed;
            rotationSpeed = startingRotationSpeed;
            armRotationSpeed = startingArmRotationSpeed;
            strafeSpeed = startingStrafeSpeed;
        }
        else 
        {
            movementSpeed = 0f;
            rotationSpeed = 0f;
            armRotationSpeed = 0f;
            strafeSpeed = 0f;
        }
    }

    public void ToggleMovementTimer(float time)
    {
        StartCoroutine("_ToggleMovementTimer", time);
    }

    IEnumerator _ToggleMovementTimer(float time)
    {
        ToggleMovement(false);
        yield return new WaitForSeconds(time);
        ToggleMovement(true);
        StopCoroutine("_ToggleMovementTimer");
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

    public void Repair()
    {
        //if currenthealth < health; currenthealth++
    }
}
