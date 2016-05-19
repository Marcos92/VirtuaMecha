using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
    public float effectRadius;
    public float explosionDamage;
    public float explosionRadius;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == 9) //9 == Enemy layer
            {
                Explosion();
            }
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Projectile") Explosion();
    }

    public void Explosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == 9)
            {
                Damageable damageable = hitColliders[i].gameObject.GetComponent<Damageable>();
                float dmg = explosionDamage / (Vector3.Distance(hitColliders[i].transform.position, transform.position));

                if (damageable.type == Damageable.Type.Body) damageable.player.ChangeHealth(-dmg);
                else if (damageable.type == Damageable.Type.LeftArm) damageable.player.leftArm.ChangeHealth(-dmg);
                else damageable.player.rightArm.ChangeHealth(-dmg);
            }
        }

        Destroy(gameObject);
    }
}
