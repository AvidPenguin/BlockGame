using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool rotatingEnemy;
    public float rotationSpeed;
    public bool movingEnemy;
    public float moveSpeed;
    public bool moveBack;
    public float moveProgress;
    public float moveDistance;
    public bool moveLR;
    public bool moveUD;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotatingEnemy)
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
        if (movingEnemy)
        {
            if (moveLR)
            {
                if (!moveBack)
                {
                    transform.Translate(moveSpeed, 0, 0);
                }
                else
                {
                    transform.Translate(-moveSpeed, 0, 0);
                }

                moveProgress += moveSpeed;
            }
            if (moveUD)
            {
                if (!moveBack)
                {
                    transform.Translate(0, 0, moveSpeed);
                }
                else
                {
                    transform.Translate(0, 0, -moveSpeed);
                }

                moveProgress += moveSpeed;
            }
            if (moveProgress > moveDistance)
            {
                moveBack = !moveBack;
                moveProgress = 0;
            }
        }
    }
}
