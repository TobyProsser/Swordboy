using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmoryController : MonoBehaviour
{
    public Animator camAnimator;

    public GameObject SwordHolder;
    public GameObject Closet;

    public GameObject mainPanel;
    public GameObject SwordPanel;
    public GameObject ClosetPanel;

    public GameObject Door1Holder;
    public GameObject Door2Holder;

    GameObject curSelectedSword;
    GameObject curSelectedSkin;

    int viewNum = 0;

    public static bool died;                //Called by playerController when died. Changes how scene loads up if scene is opening because player died

    private void Awake()
    {
        SwordPanel.SetActive(false);
        ClosetPanel.SetActive(false);
        mainPanel.SetActive(true);

        curSelectedSword = null;
    }

    private void Start()
    {
        if (died)
        {
            OnStartIfDied();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "SwordHolder")
                {
                    camAnimator.ResetTrigger("MoveToSwordHolder");
                    camAnimator.SetTrigger("MoveToSwordHolder");

                    SwordHolder.transform.GetComponent<BoxCollider>().enabled = false;                                                          //Disable box collider so player can click on swords


                    SwordPanel.SetActive(true);
                    mainPanel.SetActive(false);
                }

                if (hit.transform.tag == "Closet")
                {
                    camAnimator.ResetTrigger("MoveToCloset");
                    camAnimator.SetTrigger("MoveToCloset");

                    Closet.transform.GetComponent<BoxCollider>().enabled = false;                                                          //Disable box collider so player can click on skins

                    ClosetPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    OpenDoors();
                }

                if (hit.transform.tag == "Sword")
                {
                    //If curSelectedSword is not equal to null, move the currently selected sword back before moving new sword
                    //Dont let the player move back a sword that is selected because one sword should always be selected
                    if (curSelectedSword != null && curSelectedSword != hit.transform.gameObject)
                    {
                        //get animator from child of sword
                        Animator tempAnim1 = curSelectedSword.transform.GetComponent<Animator>();
                        //Move last selected sword in
                        tempAnim1.ResetTrigger("MoveSwordIn");
                        tempAnim1.ResetTrigger("MoveSwordOut");
                        //Move last selected sword in
                        tempAnim1.SetTrigger("MoveSwordIn");

                        //Assign curSword to clicked on sword
                        curSelectedSword = hit.transform.gameObject;
                        //get animator from child of sword
                        Animator tempAnim = curSelectedSword.transform.GetComponent<Animator>();
                        tempAnim.ResetTrigger("MoveSwordOut");
                        tempAnim.ResetTrigger("MoveSwordIn");
                        //Move Sword out
                        tempAnim.SetTrigger("MoveSwordOut");
                    }
                    else if (curSelectedSword == null)
                    {
                        //set curSelectedSword to the clicked on sword
                        curSelectedSword = hit.transform.gameObject;
                        //get animator from child of sword
                        Animator tempAnim = curSelectedSword.transform.GetComponent<Animator>();
                        tempAnim.ResetTrigger("MoveSwordOut");
                        tempAnim.ResetTrigger("MoveSwordIn");
                        tempAnim.SetTrigger("MoveSwordOut");
                    }
                }

                if (hit.transform.tag == "Skin")
                {
                    //If curSelectedSkin is not equal to null, move the currently selected skin back before moving new skin
                    //Dont let the player move back a skin that is selected because one skin should always be selected
                    if (curSelectedSkin != null && curSelectedSkin != hit.transform.gameObject)
                    {
                        //get animator from child of skin
                        Animator tempAnim1 = curSelectedSkin.transform.GetComponent<Animator>();
                        //Move last selected skin in
                        tempAnim1.ResetTrigger("MoveIn");
                        tempAnim1.ResetTrigger("MoveOut");
                        //Move last selected skin in
                        tempAnim1.SetTrigger("MoveIn");

                        //Assign curSword to clicked on skin
                        curSelectedSkin = hit.transform.gameObject;
                        //get animator from child of skin
                        Animator tempAnim = curSelectedSkin.transform.GetComponent<Animator>();
                        tempAnim.ResetTrigger("MoveOut");
                        tempAnim.ResetTrigger("MoveIn");
                        //Move skin out
                        tempAnim.SetTrigger("MoveOut");
                    }
                    else if (curSelectedSkin == null)
                    {
                        //set curSelectedSword to the clicked on skin
                        curSelectedSkin = hit.transform.gameObject;
                        //get animator from child of skin
                        Animator tempAnim = curSelectedSkin.transform.GetComponent<Animator>();
                        tempAnim.ResetTrigger("MoveOut");
                        tempAnim.ResetTrigger("MoveIn");
                        tempAnim.SetTrigger("MoveOut");
                    }
                }
            }
        }
    }


    public void SwordBackButton()
    {
        //If curSelectedSword is not equal to null, move the currently selected sword back before moving camera back
        if (curSelectedSword != null)
        {
            //get animator from child of sword
            Animator tempAnim1 = curSelectedSword.transform.GetComponent<Animator>();
            tempAnim1.ResetTrigger("MoveSwordIn");
            tempAnim1.SetTrigger("MoveSwordIn");

            //Set sword num on player info to the curselected swords modelnum when back button is pressed
            PlayerInfoScript.playerInfo.swordNum = curSelectedSword.GetComponent<SwordInfoScript>().swordModelNum;
        }

        SwordPanel.SetActive(false);
        //re-enable swordHolder hit box
        SwordHolder.transform.GetComponent<BoxCollider>().enabled = true;                                                          //enable box collider so player can click swordHolder again

        camAnimator.ResetTrigger("MoveBackFromSwordHolder");
        camAnimator.SetTrigger("MoveBackFromSwordHolder");

        mainPanel.SetActive(true);
    }

    void OnStartIfDied()
    {
        lookRight();
        PlayerInfoScript.playerInfo.lives--;
        died = false;
    }

    public void ClosetBackButton()
    {
        //If curSelectedSkin is not equal to null, move the currently selected skin back before moving camera back
        if (curSelectedSkin != null)
        {
            //get animator from child of sword
            Animator tempAnim1 = curSelectedSkin.transform.GetComponent<Animator>();
            tempAnim1.ResetTrigger("MoveIn");
            tempAnim1.SetTrigger("MoveIn");

            //Set skin num on player info to the curselected skin num when back button is pressed
            PlayerInfoScript.playerInfo.playerSkin = curSelectedSkin.GetComponent<OnSkinScript>().skinNum;
        }

        ClosetPanel.SetActive(false);
        CloseDoors();

        camAnimator.ResetTrigger("MoveBackFromCloset");
        camAnimator.SetTrigger("MoveBackFromCloset");

        Closet.transform.GetComponent<BoxCollider>().enabled = true;                                                          //enable box collider so player can click closet again


        mainPanel.SetActive(true);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void lookLeft()
    {
        viewNum--;
        if (viewNum == 0) camAnimator.SetTrigger("MoveFromTableToInitial");
    }

    public void lookRight()
    {
        viewNum++;

        if (viewNum == 1) camAnimator.SetTrigger("MoveToTable");
    }

    void OpenDoors()
    {
        Door1Holder.GetComponent<Animator>().ResetTrigger("Open");
        Door2Holder.GetComponent<Animator>().ResetTrigger("Open");

        Door1Holder.GetComponent<Animator>().SetTrigger("Open");
        Door2Holder.GetComponent<Animator>().SetTrigger("Open");
    }

    void CloseDoors()
    {
        Door1Holder.GetComponent<Animator>().ResetTrigger("Close");
        Door2Holder.GetComponent<Animator>().ResetTrigger("Close");

        Door1Holder.GetComponent<Animator>().SetTrigger("Close");
        Door2Holder.GetComponent<Animator>().SetTrigger("Close");
    }
}
