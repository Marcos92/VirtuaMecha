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
    //[HideInInspector]
    public bool immune, canMove = true, canRotate = true;
    public bool controlable = true;

    [Header("Lights")]
    public GameObject cockpitLights;
    public Color normalLights, dangerLights;
    public float minIntensity, maxIntensity, intensityFactor;
    bool lightCoroutine = false;

    [Header("HUD")]
    public Color normalColor;
    public Color cautionColor;
    public Color alertColor;
    public Color dangerColor;
    public HUD hud;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

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
        leftAxisY.position = leftAxisX.position;
        rightAxisY.position = rightAxisX.position;

        //Arm rotation
        maxArmRotationX = 30;
        maxArmRotationY = 60;
        armRotationX = 0;
        armRotationY = 0;

        //Health
        ChangeHealth(maxHealth);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Controls
        #region
        if (controlable)
        {
            //Movement
            if (canMove)
            {
                transform.Translate(Vector3.forward * Input.GetAxisRaw("VerticalLeft") * Time.deltaTime * movementSpeed);
                if (strafe) transform.Translate(Vector3.right * Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * strafeSpeed);
                else transform.Rotate(0, Input.GetAxisRaw("HorizontalLeft") * Time.deltaTime * rotationSpeed, 0);
            }

            //Left weapon
            if (Input.GetButton("LeftWeapon") && leftArm.currentHealth > 0)
            {
                if (leftArm.currentHealth > 0) leftArm.weapon.Shoot();
            }

            //Right weapon
            if (Input.GetButton("RightWeapon") && rightArm.currentHealth > 0)
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
            if (Input.GetButtonDown("Strafe")) strafe = !strafe;

            //Aim
            //Limit rotation
            armRotationX = Mathf.Clamp(armRotationX, -maxArmRotationX, maxArmRotationX);
            armRotationY = Mathf.Clamp(armRotationY, -maxArmRotationY, maxArmRotationY);

            //Player input
            if (canRotate)
            {
                armRotationX += Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed;
                armRotationY += Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed;

                //Vertical aim
                if (armRotationY > -maxArmRotationY && armRotationY < maxArmRotationY)
                {
                    if (leftArm.currentHealth > 0) leftArm.transform.RotateAround(leftAxisX.position, leftAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
                    if (rightArm.currentHealth > 0) rightArm.transform.RotateAround(rightAxisX.position, rightAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
                }

                //Horizontal aim
                if (armRotationX > -maxArmRotationX && armRotationX < maxArmRotationX)
                {
                    if (leftArm.currentHealth > 0) leftArm.transform.RotateAround(leftAxisY.position, leftAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
                    if (rightArm.currentHealth > 0) rightArm.transform.RotateAround(rightAxisY.position, rightAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
                }
            }
        }
        #endregion

        #region HealthUpdate 
        // Não é preciso fazer isto no update, pode só ser feito quando se vai alterar o valor da vida
        if (currentHealth < maxHealth / 8)
        {
            ChangeHUDColor(dangerColor);
            ChangeLightColor(dangerLights);
            if (!lightCoroutine)
            {
                StartCoroutine("ChangeLightIntensity");
                lightCoroutine = true;
            }
        }
        else if (currentHealth < maxHealth / 4)
        {
            ChangeHUDColor(alertColor);
            if (lightCoroutine)
            {
                StopCoroutine("ChangeLightIntensity");
                lightCoroutine = false;
            }
        }
        if (currentHealth < maxHealth / 2)
        {
            ChangeHUDColor(cautionColor);
            if (lightCoroutine)
            {
                StopCoroutine("ChangeLightIntensity");
                lightCoroutine = false;
            }
        }
        else
        {
            ChangeHUDColor(normalColor);
            ChangeLightColor(normalLights);
            if (lightCoroutine)
            {
                StopCoroutine("ChangeLightIntensity");
                lightCoroutine = false;
            }
        }
        #endregion

        //if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine("EMPEffect", 5);
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void ToggleRotation()
    {
        canRotate = !canRotate;
    }

    IEnumerator EMPEffect(float time)
    {
        ToggleMovement();
        ToggleRotation();
        cockpitLights.SetActive(false);
        hud.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        cockpitLights.SetActive(true);
        hud.gameObject.SetActive(true);
        ToggleRotation();
        ToggleMovement();
        StopCoroutine("EMPEffect");
    }

    public void ToggleShield(bool on)
    {
        immune = !immune;
        if (immune)
        {
            //draw shield 
        }
        else
        {
            //hide shield
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
        if (newEquipment.type == Equip.Type.Offensive && offensive != null) Destroy(transform.FindChild(offensive.name).gameObject);
        else if (defensive != null) Destroy(transform.FindChild(defensive.name).gameObject);

        Equip e = Instantiate(newEquipment, transform.position, transform.rotation) as Equip;
        e.transform.SetParent(transform);

        if (newEquipment.type == Equip.Type.Offensive) offensive = e;
        else defensive = e;
    }

    void ChangeLightColor(Color newColor)
    {
        Light[] lights = cockpitLights.GetComponentsInChildren<Light>(); 

        foreach (Light light in lights)
        {
            light.color = newColor;
        }
    }

    IEnumerator ChangeLightIntensity()
    {
        int toggle = -1;
        Light[] lights = cockpitLights.GetComponentsInChildren<Light>();

        while(true)
        {
            if (lights[0].intensity > maxIntensity || lights[0].intensity <= minIntensity) toggle *= -1;

            foreach (Light light in lights)
            {
                light.intensity += toggle * intensityFactor;
            }
            yield return null;
        }
    }

    void ChangeHUDColor(Color newColor)
    {
        hud.body.color = newColor;
        hud.leftLeg.color = newColor;
        hud.rightLeg.color = newColor;
    }
}
