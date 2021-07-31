using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    //All of enemies stats. Given to enemy through EnemySpawnerScript
    public int health = 30;

    public int correctBlockChance = 1;
    public float waitTime = 1;                                //Time inbetween attacks
    public float attackDamage = 10;

    public float playerResponceTime = 1;                      //Time that player has to react to incoming attack
}
