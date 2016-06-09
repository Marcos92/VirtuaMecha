using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ControllerManager : MonoBehaviour
{
    [HideInInspector]
    public static ControllerManager instance;

    public GameObject CreateButton;

    public EventSystem eventSystem;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetFocus(GameObject target)
    {
        eventSystem.enabled = false;
        eventSystem.firstSelectedGameObject = target;
        eventSystem.enabled = true;
    }

    public void CancelClick()
    {
        StartCoroutine("focus");
    }

    IEnumerator focus()
    {
        yield return new WaitForSeconds(0.1f);
        SetFocus(CreateButton);
    }
}
