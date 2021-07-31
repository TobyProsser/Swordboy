using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookScript : MonoBehaviour
{
    GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) this.transform.LookAt(player.transform.position);
    }
}
