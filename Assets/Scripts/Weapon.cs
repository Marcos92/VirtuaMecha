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
    public ParticleSystem muzzleFlash;
    public Projectile projectile;

    public string description;
    public Sprite icon;

    AudioSource audioSource;
    public AudioClip shotSound, emptySound;

	// Use this for initialization
	void Start ()
    {
        currentAmmo = maxAmmo;
        if (transform.parent.gameObject.name.Contains("Left")) transform.parent.GetComponent<Arm>().player.hud.leftAmmo.text = currentAmmo + "/" + maxAmmo;
        else transform.parent.GetComponent<Arm>().player.hud.rightAmmo.text = currentAmmo + "/" + maxAmmo;

        nextShotTime = Time.time;

        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug laser sight
        Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * 500, Color.red);
    }

    public void Shoot()
    {
        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + timeBetweenShots/1000f;
            if (currentAmmo > 0) audioSource.clip = shotSound;
            else audioSource.clip = emptySound;
            audioSource.PlayOneShot(audioSource.clip);

            if(currentAmmo > 0)
            {
                currentAmmo--;

                if (transform.parent.gameObject.name.Contains("Left")) transform.parent.GetComponent<Arm>().player.hud.leftAmmo.text = currentAmmo + "/" + maxAmmo;
                else transform.parent.GetComponent<Arm>().player.hud.rightAmmo.text = currentAmmo + "/" + maxAmmo;

                for (int i = 0; i < projectilesPerShot; i++)
                {
                    Vector3 rotation = muzzle.rotation.eulerAngles;
                    float rx = Random.Range(-coneAngle, coneAngle);
                    float ry = Random.Range(-coneAngle, coneAngle);
                    rotation = rotation + new Vector3(rx, ry, 0);
                    float v = Random.Range(projectileSpeed * (1 - projectileSpeedVariation), projectileSpeed);

                    Projectile p = Instantiate(projectile, muzzle.position, Quaternion.Euler(rotation)) as Projectile;
                    p.SetSpeed(v);
                }

                muzzleFlash.Play();
            }
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }
}
