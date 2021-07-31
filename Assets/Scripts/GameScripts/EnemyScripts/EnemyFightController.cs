using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightController : MonoBehaviour
{
    public FightScript fightScript;
    OnSwordScript onSwordScript;
    Animator enemyAnim;
    [HideInInspector]
    public int correctBlockChance = 1;                      //given by enemySpawnerScript
    [HideInInspector]
    public float waitTime = 1;                      //given by enemySpawnerScript
    [HideInInspector]
    public float attackDamage = 10;                      //given by enemySpawnerScript
    [HideInInspector]
    public float playerResponceTime = 1;                      //Time that player has to react to incoming attack, given by enemySpawnerScript

    bool blocking;

    private void Awake()
    {
        enemyAnim = this.transform.GetChild(0).GetComponent<Animator>();
        onSwordScript = this.transform.GetChild(0).GetComponent<OnSwordScript>();
    }

    public void StartFighting()                             //Started by fightScript when player gets close
    {
        StartCoroutine(Attack());
    }

    public void block(int incomingAttack) //Told by fightScript when to block and what attack is coming
    {
        blocking = true;                                    //Set blocking to true so enemy doesnt attack

        int toBlock = Random.Range(0, correctBlockChance);          //get chance that enemy blocks correctly
        if (toBlock == 100)
        {
            StartCoroutine(Block(incomingAttack));                  //Do right block           
            print("Correct Block");
        }
        else
        {
            int blockInt;                                       //Make sure If wrong block is chosen, that it wont accentally block the incoming attack
            if (incomingAttack == 1) blockInt = 4;
            else if (incomingAttack == 2) blockInt = 3;
            else if (incomingAttack == 3) blockInt = 2;
            else if (incomingAttack == 4) blockInt = 1;
            else if (incomingAttack == 5) blockInt = 4;
            else if (incomingAttack == 6) blockInt = 3;
            else if (incomingAttack == 7) blockInt = 4;
            else blockInt = 3;

            StartCoroutine(Block(blockInt));                      //Do random block
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (!blocking)
            {
                onSwordScript.hitSword = false;                  //turn hit sword to false to deal damage incase blocked last hit

                int chooseAttack = Random.Range(0, 8);
                fightScript.EnemyAction(chooseAttack);          //Tells FightScript which attack enemy has chosen.

                yield return new WaitForSeconds(playerResponceTime);              //gives time for player to react
                enemyAnim.SetBool("StopAnim", false);
                enemyAnim.SetTrigger(getAttack(chooseAttack));                  //Play attack animing that has been chosen
                yield return new WaitForSeconds(.5f);                          //Wait for animation to play

                enemyAnim.SetBool("StopAnim", true);                            //Stop Animation
                enemyAnim.ResetTrigger(getAttack(chooseAttack));
                //print("StopAnim");
                yield return new WaitForSeconds(waitTime);              //Time between enemy attacks
            }

            yield return null;
        }
    }

    IEnumerator Block(int incomingAction)
    {
        enemyAnim.SetBool("StopBlock", false);                          //reset stop blocking bool
        string curBlock = getBlock(incomingAction);                             //get the desired block
        enemyAnim.SetTrigger(curBlock);                                     //play blocking animation

        yield return new WaitForSeconds(.45f);

        enemyAnim.SetBool("StopBlock", true);                               //Tell animator to stop blocking
        enemyAnim.ResetTrigger(curBlock);
        blocking = false;
    }

    string getBlock(int incomingAction)                                     //Assosiates given number to the correct animation
    {
        string tempString;
        if (incomingAction == 1) tempString = "BlockDown";
        else if (incomingAction == 2) tempString = "BlockUp";
        else if (incomingAction == 3) tempString = "BlockLeft";
        else if (incomingAction == 4) tempString = "BlockRight";
        else if (incomingAction == 5) tempString = "BlockUpLeft";
        else if (incomingAction == 6) tempString = "BlockUpRight";
        else if (incomingAction == 7) tempString = "BlockDownLeft";
        else tempString = "BlockDownRight";

        return tempString;
    }

    string getAttack(int attack)
    {
        string tempString;
        if (attack == 0) tempString = "SwingUp";
        else if (attack == 1) tempString = "SwingDown";
        else if (attack == 2) tempString = "SwingLeft";
        else if (attack == 3) tempString = "SwingRight";
        else if (attack == 4) tempString = "SwingUpLeft";
        else if (attack == 5) tempString = "SwingUpRight";
        else if (attack == 6) tempString = "SwingDownLeft";
        else tempString = "SwingDownRight";

        return tempString;
    }
}
