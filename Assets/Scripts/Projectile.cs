using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Projectile : MonoBehaviour
{
    float speed;
    public int damage;
    public float lifetime;
    float timer;
    public bool explosive;
    public float explosionRadius;

    public LayerMask collisionMask;

	void Start ()
    {
        Destroy(gameObject, lifetime);
	}
	
	void Update ()
    {
        //Movement
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

    void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        //Debug.Log("Collision");

        //hit.collider.
        //dano

        if(explosive)
        {
            Explosion();
        }

        Destroy(gameObject);
    }

    //void OnHitObject(Collider c)
    //{
    //    //c.
    //}

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void Explosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.tag == "Enemy")
            {
                //dar dano
                //explodir objectos destrutiveis
            }
        }

        Destroy(gameObject);
    }
}
