using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SwordInfoScript : MonoBehaviour
{
    public string swordName;

    public int damage;
    public float attackDelay;

    public int swordModelNum;

    public bool unlocked;

    public int levelToUnlock;

    private void OnEnable()
    {
        //load this swords data when enabled
        Load();
        CheckIfUnlocked();
    }

    private void OnDisable()
    {
        //Save this swords data when object is disabled
        Save();    
    }

    void CheckIfUnlocked()
    {
        //TO SEE IF UNLOCKED SEE IF PLAYER IS PASSED A CERTAIN LEVEL
        if (PlayerInfoScript.playerInfo.level >= levelToUnlock) unlocked = true;
        else unlocked = false;
        //if sword is not unlocked turn off hitbox so it cant be clicked and hide its children so you cant see the sword
        if (!unlocked)
        {
            this.GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/Sword" + swordModelNum + " .dat")) file = File.Open(Application.persistentDataPath + "/Sword" + swordModelNum + " .dat", FileMode.Open);
        else file = File.Create(Application.persistentDataPath + "/Sword" + swordModelNum + " .dat");

        SwordData data = new SwordData();
        data.swordName = swordName;
        data.damage = damage;
        data.attackDelay = attackDelay;
        data.swordModelNum = swordModelNum;
        data.unlocked = unlocked;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Sword" + swordModelNum + " .dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Sword" + swordModelNum + " .dat", FileMode.Open);
            SwordData data = (SwordData)bf.Deserialize(file);
            file.Close();

            swordName = data.swordName;
            damage = data.damage;
            attackDelay = data.attackDelay;
            swordModelNum = data.swordModelNum;
            unlocked = data.unlocked;
        }
    }
}
