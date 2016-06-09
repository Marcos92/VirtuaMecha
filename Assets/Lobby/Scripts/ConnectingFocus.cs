using UnityEngine;
using System.Collections;

public class ConnectingFocus : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("focus");
    }

    IEnumerator focus()
    {
        yield return new WaitForEndOfFrame();
        ControllerManager.instance.SetFocus(gameObject);
    }
}
