using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenScript : MonoBehaviour
{
    public GameObject PausePanel;

    private void Start()
    {
        PausePanel.SetActive(false);
    }
    public void PauseButton()
    {
        PausePanel.SetActive(true);
    }

    public void ContinueButton()
    {
        PausePanel.SetActive(false);
    }

    public void ArmoryButton()
    {
        SceneManager.LoadScene("Armory");
    }
}
