using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    private Text finalScore;
    // Start is called before the first frame update
    void Awake()
    {
        finalScore = GetComponent<Text>();

        finalScore.text += GameManager.GetInstance().GetFinalScore();
    }
}
