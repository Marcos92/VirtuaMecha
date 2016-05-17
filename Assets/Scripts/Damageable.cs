using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class Damageable : MonoBehaviour 
{
    [HideInInspector]
    public Player player;
    public enum Type { Body, LeftArm, RightArm }
    public Type type;

	void Start () 
    {
        player = transform.parent.GetComponent<Player>();
	}

	void Update () 
    {

	}
}
