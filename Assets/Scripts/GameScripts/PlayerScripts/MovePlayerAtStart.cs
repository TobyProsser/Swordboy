using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerAtStart : MonoBehaviour
{
    public static int startSpawnNo =0;             //given by CastleStageController in main menu. Tells script which spawn point to spawn the player at

    public Transform[] playerSpawnPoints = new Transform[5];

    public GameObject player;

    private void Awake()                //called as soon has scene opens. Moves player to correct spawnPoint
    {
        player.transform.position = playerSpawnPoints[startSpawnNo].position;
    }
}
