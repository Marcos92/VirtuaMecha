using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float movementSpeed, rotationSpeed, armRotationSpeed;
    private float maxArmRotationX, maxArmRotationY, armRotationX, armRotationY;
    public GameObject leftArm, rightArm;
    public Transform leftAxisX, rightAxisX, leftAxisY, rightAxisY, leftMuzzle, rightMuzzle;
    bool strafe = false;

	// Use this for initialization
	void Start ()
    {
        maxArmRotationX = 45;
        maxArmRotationY = 45;
        armRotationX = 0;
        armRotationY = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Movement
        if (strafe) transform.Translate((Vector3.forward * Input.GetAxis("VerticalLeft") + Vector3.right * Input.GetAxis("HorizontalLeft")) * Time.deltaTime * movementSpeed);
        else
        {
            transform.Translate(Vector3.forward * Input.GetAxis("VerticalLeft") * Time.deltaTime * movementSpeed);
            transform.Rotate(0, Input.GetAxis("HorizontalLeft") * Time.deltaTime * rotationSpeed, 0);
        }

        //Left weapon
        if (Input.GetButton("LeftWeapon"))
        {
            Debug.Log("Left fire!");

            Ray ray = new Ray(leftMuzzle.position, leftMuzzle.forward);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 500f))
            {
                Destroy(hit.collider.gameObject);
            }
        }

        //Right weapon
        if (Input.GetButton("RightWeapon"))
        {
            Debug.Log("Right fire!");

            Ray ray = new Ray(rightMuzzle.position, rightMuzzle.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 500f))
            {
                Destroy(hit.collider.gameObject);
            }
        }

        //Special weapon
        if (Input.GetAxis("SpecialWeapon") > 0)
        {
            Debug.Log("Special fire!");
            //Do FireS action
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
}
