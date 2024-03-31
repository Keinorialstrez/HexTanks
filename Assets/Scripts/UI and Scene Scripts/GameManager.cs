using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager gameManager;

    private int EnemysLeft;

    private int FinalScore;

    private void Start()
    {
        if (gameManager == null)
            gameManager = this;

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        EnemysLeft = enemys.Length;

        DontDestroyOnLoad(this);
    }

    private GameManager()
    { }

    public static GameManager getInstance()
    {
        return gameManager;
    }

    public void EndGame()
    {
        SetFinalScore();

        SceneManager.LoadScene("LoseMenu");
    }

    public void Win()
    {
        SetFinalScore();

        SceneManager.LoadScene("WinMenu");
    }

    public void UpdateEnemys()
    {
        EnemysLeft -= 1;

        if (EnemysLeft < 1)
        {
            Win();
        }

        Debug.Log(EnemysLeft);

    }

    public void Restart()
    {
        getInstance().Delete();
        SceneManager.LoadScene("TestLevel");
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void SetFinalScore()
    {
        FinalScore = Score.getInstance().GetScoreValue();
    }

    public string GetFinalScore()
    {
        return FinalScore.ToString();
    }

}
