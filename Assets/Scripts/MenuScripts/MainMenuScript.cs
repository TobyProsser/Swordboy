using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{

    public void ArmoryButton()
    {
        SceneManager.LoadScene("Armory");
    }

    public void Shop()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
