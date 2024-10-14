using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public GameController gc;
    public PlayerController pc;

    public bool moveQueued;
    private readonly float moveSpeed = 0.5f;    // Has to be a divisible of 50 (1f ,0.5f etc)
    public bool isMoving;
    public MoveDirections moveDirection;
    public float moveProgress;

    public bool rotateQueued;
    private readonly float rotateSpeed = 1.8f;  // Has to be a divisible of 90 (1f ,0.9f etc)
    public bool isRotating;
    public Rotations rotateDirection;
    public float rotateProgress;

    public GameObject doors;                    // The parent empty GameObject containing all the doors
    public Vector3 playerPOS;                  // Used for storing data during rotations

    public enum MoveDirections
    {
        NextLevel,
        PreviousLevel
    }
    public enum Rotations
    {
        Up, Down, Left, Right, Clockwise, CounterClockwise
    }

    private void Start()
    {
        rotateQueued = false;
        moveQueued = false;
    }

    private void FixedUpdate()
    {
        if (moveQueued)
        {
            gc.playerController.gameObject.SetActive(false);
            if (moveDirection == MoveDirections.NextLevel)
            {
                isMoving = true;
                gc.allLevels.transform.Translate(-moveSpeed, 0, 0);
                moveProgress += moveSpeed;

                if (moveProgress == (50))
                {
                    gc.level++;
                    moveQueued = false;
                    moveProgress = 0;
                    isMoving = false;
                    gc.playerController.transform.position = new Vector3(0, 5f, 0);      //Sets new position to player
                    gc.playerController.gameObject.SetActive(true);                     //Reinstates the player to view
                    gc.cube = gc.levels[gc.level];
                    gc.SaveProgress();
                }
            }
            if (moveDirection == MoveDirections.PreviousLevel)
            {
                isMoving = true;
                gc.allLevels.transform.Translate(moveSpeed, 0, 0);
                moveProgress += moveSpeed;

                if (moveProgress == (50))
                {
                    gc.level--;
                    moveQueued = false;
                    moveProgress = 0;
                    isMoving = false;
                    gc.playerController.transform.position = new Vector3(0, 5f, 0);      //Sets new position to player
                    gc.playerController.gameObject.SetActive(true);                     //Reinstates the player to view
                    gc.cube = gc.levels[gc.level];
                }
            }
        }

        if (rotateQueued)
        {
            isRotating = true;

            if (rotateDirection == Rotations.Right)
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.forward, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.forward, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;                                     //Sets bool for rotation check
                    playerPOS.x = -5f;                                      //Moves player to new position
                    gc.playerController.transform.position = playerPOS;     //Sets new position to player
                    gc.playerController.scaleRightRevertQueued = true;
                    
                }
            }
            if (rotateDirection == Rotations.Left)
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.back, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.back, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;                                     //Sets bool for rotation check
                    playerPOS.x = 5f;                                       //Moves player to new position
                    gc.playerController.transform.position = playerPOS;     //Sets new position to player
                    gc.playerController.scaleLeftRevertQueued = true;
                }
            }
            if (rotateDirection == Rotations.Down)
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.right, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.right, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;                                     //Sets bool for rotation check
                    playerPOS.z = 5f;                                       //Moves player to new position
                    gc.playerController.transform.position = playerPOS;     //Sets new position to player
                    gc.playerController.scaleDownRevertQueued = true;
                }
            }
            if (rotateDirection == Rotations.Up)
            {
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.left, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.left, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;                                     //Sets bool for rotation check
                    playerPOS.z = -5f;                                      //Moves player to new position
                    gc.playerController.transform.position = playerPOS;     //Sets new position to player
                    gc.playerController.scaleUpRevertQueued = true;
                }
            }
            if (rotateDirection == Rotations.Clockwise)
            {
                doors.SetActive(false);
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.up, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.up, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;
                    doors.SetActive(true);
                }
            }
            if (rotateDirection == Rotations.CounterClockwise)
            {
                doors.SetActive(false);
                gc.cube.transform.RotateAround(gc.cube.transform.position, Vector3.down, rotateSpeed);
                gc.playerController.transform.RotateAround(gc.cube.transform.position, Vector3.down, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                                   //Ends rotation
                    rotateProgress = 0;                                     //Resets counter for next rotation
                    isRotating = false;
                    doors.SetActive(true);
                }
            }
        }
    }

    public void Rotate(Rotations direction)
    {
        PlayerPrefs.SetInt("totalrotations", PlayerPrefs.GetInt("totalrotations") + 1);
        PlayerPrefs.SetInt("currentrotations", PlayerPrefs.GetInt("currentrotations") + 1);
        if (gc.soundOn)
        {
            gc.rotateSound.Play();
        }
        rotateDirection = direction;                                        //Stores the rotation orientation
        rotateQueued = true;                                                //Tells the controller to start a rotation
        playerPOS = pc.gameObject.transform.position;                       //Stores the player position
    }

    public void SetLevel(int i)
    {
        Vector3 temp = gc.allLevels.transform.position;
        temp.x = -(i * 50);
        gc.allLevels.transform.position = temp;
        gc.level = i;
        gc.cube = gc.levels[gc.level];
        
    }
}
