using UnityEngine;
using System.Collections;

public class CancelClick : MonoBehaviour
{
    public GameObject CreateButton;

    void OnDisable()
    {
        if (CreateButton.activeSelf)
            ControllerManager.instance.CancelClick();
    }    
}
