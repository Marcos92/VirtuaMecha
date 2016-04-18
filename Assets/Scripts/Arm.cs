using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {

    public Gun gun;
    [HideInInspector]
    public float health;

	// Use this for initialization
	void Start ()
    {
        Gun g = Instantiate(gun, transform.position, transform.rotation) as Gun;
        g.transform.SetParent(transform);
        gun = g;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
