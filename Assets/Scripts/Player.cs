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
    public bool immune, canMove = true, canRotate = true, canShoot = true;
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

    [Header("HUD")]
    public float shakeFactor = 0.1f;
    public float maxShakeDuration = 1f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        hud = gameObject.transform.FindChild("HUD").transform.GetComponent<HUD>();

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
        leftArm.ChangeHealth(leftArm.maxHealth);
        rightArm.ChangeHealth(rightArm.maxHealth);
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

            if(canShoot)
            {
                //Left weapon
                if (Input.GetButton("LeftWeapon") && leftArm.enabled) leftArm.weapon.Shoot();

                //Right weapon
                if (Input.GetButton("RightWeapon") && rightArm.enabled) rightArm.weapon.Shoot();

                //Offensive equipment
                if (Input.GetAxis("Offensive") > 0 || Input.GetButton("Offensive")) offensive.Activate();

                //Defensive equipment
                if (Input.GetAxis("Defensive") > 0 || Input.GetButton("Defensive")) defensive.Activate();
            }

            //Melee
            //if (Input.GetButton("MeleeLeft") && Input.GetButton("MeleeRight"))
            //{
            //    Debug.Log("Special punch!");
            //    //Do SpecialMelee action
            //}
            //else if (Input.GetButton("MeleeLeft"))
            //{
            //    Debug.Log("Left punch!");
            //    //Do LeftPunch action
            //}
            //else if (Input.GetButton("MeleeRight"))
            //{
            //    Debug.Log("Right punch!");
            //    //Do RightPunch action
            //}

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
                    if (leftArm.enabled) leftArm.transform.RotateAround(leftAxisX.position, leftAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
                    if (rightArm.enabled) rightArm.transform.RotateAround(rightAxisX.position, rightAxisX.right, Input.GetAxis("VerticalRight") * Time.deltaTime * armRotationSpeed);
                }

                //Horizontal aim
                if (armRotationX > -maxArmRotationX && armRotationX < maxArmRotationX)
                {
                    if (leftArm.enabled) leftArm.transform.RotateAround(leftAxisY.position, leftAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
                    if (rightArm.enabled) rightArm.transform.RotateAround(rightAxisY.position, rightAxisY.transform.up, Input.GetAxis("HorizontalRight") * Time.deltaTime * armRotationSpeed);
                }
            }
        }
        #endregion

        //if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine("EMPEffect", 5);
        if (Input.GetKeyDown(KeyCode.Space)) ChangeHealth(-50);
        //if (Input.GetKeyDown(KeyCode.Space)) rightArm.ChangeHealth(-5);
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void ToggleRotation()
    {
        canRotate = !canRotate;
    }

    public void ToggleShooting()
    {
        canShoot = !canShoot;
    }

    IEnumerator EMPEffect(float time)
    {
        ToggleMovement();
        ToggleRotation();
        ToggleShooting();
        cockpitLights.SetActive(false);
        hud.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        cockpitLights.SetActive(true);
        hud.gameObject.SetActive(true);
        ToggleRotation();
        ToggleMovement();
        ToggleShooting();
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
        if (value < 0)
        {
            StartCoroutine("Shake", value);
            if (immune) return; //Life does not decrease while immune
        }

        currentHealth += value; //Add value to current life

        if (currentHealth > maxHealth) currentHealth = maxHealth; //Make sure current health isn't bigger than max health
        else if (currentHealth <= 0) Destroy(gameObject);

        hud.health.text = currentHealth.ToString("0"); //Write health to HUD

        #region HealthUpdateHUD
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
        else if (currentHealth >= maxHealth / 8 && currentHealth < maxHealth / 4)
        {
            ChangeHUDColor(alertColor);
            ChangeLightColor(normalLights);
            StopCoroutine("ChangeLightIntensity");
            lightCoroutine = false;
        }
        else if (currentHealth >= maxHealth / 4 && currentHealth < maxHealth / 2)
        {
            ChangeHUDColor(cautionColor);
            ChangeLightColor(normalLights);
            StopCoroutine("ChangeLightIntensity");
            lightCoroutine = false;
        }
        else if (currentHealth >= maxHealth / 2)
        {
            ChangeHUDColor(normalColor);
            ChangeLightColor(normalLights);
            StopCoroutine("ChangeLightIntensity");
            lightCoroutine = false;
        }
        #endregion

        Vector3 hb = hud.healthBar.transform.localScale;
        hud.healthBar.transform.localScale = new Vector3(currentHealth * 10 / maxHealth, hb.y, hb.z);
    }

    IEnumerable Shake(float damage)
    {
        yield return null;

        damage = Mathf.Abs(damage);
        float shakeIntensity = Mathf.Log(damage + 1) * shakeFactor;
        float shakeDuration = shakeIntensity * maxShakeDuration / (Mathf.Log(maxHealth + 1) * shakeFactor);
        float shakeStop = Time.time + shakeDuration;

        while(Time.time < shakeStop)
        {
            transform.transform.FindChild("Cockpit").transform.localPosition = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
            //yield return null;
        }

        transform.FindChild("Cockpit").transform.localPosition = Vector3.zero;
    }

    public void EquipEquipment(Equip newEquipment)
    {
        if (newEquipment.type == Equip.Type.Offensive && offensive != null) Destroy(transform.FindChild(offensive.name).gameObject);
        else if (defensive != null) Destroy(transform.FindChild(defensive.name).gameObject);

        Equip e = Instantiate(newEquipment, transform.position, transform.rotation) as Equip;
        e.transform.SetParent(transform);

        if (newEquipment.type == Equip.Type.Offensive)
        {
            offensive = e;
            hud.offImage.sprite = e.icon;
        }
        else
        {
            defensive = e;
            hud.defImage.sprite = e.icon;
        }
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
