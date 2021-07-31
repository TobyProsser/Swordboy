using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public float playerMoveSpeed;
    float currentMoveSpeed;
    public float IncreasedMoveSpeed;

    public Camera playerCamera;

    PlayerController playerController;

    bool mouseDownOnEnemy;

    public bool fightingBoss;

    void Awake()
    {
        fightingBoss = false;
        playerController = this.GetComponent<PlayerController>();
        currentMoveSpeed = playerMoveSpeed;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Enemy")
                {
                    if (hit.transform.GetComponent<EnemyController>().atDestination)                    //Only move towards enemy if enemy is at its location
                    {
                        currentMoveSpeed = playerMoveSpeed;
                        print(hit.transform.position + " " + hit.transform.name);
                        StartCoroutine(MoveTo(hit.transform.position));             //If clicked on enemy move towards him
                        mouseDownOnEnemy = true;
                        currentMoveSpeed += IncreasedMoveSpeed;                        //apply speed multipler to move speed, this will be removed when finger is lifted
                    }
                }
                
                //Only lets player click on boss enemy when boss is the only enemy left fightingBoss bool changed by PlayerCameraController
                if (hit.transform.tag == "BossEnemy" && fightingBoss)
                {
                    currentMoveSpeed = playerMoveSpeed;
                    print(hit.transform.position + " " + hit.transform.name);
                    StartCoroutine(MoveTo(hit.transform.position));             //If clicked on enemy move towards him
                    mouseDownOnEnemy = true;
                    currentMoveSpeed += IncreasedMoveSpeed;                        //apply speed multipler to move speed, this will be removed when finger is lifted
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && mouseDownOnEnemy)
        {
            currentMoveSpeed = playerMoveSpeed;                                 //resetPlayerMoveSpeed
            mouseDownOnEnemy = false;
        }
    }

    IEnumerator MoveTo(Vector3 targetPos)
    {
        print(targetPos);
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        float distance = Vector3.Distance(targetPos, this.transform.position);          //get distance between enemy and player
        while (distance > 2)                                                            //while distance is greater than two, move towards enemy
        {
            this.transform.LookAt(targetPos);                                           //Look at enemy

            float step = currentMoveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            distance = Vector3.Distance(targetPos, this.transform.position);

            if (distance < 2) break;

            yield return null;
        }

        playerController.NextToEnemy();                 //Tell playerController that player has moved to an enemies position
    }
}
