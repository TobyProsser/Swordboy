using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleStageController : MonoBehaviour
{
    public Button[] castleButtons = new Button[7];

    int levelNumber;        //Given by script that saves levels, used to check which buttons should be unlocked
    int castleLevelAmount = 7;

    public Color onColor;
    public Color offColor;

    private void OnEnable()
    {
        levelNumber = PlayerInfoScript.playerInfo.level;
        LockButtons();
        UnlockButtons();
    }

    void UnlockButtons()
    {
        for (int i = 0; i <= levelNumber; i++)                           //loops through buttons and turns them on until level number is reached. 
        {
            if (i >= castleLevelAmount) break;                            //If the levelnumber play is on is greater than amount of levels in castle stage, turn all levels on then break
            else
            {
                castleButtons[i].interactable = true;
                castleButtons[i].transform.GetChild(0).GetComponent<Image>().color = onColor;
            }
        }
    }

    void LockButtons()
    {
        for (int i = 0; i < castleButtons.Length; i++)                           //loops through buttons and turns them off
        {
            castleButtons[i].interactable = false;
            castleButtons[i].transform.GetChild(0).GetComponent<Image>().color = offColor;
        }
    }

    public void LevelSelector(int level)
    {
        MovePlayerAtStart.startSpawnNo = level;
        SceneManager.LoadScene("CastleScene");
    }
}
