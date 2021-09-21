using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public void Restart()
    {
        GameManager.GetInstance().Restart();
    }

}
