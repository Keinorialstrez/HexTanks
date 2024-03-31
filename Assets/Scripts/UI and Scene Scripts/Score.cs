using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static Score instance;

    private int scoreValue;

    private Text scoreText;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        scoreValue = 0;

        scoreText = GetComponent<Text>();

        ChangeScore(scoreValue);

       // DontDestroyOnLoad(transform.root);

    }


    public void ChangeScore(int value)
    {
        scoreValue += value;

        scoreText.text = "Score: " + scoreValue;
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    private Score()
    { }

    public static Score getInstance()
    {
            return instance;
    }

}
