using UnityEngine;
using System.Collections;

public class Equip : MonoBehaviour {

    string name;
    public enum Type { Offensive, Defensive };
    public Type type;
    public float cooldown;
    float nextActivationTime, turnOffTime;

	void Start () 
    {
        nextActivationTime = Time.time;
        name = gameObject.name;
	}
	
	void Update () 
    {
        
	}

    public void Activate()
    {
        if(Time.time > nextActivationTime)
        {
            StartCoroutine(name);
        }
    }

    IEnumerator Turret()
    {
        float duration = 2f;
        turnOffTime = Time.time + duration;

        while(Time.time < turnOffTime)
        {
            //pew pew
            Debug.Log("Shooting");

            yield return null;
        }

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Mine()
    {
        //dropmine

        yield return null;

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator EMP()
    {
        //attack

        yield return null;

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Shield()
    {
        float duration = 2f;
        turnOffTime = Time.time + duration;

        while (Time.time < turnOffTime)
        {
            //shields up

            yield return null;
        }

        //shields down

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Repair()
    {
        float duration = 2f;
        turnOffTime = Time.time + duration;

        //cant move

        while (Time.time < turnOffTime)
        {
            //repair
            yield return null;
        }

        //can move

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }

    IEnumerator Boost()
    {
        //boost

        yield return null;

        nextActivationTime = Time.time + cooldown;
        StopCoroutine(name);
    }
}
