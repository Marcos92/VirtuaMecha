using UnityEngine;
using System.Collections;

public class Equip : MonoBehaviour 
{
    string name;
    public enum Type { Offensive, Defensive };
    public Type type;
    public float cooldown;
    float nextActivationTime, turnOffTime;
    Player player;
    [HideInInspector]
    public string info;
    public string description;
    bool active = false;
    public Sprite icon;

    [Header("Turret")]
    public float turretDuration;
    public float timeBetweenShots;
    public float projectileSpeed;
    public float rotationSpeed;
    public float reach;
    float nextShotTime;
    public Projectile projectile;

    [Header("Mine")]
    public Mine mine;
    public Transform mineSpawnPoint;
    public string enemyTag;

    [Header("EMP")]
    public float effectRadius;
    public float shockDuration;

    [Header("Shield")]
    public float shieldDuration;

    [Header("Repair")]
    public float repairDuration;

    [Header("Boost")]
    public float boostSpeed;
    public float boostDuration;

	void Start () 
    {
        //When you can use the equip
        nextActivationTime = Time.time;

        //Type of equip
        name = gameObject.name;
        name = name.Replace("(Clone)", "");

        //Associate player to equip
        player = transform.parent.GetComponent<Player>();

        info = name + "\nREADY";
	}
	
	void Update () 
    {
        if (Time.time < nextActivationTime) info = name + "\nCOOLDOWN: " + (nextActivationTime - Time.time).ToString("00.00") + "s";
        else if (!active) info = name + "\nREADY";
	}

    public void Activate()
    {
        if(Time.time > nextActivationTime && !active)
        { 
            StartCoroutine(name);
        }
    }

    IEnumerator Turret()
    {
        active = true;
        turnOffTime = Time.time + turretDuration;
        nextShotTime = Time.time;

        Transform muzzle = transform.Find("Muzzle").transform;

        while(Time.time < turnOffTime)
        {
            //Rotation
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            GameObject target = null;

            foreach (GameObject t in targets) //Get all targets in reach and find the closest
            {
                float newDistance = (position - t.transform.position).sqrMagnitude;

                if (newDistance < distance && newDistance <= reach) target = t;
            }

            if (target != null && Time.time > nextShotTime)
            {
                info = name + "\nSHOOTING";

                //Point to target
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), Time.deltaTime * rotationSpeed);

                //Shooting
                nextShotTime = Time.time + timeBetweenShots / 1000f;
                Projectile p = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                p.SetSpeed(projectileSpeed);
            }
            else if (target == null) info = name + "\nNO TARGET";

            yield return null;
        }

        active = false;
        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Mine()
    {
        yield return null;
        active = true;

        Mine m = Instantiate(mine, mineSpawnPoint.position, mineSpawnPoint.rotation) as Mine;
        m.SetEnemyTag(enemyTag);

        active = false;
        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator EMP()
    {
        yield return null;
        active = true;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == 9) //9 == Enemy layer
            {
                hitColliders[i].gameObject.transform.GetComponent<Player>().StartCoroutine("EMPEffect", shockDuration);
            }
        }

        active = false;
        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Shield()
    {
        active = true;
        info = name + "\nSHIELDS UP";

        player.ToggleShield(true);
        yield return new WaitForSeconds(shieldDuration);
        player.ToggleShield(false);

        active = false;
        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Repair()
    {
        float newCooldown = cooldown;

        if (player.currentHealth == player.maxHealth && player.leftArm.currentHealth == player.leftArm.maxHealth && player.rightArm.currentHealth == player.rightArm.maxHealth)
        {
            newCooldown = 0f; //If at full health and repair is used you won't have to wait for cooldown
        }

        active = true;

        info = name + "\nREPAIRING";

        turnOffTime = Time.time + repairDuration;

        player.ToggleMovement();
        player.ToggleRotation();

        while (Time.time < turnOffTime && (player.currentHealth < player.maxHealth || player.leftArm.currentHealth < player.leftArm.maxHealth || player.rightArm.currentHealth < player.rightArm.maxHealth))
        {
            player.ChangeHealth(player.maxHealth * Time.deltaTime / repairDuration);
            player.leftArm.ChangeHealth(player.leftArm.maxHealth * Time.deltaTime / repairDuration);
            player.rightArm.ChangeHealth(player.rightArm.maxHealth * Time.deltaTime / repairDuration);
            yield return null;
        }

        player.ToggleMovement();
        player.ToggleRotation();

        active = false;
        nextActivationTime = Time.time + newCooldown;
        StopCoroutine(name);
    }

    IEnumerator Boost()
    {
        active = true;

        info = name + "\nBOOSTING";

        turnOffTime = Time.time + boostDuration;

        player.ToggleMovement();

        while(Time.time < turnOffTime)
        {
            player.transform.Translate(Vector3.forward * Time.deltaTime * boostSpeed);
            yield return null;
        } 

        player.ToggleMovement();

        active = false;
        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }
}
