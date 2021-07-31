using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    GameObject curEnemy;

    public CinematicHandler cinematicHandler;

    public GameObject player;

    Cinemachine.CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = this.GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void FindNextEnemy()             //called by PlayerController when current enemy is killed. FIRST TIME IS CALLED BY ENEMYSPAWNERSCRIPT AFTER FIRST ENEMIES HAVE BEEN SPAWNED
    {
        curEnemy = FindNearEnemies();
        if (curEnemy != null) cam.m_LookAt = curEnemy.transform;
        else Debug.Log("No Enemies found to look at");
    }

    GameObject FindNearEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();
        Collider[] EnemiesColliders = Physics.OverlapSphere(this.transform.position, 30);
        foreach (var hitCollider in EnemiesColliders)                               //Find All enemies in radius
        {
            if (hitCollider.tag == "Enemy")                                 //if tag is Enemy, add them to enemy list
            {
                enemies.Add(hitCollider.gameObject);
            }
        }

        GameObject closest = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject enemy in enemies)                                   //loop through list of Enemies and find closest enemy
        {
            float dist = Vector3.Distance(enemy.transform.position, this.transform.position);
            if (dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        if (closest == null)                                    //If there are no enemies near, look for the Boss Enemy
        {
            enemies = new List<GameObject>();
            EnemiesColliders = Physics.OverlapSphere(this.transform.position, 30);
            foreach (var hitCollider in EnemiesColliders)                               //Find Boss object in array off colliders
            {
                if (hitCollider.tag == "BossEnemy")                                 //if tag is Boss, set it to the closes enemy to look at
                {
                    closest = hitCollider.gameObject;
                    cinematicHandler.StartCinematic();                                  //Start Boss cinematic shot

                    player.GetComponent<PlayerMoveController>().fightingBoss = true;

                    break;
                }
            }
        }

        return closest;
    }
}
