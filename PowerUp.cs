using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float moveSpeed = 8f;

    public bool isHealth = false;
    public bool isAmmo = false;
    public bool isWeapon = false;
    public bool isExtraLife = false;
    public bool isMissle = false;

    public AudioSource pickUpAudio;

    GameObject powerUp;

    //Cursor Icon Variables:
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        powerUp = this.gameObject;

        pickUpAudio = GameObject.Find("WorldBottom").GetComponent<AudioSource>();

        hotSpot = new Vector2(cursorTexture.height / 2, cursorTexture.width / 2);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            MovePowerUp();
        }

        CheckDestroy();

        //CheckPaused();
    }

    void CheckPaused()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPaused)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void CheckDestroy()
    {
        if (!GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            Destroy(this.gameObject, 0.5f);
        }
    }

    void MovePowerUp()
    {
        if (isExtraLife)
        {
            moveSpeed = 16f;
            Vector2 pos = this.transform.localPosition;
            pos.x -= moveSpeed * Time.deltaTime;
            powerUp.transform.localPosition = pos;

            if(pos.x < -14f)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Vector2 pos = this.transform.localPosition;
            pos.x -= moveSpeed * Time.deltaTime;
            powerUp.transform.localPosition = pos;

            if (pos.x < -14f)
            {
                if (isAmmo)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAmmoPowerUp = false;
                }
                else if (isHealth)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHealthPowerUp = false;
                }
                else if (isWeapon)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAmmoPowerUp = false;
                }
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isAmmo)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAmmoPowerUp = false;
            }
            else if (isHealth)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHealthPowerUp = false;
            }
            else if (isWeapon)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnWeaponPowerUp = false;
            }
            else if (isMissle)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnMisslePowerUp = false;
            }

            if(pickUpAudio != null)
            {
                pickUpAudio.Play();
            }

            Destroy(this.gameObject);
        }
    }

    //Cursor Icon Functions:
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
