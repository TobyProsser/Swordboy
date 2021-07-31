using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript playerInfo;
    public int playerSkin;
    public int experience;
    public int swordNum;

    public int bestEndlessScore;

    //Accessed by StageControllers ie. CastleStageController
    public int level = 6;
    //Accessed by levelManager
    public int lives = 1;

    //Cur Sword values. Given when GetCurSword is ran
    public string swordName;

    public int damage;
    public float attackDelay;

    public int swordModelNum;

    private void Awake()
    {
        if (playerInfo == null)
        {
            playerInfo = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerInfo != this)
        {
            Destroy(gameObject);
        }

        //load information as soon as possible
        Load();
    }

    //Ran by onSwordScript
    public void GetCurSword()
    {
        //Open cur sword numbers save file
        if (File.Exists(Application.persistentDataPath + "/Sword" + swordNum + " .dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Sword" + swordNum + " .dat", FileMode.Open);
            SwordData data = (SwordData)bf.Deserialize(file);
            file.Close();

            swordName = data.swordName;
            damage = data.damage;
            attackDelay = data.attackDelay;
            swordModelNum = data.swordModelNum;
        }
    }

    private void OnDisable()
    {
        //Save player info whenever disabled
        Save();
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
        else file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.playerSkin = playerSkin;
        data.experience = experience;
        data.swordNum = swordNum;

        data.bestEndlessScore = bestEndlessScore;

        data.level = level;
        data.lives = lives;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerSkin = data.playerSkin;
            experience = data.experience;
            swordNum = data.swordNum;

            bestEndlessScore = data.bestEndlessScore;

            level = data.level;
            lives = data.lives;
        }
    }

    [Serializable]
    class PlayerData
    {
        public int playerSkin;
        public int experience;
        public int swordNum;

        public int bestEndlessScore;

        public int level;
        public int lives;
    }
}
