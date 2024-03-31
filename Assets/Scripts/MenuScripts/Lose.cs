using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public void Restart()
    {
        GameManager.getInstance().Restart();
    }

}
