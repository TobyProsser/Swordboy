using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkToScript : MonoBehaviour
{
    public Vector3 walkToPoint;

    private void Start()                                    //created and Called by enemySpawnScript
    {
        this.GetComponent<NavMeshAgent>().SetDestination(walkToPoint);
    }

    private void Update()
    {
        if (walkToPoint != null)
        {
            Vector3 tempPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);                                 //remove y val for calculate to see if enemy is close enough to destination point
            Vector3 tempWalkTo = new Vector3(walkToPoint.x, 0, walkToPoint.z);
            if (Mathf.Abs(Vector3.Distance(tempPos, tempWalkTo)) < .005f)  //once point is reached
            {
                this.GetComponent<EnemyController>().atDestination = true;
                Destroy(this.GetComponent<NavMeshAgent>());                                 //Remove navmesh agent, it will no longer be used
                Destroy(this);                                                              //delete this script
            }
        }
    }
}
