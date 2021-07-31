using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSkinScript : MonoBehaviour
{
    public int skinNum;
    public int unlockLevel;

    private void Start()
    {
        checkIfUnlocked();
    }

    void checkIfUnlocked()
    {
        if (unlockLevel >= PlayerInfoScript.playerInfo.level) this.gameObject.SetActive(false);
        else this.gameObject.SetActive(true);
    }
}
