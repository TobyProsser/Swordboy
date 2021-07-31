using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwordScript : MonoBehaviour
{
    Animator thisAnim;

    [HideInInspector]
    public bool hitSword;                         //Reset by MoveOnSwipe_EightDirections script
    bool hitOpponent;

    public bool playerSword;
    public int damage;

    bool swinging;                                  //Prevents dealing damage twice;  

    //[HideInInspector]
    public bool swordActive = false;
    BoxCollider BC;

    private void Awake()
    {
        thisAnim = this.GetComponent<Animator>();
        BC = this.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        //If this is the players sword, get values from PlayerInfo singleton
        if (playerSword)
        {
            //run get cur sword on playerInfo singleton
            PlayerInfoScript.playerInfo.GetCurSword();
            //set needed values
            damage = PlayerInfoScript.playerInfo.damage;

            //Set the correct sword model to activel
            SetCorrectModel();
        }
    }

    private void Update()
    {
        if (playerSword)
        {
            //ADD BACK WHEN I HAVE TIME TO OPTIMIZE
            //if (swordActive) BC.enabled = true;                               //when sword isnt being used disable box collider. This means attacks arent blocked when just standing still
            //else BC.enabled = false;                                          //bool changed by MoveOnSwipe 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Sword")
        {
            thisAnim.SetBool("StopAnim", true);                     //stops animation if swords it, this is put before WaitForHitSword so animation stops at the right time
            hitSword = true;                                        //if hit sword dont deal damage
        }

        if (other.transform.tag == "Enemy" && !hitSword && playerSword && !swinging)                     //if this sword belongs to the player and hits enemy give damage
        {
            swinging = true;                                  //Prevents dealing damage twice;                                        
            hitOpponent = true;                                        //tell ienumerator WaitForHitSword that enemy was hit
            StartCoroutine(WaitForHitSword(other.transform));
        }

        if (other.transform.tag == "BossEnemy" && !hitSword && playerSword && !swinging)                     //if this sword belongs to the player and hits enemy give damage
        {
            swinging = true;                                  //Prevents dealing damage twice;                                        
            hitOpponent = true;                                        //tell ienumerator WaitForHitSword that enemy was hit
            StartCoroutine(WaitForHitSword(other.transform));
        }

        if (other.transform.tag == "PlayerHitCol" && !hitSword && !playerSword && !swinging)                     //if this sword belongs to the enemy and hits player give damage
        {
            swinging = true;                                  //Prevents dealing damage twice;  
            hitOpponent = true;                                        //tell ienumerator WaitForHitSword that palyer was hit
            StartCoroutine(WaitForHitSword(other.transform));
        }
    }

    void SetCorrectModel()
    {
        //get saved model number
        int modelNum = PlayerInfoScript.playerInfo.swordModelNum;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(modelNum).gameObject.SetActive(true);
    }

    IEnumerator WaitForHitSword(Transform other)                        //Wait a little to see if enemy did block but sword passed through enemy for a second
    {
        yield return new WaitForSeconds(.07f);                           //This is will fix the up, upleft and upright attacks that pass through the enemy for a milisecond before being blockeds
        //print(hitOpponent + " " + hitSword);
        if (hitOpponent && !hitSword)                                   //if animation went through enemy and did not hit sword, deal damage
        {
            if (playerSword)
            {
                other.GetComponent<EnemyController>().health -= damage;
            }
            else other.parent.GetComponent<PlayerController>().health -= damage;
        }

        hitOpponent = false;                                                            //reset variables for next hit
        hitSword = false;

        yield return new WaitForSeconds(.2f);                                          //Makes sure damage isnt dealt again before swing animation is over
        swinging = false;                                                               //reset swinging variable so damage can be dealt again
    }
}
