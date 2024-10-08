using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public GameController gc;
    public PlayerController pc;

    public bool moveQueued;
    private readonly float moveSpeed = 0.5f;    // Has to be a divisible of 50 (1f ,0.5f etc)
    public bool isMoving;
    public string moveDirection;
    public float moveProgress;
    public int movesToDo;

    public bool rotateQueued;
    private readonly float rotateSpeed = 1.8f;  // Has to be a divisible of 90 (1f ,0.9f etc)
    public bool isRotating;
    public string rotateDirection;
    public float rotateProgress;



    public Vector3 playerPOS;                  // Used for storing data during rotations

    private void Start()
    {
        movesToDo = 1;
        rotateQueued = false;
        moveQueued = false;
    }

    private void FixedUpdate()
    {
        if (moveQueued)
        {
            isMoving = true;
            gc.allLevels.transform.Translate(-moveSpeed, 0, 0);
            moveProgress += moveSpeed;

            if (moveProgress == (50 * movesToDo))
            {
                moveQueued = false;
                moveProgress = 0;
                isMoving = false;
                gc.player.transform.position = new Vector3(0, 5.125f, 0);      //Sets new position to player
                gc.player.gameObject.SetActive(true);          //Reinstates the player to view
                movesToDo = 1;
            }
        }

        if (rotateQueued)
        {
            isRotating = true;

            if (rotateDirection == "right")
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.forward, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.forward, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.x = -5f;                       //Moves player to new position
                    gc.player.transform.position = playerPOS;      //Sets new position to player
                    gc.player.scaleRightRevertQueued = true;
                }
            }
            if (rotateDirection == "left")
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.back, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.back, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.x = 5f;                       //Moves player to new position
                    gc.player.transform.position = playerPOS;      //Sets new position to player
                    gc.player.scaleLeftRevertQueued = true;
                }
            }
            if (rotateDirection == "down")
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.right, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.right, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.z = 5f;                       //Moves player to new position
                    gc.player.transform.position = playerPOS;      //Sets new position to player
                    gc.player.scaleDownRevertQueued = true;
                }
            }
            if (rotateDirection == "up")
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.left, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.left, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.z = -5f;                          //Moves player to new position
                    gc.player.transform.position = playerPOS;      //Sets new position to player
                    gc.player.scaleUpRevertQueued = true;
                }
            }
            if (rotateDirection == "clockwise")
            {
                gc.doors.SetActive(false);
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.up, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.up, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;
                    gc.doors.SetActive(true);
                }
            }
            if (rotateDirection == "counterclockwise")
            {
                gc.doors.SetActive(false);
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.down, rotateSpeed);
                gc.player.transform.RotateAround(gc.cube.transform.position, Vector3.down, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;
                    gc.doors.SetActive(true);
                }
            }
        }
    }

    public void Rotate(string direction) //Right: Z-, Left, Z+
    {
        if (gc.soundOn)
        {
            gc.rotateSound.Play();
        }
        rotateDirection = direction;                                        //Stores the rotation orientation
        rotateQueued = true;                                             //Tells the controller to start a rotation
        playerPOS = pc.gameObject.transform.position;                       //Stores the player position
    }
}
