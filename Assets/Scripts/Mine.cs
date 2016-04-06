using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
    public float effectRadius;
    public float damage;
    public float explosionRadius;
    string enemyTag;

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
            if (hitColliders[i].gameObject.tag == enemyTag)
            {
                //dar dano
                //explodir objectos destrutiveis
            }
        }

        Destroy(gameObject);
    }

    public void SetEnemyTag(string _enemyTag)
    {
        enemyTag = _enemyTag;
    }
}
