using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHoldScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string animToCall;           //Set on button, tells script which anim to call for each button
    public Animator swordAnimator;
    public OnSwordScript onSwordScript;

    bool pointerDown;
    float pointerDownTimer;
    bool longClickAchieved;
    public float requiredHoldTime;          //Amount of time required to hold button down

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;                     //If pointer is down, start counting
            if (pointerDownTimer > requiredHoldTime)                //if counter is greater than the time required
            {
                longClickAchieved = true;
                block();                                            //Run block function
            }
        }
    }

    void Reset()
    {
        if (longClickAchieved)
        {
            StartCoroutine(endBlock());                             //If longclick was achieved and finger is lifted, end the block
            longClickAchieved = false;
        }
        pointerDown = false;
        pointerDownTimer = 0;
    }

    void block()
    {
        onSwordScript.swordActive = true;                               //enable sword hitBox to block
        swordAnimator.ResetTrigger(animToCall);
        swordAnimator.SetTrigger(animToCall);
    }

    IEnumerator endBlock()
    {
        swordAnimator.SetBool("StopBlock", true);               //tell animator to stop

        yield return new WaitForSeconds(.15f);                  //wait .15f for animator to stop the animation       

        swordAnimator.SetBool("StopBlock", false);              //reset StopBlock bool so another animation can play

        onSwordScript.swordActive = false;
    }


    /*  FOR DOUBLECLICKING TO BLOCK
     *  
     *  int clicked;                                                //Stores amount of times clicked for double click
    float clickTime = 0;                                            //Time between clicks
    float clickDelay = 1.5f;                                    //Acceptable time between clicks for double click
    bool doubleClickAchieved = false;

     * public void OnPointerDown(PointerEventData eventData)
    {
        clicked++;
        if (clicked == 1) clickTime = Time.time;                                       //If clicked start timer

        if (clicked > 1 && Time.time - clickTime < clickDelay)                         //detects double click
        {
            clicked = 0;                                                                        //Resets double click test varibles
            clickTime = 0;
            doubleClickAchieved = true;                                                         //Let script know that doubleclick was achieved for when user releases button
            block();                                                                            //Run block, this will be stopped when user stops holding button
            print("Block " + (Time.time - (float)clickTime));
        }
        else if (clicked > 2 || Time.time - clickTime > clickDelay)
        {
            clicked = 0;
            print("Took To Long TO Double Click");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (doubleClickAchieved) StartCoroutine(endBlock());                                  //If double click occured and animation started playing. end it when user releases button
    }
    */
}
