using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject spawnerHolder;
    List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject lastSpawnPoint;

    public GameObject mainLocationSpawnHolder;                              //holds spawn points at last location(ex. spawn points in castle)
    bool lastEnemiesSpawned;

    int curEnemyNo = 0;

    public int numEnemiesInEachLevel;                                      //tells the amount of enemies between each respawnPoint, used for not spawning enemies behind players when loading into other respawn points other than the first one

    public GameObject Enemy;
    public GameObject bossEnemy;

    public PlayerCameraController playerCamController;

    // Start is called before the first frame update
    void Start()
    {
        if (MovePlayerAtStart.startSpawnNo < 6) GetSpawnPoints();                               //If player clicks right on castle from menu, just spawn last enemies
        else
        {
            SpawnLastEnemies();
            lastEnemiesSpawned = true;
        }
    }

    public void SpawnStartingEnemies(int chosenPlayerSpawnPoint)                                        //Called after GetSpawnPoints
    {
        curEnemyNo = chosenPlayerSpawnPoint * numEnemiesInEachLevel;                                      //detects where to spawn the first enemies. Takes the current player spawn point and multiplies it by the number of enemies per spawn point to get the next spawn point for enemy
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
        else if (curEnemyNo < spawnPoints.Count) SpawnEnemy(lastSpawnPoint.transform.position, spawnPoints[curEnemyNo].transform.position);                                                     //If there is no spawn point 2 before target spawn point and there are still spawn points, use spawn point in castle
        else print("END");

        if (curEnemyNo + 2 >= spawnPoints.Count && !lastEnemiesSpawned)
        {
            SpawnLastEnemies();
            lastEnemiesSpawned = true;
        }
    }

    void SpawnEnemy(Vector3 spawnPoint, Vector3 walkToLoc)
    {
        GameObject curEnemy = Instantiate(Enemy, spawnPoint, Quaternion.identity);
        curEnemy.AddComponent<EnemyWalkToScript>();
        curEnemy.GetComponent<EnemyWalkToScript>().walkToPoint = walkToLoc;
        GiveEnemyStats(curEnemy);                                                                //Give enemies its Stats using EnemyStats script
        curEnemyNo++;                                               //increase number of enemies spawned count
    }

    void GiveEnemyStats(GameObject curEnemy)
    {
        EnemyStats curEnemyStats = new EnemyStats();
        curEnemy.GetComponent<EnemyController>().health = curEnemyStats.health;

        curEnemy.GetComponent<EnemyFightController>().correctBlockChance = curEnemyStats.correctBlockChance;
        curEnemy.GetComponent<EnemyFightController>().waitTime = curEnemyStats.waitTime;
        curEnemy.GetComponent<EnemyFightController>().attackDamage = curEnemyStats.attackDamage;
        curEnemy.GetComponent<EnemyFightController>().playerResponceTime = curEnemyStats.playerResponceTime;
    }

    void SpawnLastEnemies()
    {
        for (int i = 0; i < mainLocationSpawnHolder.transform.childCount; i++)                                                 //Get Spawn points in object and put them into list
        {
            if (i < mainLocationSpawnHolder.transform.childCount - 1) SpawnLastEnemy(mainLocationSpawnHolder.transform.GetChild(i).position, mainLocationSpawnHolder.transform.GetChild(i).position);         //Spawn in Last enemies. Spawn point is same as walkToPoint 
            else SpawnBoss(mainLocationSpawnHolder.transform.GetChild(i).position);                    //Spawn boss on last spawnPoint
        }

        playerCamController.FindNextEnemy();                                                    //Tells player camera to look at enemy after its been spawned.
    }

    void SpawnLastEnemy(Vector3 spawnPoint, Vector3 walkToLoc)
    {
        GameObject curEnemy = Instantiate(Enemy, spawnPoint, Quaternion.identity);
        curEnemy.AddComponent<EnemyWalkToScript>();
        curEnemy.GetComponent<EnemyWalkToScript>().walkToPoint = walkToLoc;
        GiveEnemyStats(curEnemy);                                                                //Give enemies its Stats using EnemyStats script
    }

    void SpawnBoss(Vector3 spawnPoint)
    {
        GameObject curEnemy = Instantiate(bossEnemy, spawnPoint, Quaternion.identity);
        curEnemy.transform.Rotate(0, 180, 0);
        GiveEnemyStats(curEnemy);
    }
}
