using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessEnemySpawner : MonoBehaviour
{
    public GameObject spawnerHolder;
    List<GameObject> spawnPoints = new List<GameObject>();
    int curEnemyNo = 0;
    int countTilEnemyUpgrade = 0;
    int amountForEnemyUpgrade = 5;

    public GameObject Enemy;

    public PlayerCameraController playerCamController;

    EnemyStats curEnemyStats;

    private void Awake()
    {
        curEnemyStats = new EnemyStats();
    }

    void Start()
    {
        GetSpawnPoints();
    }

    public void SpawnStartingEnemies(int chosenPlayerSpawnPoint)                                        //Called after GetSpawnPoints
    {
        int amountOfEnemies = curEnemyNo + 3;                                                               //takes the cur enemy number and adds it to how many starting enemies are requested (3). This spawns x enemies starting at curEnemyNo
        for (int i = curEnemyNo; i < amountOfEnemies; i++)
        {
            SpawnEnemy(spawnPoints[i].transform.position, spawnPoints[i].transform.position);                               //Spawn in first three enemies, their spawn point and walktoloc is the same
        }

        playerCamController.FindNextEnemy();                                                    //Tells player camera to look at enemy after its been spawned.
    }

    void GetSpawnPoints()
    {
        for (int i = 0; i < spawnerHolder.transform.childCount; i++)                                                 //Get Spawn points in object and put them into list
        {
            spawnPoints.Add(spawnerHolder.transform.GetChild(i).gameObject);
        }

        SpawnStartingEnemies(MovePlayerAtStart.startSpawnNo);                                                       //Spawns starting enemies and passes through the level that player selected
    }

    public void SpawnNextEnemy()                                                        //Called by PlayerController when enemy is killed
    {
        if (curEnemyNo + 2 < spawnPoints.Count - 1) SpawnEnemy(spawnPoints[curEnemyNo + 2].transform.position, spawnPoints[curEnemyNo].transform.position);                 //spawn in next enemy 2 spawnpoints away from actual point and make it walk to actual point
        else if (curEnemyNo < spawnPoints.Count) SpawnEnemy(spawnPoints[0].transform.position, spawnPoints[curEnemyNo].transform.position);                                                     //If there is no spawn point 2 before target spawn point and there are still spawn points, use the first spawn point to loop again
        else
        {
            SpawnEnemy(spawnPoints[1].transform.position, spawnPoints[curEnemyNo].transform.position);
            curEnemyNo = 0;                                                                                     //Once player loops around map, reset curEnemyNo
        }
    }

    void SpawnEnemy(Vector3 spawnPoint, Vector3 walkToLoc)
    {
        GameObject curEnemy = Instantiate(Enemy, spawnPoint, Quaternion.identity);
        curEnemy.AddComponent<EnemyWalkToScript>();
        curEnemy.GetComponent<EnemyWalkToScript>().walkToPoint = walkToLoc;
        GiveEnemyStats(curEnemy);                                                                //Give enemies its Stats using EnemyStats script
        curEnemyNo++;                                               //increase number of enemies spawned count

        countTilEnemyUpgrade++;
        if (countTilEnemyUpgrade >= amountForEnemyUpgrade)                              //After amountForEnemyUpgrade enemies have been killed, upgrade enemies
        {
            MakeEnemiesHarder();                                                //upgrade enemies
            countTilEnemyUpgrade = 0;                                               //reset counter
            amountForEnemyUpgrade += 1;                                             //Make amount of kills necessary for Enemy upgrade increase every time they get upgraded
        }
    }

    void GiveEnemyStats(GameObject curEnemy)
    {
        curEnemy.GetComponent<EnemyController>().health = curEnemyStats.health;

        curEnemy.GetComponent<EnemyFightController>().correctBlockChance = curEnemyStats.correctBlockChance;
        curEnemy.GetComponent<EnemyFightController>().waitTime = curEnemyStats.waitTime;
        curEnemy.GetComponent<EnemyFightController>().attackDamage = curEnemyStats.attackDamage;
        curEnemy.GetComponent<EnemyFightController>().playerResponceTime = curEnemyStats.playerResponceTime;
    }

    void MakeEnemiesHarder()
    {
        curEnemyStats.health += 2;
        curEnemyStats.waitTime -= .005f;
        curEnemyStats.attackDamage += 2;

        if(curEnemyStats.playerResponceTime > 3.5f) curEnemyStats.playerResponceTime -= .005f;
    }
}
