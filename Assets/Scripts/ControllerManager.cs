using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ControllerManager : MonoBehaviour
{
    [HideInInspector]
    public static ControllerManager instance;

    public GameObject[] Buttons;

    private EventSystem eventSystem;

    void Awake()
    {
        if (instance == null)
            instance = this;

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        ChangeFocus(0);
    }

    public void ChangeFocus(int index)
    {
        eventSystem.firstSelectedGameObject = Buttons[index];
    }
}
