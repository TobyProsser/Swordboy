using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GG.Infrastructure.Utils.Swipe;

public class MoveOnSwipe_EightDirections : MonoBehaviour
{
    [Header("Available movements:")]

    [SerializeField] private bool _up = true;
    [SerializeField] private bool _down = true;
    [SerializeField] private bool _left = true;
    [SerializeField] private bool _right = true;
    [SerializeField] private bool _upLeft = true;
    [SerializeField] private bool _upRight = true;
    [SerializeField] private bool _downLeft = true;
    [SerializeField] private bool _downRight = true;

    public FightScript fightScript;
    public OnSwordScript onSwordScript;
    public Animator thisSwordAnim;

    public GameObject swordTrailObject;

    bool canAttack = true;
    public float attackDelay;                                   //Amount of time between attacks
    public Slider attackDelaySlider;

    private void Awake()
    {
        swordTrailObject.SetActive(false);
        attackDelaySlider.gameObject.SetActive(false);
    }

    private void Start()
    {
        //run get cur sword on playerInfo singleton
        PlayerInfoScript.playerInfo.GetCurSword();
        //set needed values
        attackDelay = PlayerInfoScript.playerInfo.attackDelay;
    }

    public void OnSwipeHandler(string id)
    {
        if (canAttack)
        {
            StartCoroutine(delaySliderController());
            switch (id)
            {
                case DirectionId.ID_UP:
                    MoveUp();
                    break;

                case DirectionId.ID_DOWN:
                    MoveDown();
                    break;

                case DirectionId.ID_LEFT:
                    MoveLeft();
                    break;

                case DirectionId.ID_RIGHT:
                    MoveRight();
                    break;

                case DirectionId.ID_UP_LEFT:
                    MoveUpLeft();
                    break;

                case DirectionId.ID_UP_RIGHT:
                    MoveUpRight();
                    break;

                case DirectionId.ID_DOWN_LEFT:
                    MoveDownLeft();
                    break;

                case DirectionId.ID_DOWN_RIGHT:
                    MoveDownRight();
                    break;
            }
        }
    }

    private void MoveDownRight()
    {
        if (_downRight)
        {
            print("MoveDownRight");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingDownRight");
            thisSwordAnim.SetTrigger("SwingDownRight");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(8);
        }
    }

    private void MoveDownLeft()
    {
        if (_downLeft)
        {
            print("MoveDownLeft");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingDownLeft");
            thisSwordAnim.SetTrigger("SwingDownLeft");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(7);
        }
    }

    private void MoveUpRight()
    {
        if (_upRight)
        {
            print("MoveUpRight");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingUpRight");
            thisSwordAnim.SetTrigger("SwingUpRight");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(6);
        }
    }

    private void MoveUpLeft()
    {
        if (_upLeft)
        {
            print("MoveUpLeft");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingUpLeft");
            thisSwordAnim.SetTrigger("SwingUpLeft");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(5);
        }
    }

    private void MoveRight()
    {
        if (_right)
        {
            print("MoveRight");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingRight");
            thisSwordAnim.SetTrigger("SwingRight");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(4);
        }
    }

    private void MoveLeft()
    {
        if (_left)
        {
            print("MoveLeft");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingLeft");
            thisSwordAnim.SetTrigger("SwingLeft");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(3);
        }
    }

    private void MoveDown()
    {
        if (_down)
        {
            print("MoveDown");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingDown");
            thisSwordAnim.SetTrigger("SwingDown");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(2);
        }
    }

    private void MoveUp()
    {
        if (_up)
        {
            print("MoveUp");
            thisSwordAnim.SetBool("StopAnim", false);

            thisSwordAnim.ResetTrigger("SwingUp");
            thisSwordAnim.SetTrigger("SwingUp");
            StartCoroutine(StopAnimation());

            fightScript.PlayerAction(1);
        }
    }

    IEnumerator StopAnimation()                         //Allows for stopping of current playing animation
    {
        onSwordScript.hitSword = false;                  //turn hit sword to false to deal damage incase blocked last hit
        StartCoroutine(ControlSwordTrail());
        yield return new WaitForSeconds(0.5f);           //if not hit sword play full animation, sword script will stop it earlier if nessessary

        thisSwordAnim.SetBool("StopAnim", true);
    }

    IEnumerator ControlSwordTrail()
    {
        swordTrailObject.SetActive(true);                   //turn on trail renderer
        yield return new WaitForSeconds(0.4f);
        swordTrailObject.SetActive(false);                  //turn off trail renderer after half way thru animation
    }

    IEnumerator delaySliderController()
    {
        print("Sword Active");
        onSwordScript.swordActive = true;                               //enable sword hitBox
        attackDelaySlider.gameObject.SetActive(true);
        canAttack = false;                  //Disable ability to attack

        float sliderValue = 100;            //set slider to full
        float timeLeft = attackDelay;
        attackDelaySlider.maxValue = sliderValue;

        for (float t = 0f; t < attackDelay; t += Time.deltaTime)
        {
            sliderValue = Mathf.Lerp(100, 0, t / attackDelay);                      //go from 100 to 0 at a rate of time divided by the attack delay
            print(sliderValue);
            attackDelaySlider.value = sliderValue;                          //set sliders value to sliderValue
            yield return null;
        }

        //This wait needs to be here or sword active isnt true for long enough to activate swords box collider
        yield return new WaitForSeconds(.3f);
        canAttack = true;                                                       //once done, enable attack again
        attackDelaySlider.gameObject.SetActive(false);
        onSwordScript.swordActive = false;
    }
}
