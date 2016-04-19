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

	void Start () 
    {
        //When you can use the equip
        nextActivationTime = Time.time;

        //Type of equip
        name = gameObject.name;
        name = name.Replace("(Clone)", "");

        //Associate player to equip
        player = transform.parent.GetComponent<Player>();
	}
	
	void Update () 
    {
        
	}

    public void Activate()
    {
        if(Time.time > nextActivationTime)
        {
            StartCoroutine(name);
        }
    }

    IEnumerator Turret()
    {
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

            if (target != null) //Point to target
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), Time.deltaTime * rotationSpeed);

            //Shooting
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + timeBetweenShots / 1000f;

                Projectile p = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                p.SetSpeed(projectileSpeed);
            }

            yield return null;
        }

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Mine()
    {
        yield return null;

        Mine m = Instantiate(mine, mineSpawnPoint.position, mineSpawnPoint.rotation) as Mine;
        m.SetEnemyTag(enemyTag);

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator EMP()
    {
        yield return null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == 9) //9 == Enemy layer
            {
                hitColliders[i].gameObject.transform.GetComponent<Player>().ToggleMovementTimer(shockDuration);
            }
        }

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Shield()
    {
        turnOffTime = Time.time + shieldDuration;

        while (Time.time < turnOffTime)
        {
            player.ToggleShield(true);
            yield return null;
        }

        player.ToggleShield(false);

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Repair()
    {
        yield return null;

        turnOffTime = Time.time + repairDuration;

        player.ToggleMovement(false);

        while (Time.time < turnOffTime && player.currentHealth < player.maxHealth)
        {
            player.ChangeHealth(Time.deltaTime * player.maxHealth / repairDuration);
        }

        player.ToggleMovement(true);

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Boost()
    {
        //boost

        yield return null;

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }
}
