using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public int health;                      //given by enemySpawnerScript, accessed by Player's OnSwordScript

    Slider healthSlider;

    [HideInInspector]
    public GameObject player;                      //given by fight script
    [HideInInspector]
    public bool fighting;                      //tells health slider to start updating

    public bool atDestination = false;                                  //Accessed by EnemyWalkToScript when done walking

    public void FightStart()                   //Started by fightScript
    {
        healthSlider = this.transform.GetChild(1).GetChild(0).GetComponent<Slider>();    //gets canvas in gameobject, then gets the slider in that canvas
        healthSlider.maxValue = health;
        healthSlider.value = health;

        fighting = true;                      //tells health slider to start updating
    }

    private void LateUpdate()
    {
        if (fighting)
        {
            healthSlider.value = health;

            if (health <= 0)
            {
                if (player != null) player.GetComponent<PlayerController>().CurrentEnemyDestoryed(this.gameObject);                    //Tells PlayerController that curEnemy has died
                Destroy(this.gameObject);
            }
        }
    }
}
