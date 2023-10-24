using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Level-0");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
