using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int health;
    public Slider healthSlider;

    public FightScript fightScript;

    PlayerMoveController playerMoveController;
    SwipeListener swipeListener;
    MoveOnSwipe_EightDirections moveOnSwipe;
    public PlayerCameraController playerCamScript;

    //Only one of these variables will be assigned depending on which scene player is in
    public EnemySpawnerScript enemySpawnerScript;
    public EndlessEnemySpawner endlessSpawner;

    bool nextToEnemy;

    public GameObject beatStagePanel;

    public bool endless;      //Set to true in inspector if in endless scene
    public int curScore;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (!endless) beatStagePanel.SetActive(false);
        else
        {
            curScore = 0;
            UpdateScoreText();
        }

        playerMoveController = this.GetComponent<PlayerMoveController>();
        swipeListener = this.GetComponent<SwipeListener>();
        moveOnSwipe = this.GetComponent<MoveOnSwipe_EightDirections>();


        disableScripts();

        healthSlider.maxValue = health;
    }

    void Start()
    {
        ChangeSkin();
    }

    void LateUpdate()
    {
        healthSlider.value = health;

        if (health <= 0) Died();
    }

    void ChangeSkin()
    {
        List<GameObject> skins = new List<GameObject>();

        //Loop through all children in player
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform curChild = transform.GetChild(i);
            //if childs tag is skin
            if (curChild.tag == "Skin")
            {
                //turn gameobject off
                curChild.gameObject.SetActive(false);
                //add gameobject to list
                skins.Add(curChild.gameObject);
            }
        }
        //turn on the skin that relates to the saved skin number in playerInfo singleton
        skins[PlayerInfoScript.playerInfo.playerSkin].SetActive(true);
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

    private void UpdateScoreText()
    {
        scoreText.text = curScore.ToString();
    }

    public void CurrentEnemyDestoryed(GameObject killedEnemy)                                     //Called by current enemies enemy Controller on death
    {
        if (endless)
        {
            curScore++;
            UpdateScoreText();
        }

        if (killedEnemy.tag == "BossEnemy")
        {
            //if player has beaten this stage before
            if (PlayerInfoScript.playerInfo.level >= 7)                                     
            {
                //just open ArmoryScene
                SceneManager.LoadScene("Armory");                                                       
            }
            else
            {
                //if player hasnt beaten this stage before open up win panel
                beatStagePanel.SetActive(true);
                //give player four more lives for beating stage
                PlayerInfoScript.playerInfo.level += 4;
            }
        }
        nextToEnemy = false;
        disableScripts();
        StartCoroutine(TellCamToMove());                                      //Tell Camera To look at next enemy

        if(!endless) enemySpawnerScript.SpawnNextEnemy();                                  //Spawn another enemy
        else endlessSpawner.SpawnNextEnemy();
    }

    IEnumerator TellCamToMove()
    {
        yield return new WaitForSeconds(.1f);                                     //Wait a second for current enemy to die so camera does find the current enemy
        playerCamScript.FindNextEnemy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag  == "BossEnemy")
        {
            nextToEnemy = true;
            fightScript.AssignNewEnemy(other.gameObject, this.gameObject);         //Tell fightScript which enemy you are currently next to and fighting. And give current enemy this gameobject to tell player when enemy dies
        }

        if (other.tag == "RespawnObject")
        {
            if (PlayerInfoScript.playerInfo.level > other.GetComponent<RespawnObjectScript>().respawnNumber) PlayerInfoScript.playerInfo.level++;                                                         //Only increase level if current level is less than what level respawnpoint represents
        }
    }

    void Died()
    {
        if (!endless)
        {
            //Camera zooms out on dead body
            //save Respawn point data
            //load correct stage panel in main menu
            ArmoryController.died = true;                                             //tells MainMenu that it is being opened because player died. This will open the stage panel and remove a life
            SceneManager.LoadScene("Armory");
        }
        else
        {
            //save top score
            int topScore = PlayerInfoScript.playerInfo.bestEndlessScore;
            //if the current score is greater than the current top score
            if (curScore > topScore)
            {
                //set saved top score to the curScore;
                PlayerInfoScript.playerInfo.bestEndlessScore = curScore;
            }

            //Set current Score to between scene;
            InBetweenEndlessController.lastScore = curScore;
            //open play again scene
            SceneManager.LoadScene("InBetweenEndlessScene");
        }
    }
}
