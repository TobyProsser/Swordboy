using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndlessPlayerController : MonoBehaviour
{
    public int health;
    public Slider healthSlider;

    public FightScript fightScript;

    PlayerMoveController playerMoveController;
    SwipeListener swipeListener;
    MoveOnSwipe_EightDirections moveOnSwipe;
    public PlayerCameraController playerCamScript;

    public EnemySpawnerScript enemySpawnerScript;

    bool nextToEnemy;

    void Awake()
    {
        playerMoveController = this.GetComponent<PlayerMoveController>();
        swipeListener = this.GetComponent<SwipeListener>();
        moveOnSwipe = this.GetComponent<MoveOnSwipe_EightDirections>();


        disableScripts();

        healthSlider.maxValue = health;
    }

    void LateUpdate()
    {
        healthSlider.value = health;

        if (health <= 0) Died();
    }

    public void NextToEnemy()               //Player move script will call this when player has moved to an enemy
    {
        if (nextToEnemy)                    //If an enemy has entered trigger, just to check that the player has moved to an enemies position
        {
            enableScripts();
        }
    }

    void enableScripts()
    {
        playerMoveController.enabled = false;
        swipeListener.enabled = true;
        moveOnSwipe.enabled = true;
    }

    void disableScripts()
    {
        playerMoveController.enabled = true;
        swipeListener.enabled = false;
        moveOnSwipe.enabled = false;
    }

    public void CurrentEnemyDestoryed()                                     //Called by current enemies enemy Controller on death
    {
        nextToEnemy = false;
        disableScripts();
        StartCoroutine(TellCamToMove());                                      //Tell Camera To look at next enemy

        enemySpawnerScript.SpawnNextEnemy();                                  //Spawn another enemy
    }

    IEnumerator TellCamToMove()
    {
        yield return new WaitForSeconds(.1f);                                     //Wait a second for current enemy to die so camera does find the current enemy
        playerCamScript.FindNextEnemy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            nextToEnemy = true;
            fightScript.AssignNewEnemy(other.gameObject, this.gameObject);         //Tell fightScript which enemy you are currently next to and fighting. And give current enemy this gameobject to tell player when enemy dies
        }
    }

    void Died()
    {
        //Camera zooms out on dead body
        //save Respawn point data
        //load correct stage panel in main menu
        ArmoryController.died = true;                                             //tells MainMenu that it is being opened because player died. This will open the stage panel and remove a life
        SceneManager.LoadScene("MainMenu");
    }
}
