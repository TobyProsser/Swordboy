using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScript : MonoBehaviour
{
    //1 - up, 2 - down, 3 - left, 4 - right, 5 - upleft, 6 - upright, 7 - downleft, 8 - downright || 9-16 same but block
    int curPlayerAction; //Number 1-16 that shows block or attack and from which direction
    int curEnemyAction; //Number 1-16 that shows block or attack and from which direction
    public float responceTime;              //Time between screen flash and enemy attacking
    public int enemyHealth;
    public int blockChance;                 //Chance that enemy chooses right block, 1 = always right block;

    bool comparing;

    GameObject curEnemy;

    public GameObject[] blockWarnings = new GameObject[8];                  //Has to be in correct order. Objects in order of correct block for list above

    public void PlayerAction(int action)          //called by MoveOnSwipe when player makes move
    {
        //If player action is to attack Tell enemy attack controller to block and not to attack
        curPlayerAction = action;

        if (action <= 8) curEnemy.GetComponent<EnemyFightController>().block(action);
    }

    public void EnemyAction(int action)           //Called by enemieFightController when they make a move
    {
        //if enemy action is to attack show where they are going to attack on player screen
        //Give player time to react by running coroutine
        StartCoroutine(FlashScreen(action));
    }

    public void AssignNewEnemy(GameObject enemy, GameObject player)                //Called by playerController when Starting to fight a new enemy
    {
        curEnemy = enemy;                                       //set curEnemy to the enemy playerController is near

        curEnemy.GetComponent<EnemyFightController>().fightScript = this;
        curEnemy.GetComponent<EnemyFightController>().StartFighting();
        curEnemy.GetComponent<EnemyController>().player = player;
        curEnemy.GetComponent<EnemyController>().FightStart();
    }

    IEnumerator FlashScreen(int action)
    {
        GameObject curWarningObject = blockWarnings[action];                //returns correct block warning for current action
        curWarningObject.SetActive(true);
        yield return new WaitForSeconds(.6f);
        curWarningObject.SetActive(false);
    }
}
