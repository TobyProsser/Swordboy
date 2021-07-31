using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InBetweenEndlessController : MonoBehaviour
{
    public TextMeshProUGUI topScoreText;
    public TextMeshProUGUI lastScoreText;

    public static int lastScore;

    void Start()
    {
        topScoreText.text = PlayerInfoScript.playerInfo.bestEndlessScore.ToString();
        lastScoreText.text = lastScore.ToString();
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene("EndlessScene");
    }

    public void ArmoryButton()
    {
        SceneManager.LoadScene("Armory");
    }
}
