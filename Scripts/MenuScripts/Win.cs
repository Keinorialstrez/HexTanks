using UnityEngine.SceneManagement;
using UnityEngine;

public class Win : MonoBehaviour
{
    public void Restart()
    {
        GameManager.GetInstance().Restart();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
