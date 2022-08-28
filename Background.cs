using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public bool isbackground1;
    public bool isbackground2;
    public bool isbackground3;
    public bool isbackground4;

    public bool didMove1;

    public float movespeed1;
    public float movespeed2;
    public float movespeed3;
    public float movespeed4;

    Vector2 background2Pos;
    Vector2 background3Pos;
    Vector2 background4Pos;
    Vector2 startingPos;

    void Start()
    {
        startingPos = transform.localPosition;
    }

    void Update()
    {
        MoveBackground();
    }

    void MoveBackground()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            if (isbackground1)
            {
                float rearPosX = transform.localPosition.x;
            }

            if (isbackground2)
            {
                //Second to Back Image
                float secondPosX = transform.localPosition.x;

                secondPosX -= movespeed2 * Time.deltaTime;
                background2Pos.x = secondPosX;
                transform.localPosition = background2Pos;

                if (transform.localPosition.x <= -27)
                {
                    transform.localPosition = new Vector2(27, 0);
                }
            }

            if (isbackground3)
            {
                //Third Layer from Back Image
                float thirdPosX = transform.localPosition.x;

                thirdPosX -= movespeed3 * Time.deltaTime;
                background3Pos.x = thirdPosX;
                transform.localPosition = background3Pos;

                if (transform.localPosition.x <= -27)
                {
                    transform.localPosition = new Vector2(27, 0);
                }
            }

            if (isbackground4)
            {
                //Top Layer
                float fourthPosX = transform.localPosition.x;

                fourthPosX -= movespeed4 * Time.deltaTime;
                background4Pos.x = fourthPosX;
                transform.localPosition = background4Pos;

                if (transform.localPosition.x <= -27)
                {
                    transform.localPosition = new Vector2(27, 0);
                }
            }
        }
    }
}
