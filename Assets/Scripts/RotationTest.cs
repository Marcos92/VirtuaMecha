using UnityEngine;
using System.Collections;

public class RotationTest : MonoBehaviour {

    public Transform center;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(center.position, center.right, 1);
        transform.RotateAround(center.position, center.forward, 1);
    }
}
