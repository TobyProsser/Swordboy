using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicHandler : MonoBehaviour
{
    public Transform playerMoveToPoint;
    public GameObject cam;

    public GameObject playerCam;

    public float lengthOfCamAnim;
    public GameObject player;

    private void Awake()
    {
        cam.SetActive(false);
    }
    public void StartCinematic()                            //Called by PlayerCameraController when it looks at boss for the first time
    {
        player.transform.position = new Vector3(player.transform.position.x, -4, player.transform.position.z);                  //Move player below ground to remove him from shot
        playerCam.SetActive(false);
        cam.SetActive(true);                                                    //Switch to using new camera
        cam.transform.GetComponent<Animation>().Play();

        StartCoroutine(SwitchBackToPlayerCam());
    }

    IEnumerator SwitchBackToPlayerCam()
    {
        yield return new WaitForSeconds(lengthOfCamAnim);                                 //wait for animation to play

        playerCam.SetActive(true);
        cam.SetActive(false);

        player.transform.position = playerMoveToPoint.position;
        player.transform.Rotate(0, 180, 0);
    }
}
