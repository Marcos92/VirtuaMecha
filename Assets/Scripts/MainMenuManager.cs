using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartClick()
    {
        SceneManager.LoadScene("MenuTest");
    }
}
