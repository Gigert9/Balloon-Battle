using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissle : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 5;
    public float rotateSpeed = 200;

    public bool isPlayerMissle = false;

    private Rigidbody2D rb;

    private GameObject missleBlimp;
    private GameObject missleBoss;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("player").GetComponent<Transform>();
        missleBlimp = GameObject.Find("MissleBlimp(Clone)");
        missleBoss = GameObject.Find("MissleBoss(Clone)");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isPlayerMissle)
        {
            if (rb.position.x <= -13 || rb.position.y <= -8 || rb.position.y >= 8)
            {
                Destroy(this.gameObject);
            }
            else if (missleBlimp != null)
            {
                if (rb.position.x > missleBlimp.GetComponent<Transform>().position.x - 2)
                {
                    rb.velocity = -transform.right * moveSpeed;

                    Vector2 pos = this.transform.position;
                    Vector2 blimpPos = missleBlimp.GetComponent<Transform>().position;

                    pos.y = blimpPos.y + 0.8f;
                    this.transform.position = pos;
                }
            }
            else if (missleBoss != null)
            {
                if (rb.position.x > missleBoss.GetComponent<Transform>().position.x - 2)
                {
                    rb.velocity = -transform.right * moveSpeed;

                    Vector2 pos = this.transform.position;
                    Vector2 bossPos = missleBoss.GetComponent<Transform>().position;

                    pos.y = bossPos.y - 1.3f;
                    this.transform.position = pos;
                }
            }

            if (rb.position.x >= -6.5f)
            {
                if (missleBlimp != null)
                {
                    if (rb.position.x <= missleBlimp.GetComponent<Transform>().position.x - 2)
                    {
                        Vector2 direction = (Vector2)player.position - rb.position;

                        direction.Normalize();

                        float rotateAmount = Vector3.Cross(direction, -transform.right).z;

                        rb.angularVelocity = -rotateAmount * rotateSpeed;

                        rb.velocity = -transform.right * moveSpeed;
                    }
                }
                else if (missleBoss != null)
                {
                    if (rb.position.x <= missleBoss.GetComponent<Transform>().position.x - 2)
                    {
                        Vector2 direction = (Vector2)player.position - rb.position;

                        direction.Normalize();

                        float rotateAmount = Vector3.Cross(direction, -transform.right).z;

                        rb.angularVelocity = -rotateAmount * rotateSpeed;

                        rb.velocity = -transform.right * moveSpeed;
                    }
                }
                else
                {
                    Vector2 direction = (Vector2)player.position - rb.position;

                    direction.Normalize();

                    float rotateAmount = Vector3.Cross(direction, -transform.right).z;

                    rb.angularVelocity = -rotateAmount * rotateSpeed;

                    rb.velocity = -transform.right * moveSpeed;
                }
            }
            else
            {
                rb.velocity = -transform.right * moveSpeed;

                rb.angularVelocity = 0;
            }
        }
        //If the missle came from the player...
        else if (isPlayerMissle)
        {
            if (rb.position.x >= 13 || rb.position.y >= 8 || rb.position.y <= -8)
            {
                Destroy(this.gameObject);
            }

            if(rb.position.x <= -1)
            {
                Vector2 direction = (Vector2)mousePos - rb.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.right).z;

                rb.angularVelocity = -rotateAmount * rotateSpeed;

                rb.velocity = transform.right * moveSpeed;
            }
            else
            {
                rb.velocity = transform.right * moveSpeed;

                rb.angularVelocity = 0;
            }


        }
    }
}
