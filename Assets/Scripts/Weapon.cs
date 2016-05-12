using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float projectileSpeed;
    public float timeBetweenShots; //ms
    float nextShotTime; 
    public int maxAmmo;
    [HideInInspector]
    public int currentAmmo;
    public int projectilesPerShot = 1;
    [Range(0.0f, 90.0f)]
    public float coneAngle = 0f;
    [Range(0.0f, 0.99f)]
    public float projectileSpeedVariation = 0.5f;

    public Transform muzzle;
    public Projectile projectile;

    public string description;
    public Sprite icon;

	// Use this for initialization
	void Start ()
    {
        currentAmmo = maxAmmo;
        nextShotTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug laser sight
        Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * 500, Color.red);
    }

    public void Shoot()
    {
        if(Time.time > nextShotTime && currentAmmo > 0)
        {
            nextShotTime = Time.time + timeBetweenShots/1000f;

            currentAmmo--;

            for(int i = 0; i < projectilesPerShot; i++)
            {
                Vector3 rotation = muzzle.rotation.eulerAngles;
                float rx = Random.Range(-coneAngle, coneAngle);
                float ry = Random.Range(-coneAngle, coneAngle);
                rotation = rotation + new Vector3(rx, ry, 0);
                float v = Random.Range(projectileSpeed * (1 - projectileSpeedVariation), projectileSpeed);

                Projectile p = Instantiate(projectile, muzzle.position, Quaternion.Euler(rotation)) as Projectile;
                p.SetSpeed(v);
            }
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }
}
