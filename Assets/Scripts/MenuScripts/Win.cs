using UnityEngine.SceneManagement;
using UnityEngine;

public class Win : MonoBehaviour
{
    public void Restart()
    {
        GameManager.getInstance().Restart();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
