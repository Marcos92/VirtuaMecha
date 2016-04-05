using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Projectile : MonoBehaviour
{
    float speed;
    public int damage;
    public float lifetime;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag != "Projectile" && c.gameObject.tag != "Player") Destroy(gameObject);
        //calcular dano
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
