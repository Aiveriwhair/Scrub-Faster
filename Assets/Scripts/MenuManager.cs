using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Level-0");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
