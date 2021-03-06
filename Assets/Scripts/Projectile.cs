﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Projectile : MonoBehaviour
{
    float speed;
    public int damage;
    public float lifetime;
    float timer;
    public bool explosive;
    public float explosionRadius, explosionDamage;

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
        if (hit.collider.gameObject.GetComponent<Damageable>() != null && hit.collider.gameObject.layer == 9) 
        {
            Damageable damageable = hit.collider.gameObject.GetComponent<Damageable>();

            if (damageable.type == Damageable.Type.Body) damageable.player.ChangeHealth(-damage);
            else if (damageable.type == Damageable.Type.LeftArm) damageable.player.leftArm.ChangeHealth(-damage);
            else damageable.player.rightArm.ChangeHealth(-damage);
        }

        if(explosive) Explosion();

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
            if (hitColliders[i].gameObject.layer == 9)
            {
                Damageable damageable = hitColliders[i].gameObject.GetComponent<Damageable>();
                float dmg = explosionDamage / (Vector3.Distance(hitColliders[i].transform.position, transform.position)); 

                if (damageable.type == Damageable.Type.Body) damageable.player.ChangeHealth(-dmg);
                else if (damageable.type == Damageable.Type.LeftArm) damageable.player.leftArm.ChangeHealth(-dmg);
                else damageable.player.rightArm.ChangeHealth(-dmg);
            }
        }
    }
}
