using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjectScript : MonoBehaviour
{
    public int respawnNumber;

    //Run on start so player info has time to load information
    private void Start()
    {
        if (respawnNumber <= PlayerInfoScript.playerInfo.level)                                          //if this respawn point has already been reached
        {
            Destroy(this.GetComponent<BoxCollider>());                                      //destroy collider so it doesnt add another level has been passed
            Destroy(this);
        }
    }
}
