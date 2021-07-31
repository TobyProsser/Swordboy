using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatStagePanelScript : MonoBehaviour
{
    public void BackToArmoryButton()
    {
        SceneManager.LoadScene("Armory");
    }
}
