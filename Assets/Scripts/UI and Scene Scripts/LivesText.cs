using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{
    private static LivesText instance;

    private int livesValue;

    private Text livesText;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        livesValue = 0;

        livesText = GetComponent<Text>();

        ChangeLivesAmount(livesValue);

    }

    
    public void ChangeLivesAmount(int value)
    {
        livesValue = value;

        livesText.text = "Lives: " + livesValue;
    }

    private LivesText()
    { }

    public static LivesText GetInstance()
    {
        return instance;
    }

}
