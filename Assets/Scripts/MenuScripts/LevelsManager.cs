using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public GameObject[] panels = new GameObject[2];

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI topScoreText;

    void Start()
    {
        livesText.text = "Lives: " + PlayerInfoScript.playerInfo.lives.ToString();
        topScoreText.text = "Top Score: " + PlayerInfoScript.playerInfo.bestEndlessScore.ToString();
        ShowPanel(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndlessMode()
    {
        SceneManager.LoadScene("EndlessScene");
    }

    public void CastleStageButton()
    {
        ShowPanel(1);
    }

    public void StageBackButton()                       //accessed by back buttons on level panels, brings back to stage seletor panel
    {
        ShowPanel(0);
    }

    void ShowPanel(int panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[panel].SetActive(true);
    }
}
