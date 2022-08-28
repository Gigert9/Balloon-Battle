using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHealth;

    public float moveSpeed = 5f;
    public float verticalMoveSpeed = 5f;
    public float bounceAmount = 1f;

    public float shootTimer = 0;
    public float shootFrequency = 0.5f;
    public float bulletSpeed;
    public float bossMissleTimer = 0;

    public float shootPauseTime = 2f;

    public bool shotOne;
    public bool shotTwo;
    public bool shotThree;
    public bool didShoot;
    public bool shotAgain;

    public bool isDead = false;

    public bool isLevelOneEnemy;
    public bool isLevelTwoEnemy;
    public bool isLevelThreeEnemy;
    public bool isMissleBlimp;
    public bool isBoss;
    public bool isFlameBoss;
    public bool isSecondaryFlameBoss;

    public AudioSource explosionAudio;
    public AudioSource bulletSound;
    public AudioSource bulletHit;
    public AudioSource missileSound;
    public AudioSource missileHit;
    public AudioSource flameAudio;

    public GameObject enemyBullet;
    public GameObject enemyMissle;
    public GameObject enemyFlame;

    public GameObject blimpExplosion;
    public GameObject heliExplosion;
    public GameObject missileBlimpExplosion;
    public GameObject bigBossExplosion;
    public GameObject mediumBossExplosion;
    public GameObject bulletSparks;
    public GameObject bossSparks;
    public GameObject missileHitBlast;

    public bool movedUp = true;

    public Vector2 startingPosition;
    public Vector2 currentPosition;

    //Cursor Icon Variables:
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
        didShoot = false;
        shotOne = false;
        shotTwo = false;
        shotThree = false;

        explosionAudio = GameObject.Find("GameManager").GetComponent<AudioSource>();

        hotSpot = new Vector2(cursorTexture.height / 2, cursorTexture.width / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        CheckDestroy();
        //CheckPaused();
        MoveFlame();
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

    void Shoot()
    {
        shootTimer += 1 * Time.deltaTime;

        Vector2 pos = this.transform.localPosition;

        if (isLevelOneEnemy)
        {
            if (pos.x <= 9f && !shotAgain)
            {
                if (shootTimer >= shootFrequency && !shotOne && !shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBullet = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);
                    bulletSound.Play();
                    shotOne = true;
                    shootTimer = 0;
                    Destroy(newBullet, 3f);
                }

                if (shootTimer >= shootFrequency && shotOne && !shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBullet = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);
                    bulletSound.Play();
                    shotTwo = true;
                    shootTimer = 0;
                    Destroy(newBullet, 3f);
                }

                if (shootTimer >= shootFrequency && shotOne && shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBullet = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);
                    bulletSound.Play();
                    shotThree = true;
                    didShoot = true;
                    shootTimer = 0;
                    Destroy(newBullet, 3f);
                }

                if (shotOne && shotTwo && shotThree && didShoot && !shotAgain)
                {
                    ShootAgain();
                    shotAgain = true;
                }
            }
        }

        if (isLevelTwoEnemy)
        {
            if(pos.x <= 8f && !shotAgain)
            {
                if(shootTimer >= shootFrequency && !didShoot)
                {
                    GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + .5f), transform.localRotation);
                    newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                    GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                    Destroy(newBulletOne, 3f);
                    Destroy(newBulletTwo, 3f);

                    bulletSound.Play();
                    didShoot = true;
                    shootTimer = 0;
                    ShootAgain();
                }
            }
        }

        if (isLevelThreeEnemy)
        {
            if(pos.x <= 2f && !shotAgain)
            {
                if (shootTimer >= shootFrequency && !shotOne && !shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + .5f), transform.localRotation);
                    newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                    GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                    bulletSound.Play();
                    shotOne = true;
                    shootTimer = 0;
                    Destroy(newBulletOne, 3f);
                    Destroy(newBullet, 3f);
                }

                if (shootTimer >= shootFrequency && shotOne && !shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                    GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                    shotTwo = true;
                    shootTimer = 0;
                    Destroy(newBulletTwo, 3f);
                    Destroy(newBullet, 3f);
                }

                if(shootTimer >= shootFrequency && shotOne && shotTwo && !shotThree && !didShoot)
                {
                    GameObject newBulletThree = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + .5f), transform.localRotation);
                    newBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                    GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                    newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                    shotThree = true;
                    didShoot = true;
                    shootTimer = 0;
                    Destroy(newBulletThree, 3f);
                    Destroy(newBullet, 3f);
                }

                if(shotOne && shotTwo && shotThree && didShoot && !shotAgain)
                {
                    ShootAgain();
                    shotAgain = true;
                }
            }
        }

        if (isMissleBlimp)
        {
            if (pos.x <= 12)
            {
                float shootingFrequency = Random.Range(2, 3);

                if(shootTimer >= shootingFrequency)
                {
                    GameObject newBullet = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y), transform.localRotation);
                    missileSound.Play();
                    shootTimer = 0;
                }
            }
        }

        if (isBoss)
        {
            if (GameObject.Find("player").GetComponent<Player>().currentLevel == 5)
            {
                //The first time we see a boss...

                if (pos.x <= 10 && !shotAgain && !isDead)
                {
                    if (shootTimer >= shootFrequency && !shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1), transform.localRotation);
                        newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1), transform.localRotation);
                        newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletThree = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletFour = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 2), transform.localRotation);
                        newBulletFour.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        shotOne = true;
                        shootTimer = 0;

                        Destroy(newBulletOne, 3f);
                        Destroy(newBulletTwo, 3f);
                        Destroy(newBulletThree, 3f);
                        Destroy(newBulletFour, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1), transform.localRotation);
                        newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        shotTwo = true;
                        shootTimer = 0;

                        Destroy(newBullet, 3f);
                        Destroy(newBulletTwo, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1), transform.localRotation);
                        newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        shotThree = true;
                        didShoot = true;
                        shootTimer = 0;

                        Destroy(newBullet, 3f);
                        Destroy(newBulletTwo, 3f);
                    }

                    if (shotOne && shotTwo && shotThree && didShoot && !shotAgain)
                    {
                        ShootAgain();
                        shotAgain = true;
                    }
                }
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 10)
            {
                //Changing how the boss shoots for the second encounter...

                shootPauseTime = 1f;

                if (pos.x <= 10 && !shotAgain && !isDead)
                {
                    if (shootTimer >= shootFrequency && !shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1f), transform.localRotation);
                        newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject extraNewBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1f), transform.localRotation);
                        extraNewBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                        bulletSound.Play();
                        shotOne = true;
                        shootTimer = 0;
                        Destroy(newBulletOne, 3f);
                        Destroy(newBullet, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1f), transform.localRotation);
                        newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject extraNewBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1f), transform.localRotation);
                        extraNewBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                        bulletSound.Play();
                        shotTwo = true;
                        shootTimer = 0;
                        Destroy(newBulletTwo, 3f);
                        Destroy(newBullet, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletThree = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1f), transform.localRotation);
                        newBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject extraNewBulletThree = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1f), transform.localRotation);
                        extraNewBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                        bulletSound.Play();
                        shotThree = true;
                        didShoot = true;
                        shootTimer = 0;
                        Destroy(newBulletThree, 3f);
                        Destroy(newBullet, 3f);
                    }

                    if (shotOne && shotTwo && shotThree && didShoot && !shotAgain)
                    {
                        ShootAgain();
                        shotAgain = true;
                    }
                }
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 15 || GameObject.Find("player").GetComponent<Player>().currentLevel >= 20)
            {
                //Third Boss shooting method...

                shootPauseTime = 1f;
                bossMissleTimer += 1 * Time.deltaTime;

                if (pos.x <= 10 && !shotAgain && !isDead)
                {
                    //Shoot regular bullets
                    if (shootTimer >= shootFrequency && !shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1f), transform.localRotation);
                        newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject extraNewBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1f), transform.localRotation);
                        extraNewBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                        bulletSound.Play();
                        shotOne = true;
                        shootTimer = 0;
                        Destroy(newBulletOne, 3f);
                        Destroy(newBullet, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && !shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletOne = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1), transform.localRotation);
                        newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletTwo = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1), transform.localRotation);
                        newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletThree = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBulletFour = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 2), transform.localRotation);
                        newBulletFour.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        bulletSound.Play();
                        shotTwo = true;
                        shootTimer = 0;
                        Destroy(newBulletOne, 3f);
                        Destroy(newBulletTwo, 3f);
                        Destroy(newBulletThree, 3f);
                        Destroy(newBulletFour, 3f);
                    }

                    if (shootTimer >= shootFrequency && shotOne && shotTwo && !shotThree && !didShoot)
                    {
                        GameObject newBulletThree = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y + 1f), transform.localRotation);
                        newBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject extraNewBulletThree = Instantiate(enemyBullet, new Vector2(transform.localPosition.x, transform.localPosition.y - 1f), transform.localRotation);
                        extraNewBulletThree.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * bulletSpeed);

                        GameObject newBullet = Instantiate(enemyBullet, transform.localPosition, transform.localRotation);
                        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * (bulletSpeed + 50f));

                        bulletSound.Play();
                        shotThree = true;
                        shootTimer = 0;
                        didShoot = true;
                        Destroy(newBulletThree, 3f);
                        Destroy(newBullet, 3f);
                    }

                    if (shotOne && shotTwo && shotThree && didShoot && !shotAgain)
                    {
                        ShootAgain();
                        shotAgain = true;
                    }
                }

                if (pos.x <= 11 && !isDead)
                {
                    //Shoot missles
                    float shootingFrequency = Random.Range(2, 3);

                    if (bossMissleTimer >= shootingFrequency && !isDead)
                    {
                        GameObject newMissle = Instantiate(enemyMissle, new Vector2(transform.localPosition.x, transform.localPosition.y), transform.localRotation);
                        missileSound.Play();
                        bossMissleTimer = 0;
                    }
                }
            }
        }

        if (isFlameBoss || isSecondaryFlameBoss)
        {
            if (pos.x <= 10)
            {
                float shootingFrequency = 4;

                if(shootTimer >= shootingFrequency)
                {
                    GameObject newFlame = Instantiate(enemyFlame, new Vector2(transform.localPosition.x - 1.6f, transform.localPosition.y - 1), Quaternion.Euler(new Vector3(0, -90, 0)));
                    flameAudio.Play();
                    shootTimer = 0;
                    Destroy(newFlame, 4f);
                } 
            }
        }
    }

    void MoveFlame()
    {
        GameObject flameStream = GameObject.FindGameObjectWithTag("bossFire");

        if(flameStream != null && (isFlameBoss || isSecondaryFlameBoss))
        {
            Vector2 firePos = flameStream.transform.localPosition;
            Vector2 enemyPos = this.transform.localPosition;

            firePos.x = enemyPos.x - 1.6f;
            firePos.y = enemyPos.y - 1;

            flameStream.transform.localPosition = firePos;
        }
    }

    void ShootAgain()
    {
        Invoke("ChangeDidShoot", shootPauseTime);
    }

    void ChangeDidShoot()
    {
        didShoot = false;
        shotOne = false;
        shotTwo = false;
        shotThree = false;
        shotAgain = false;
    }

    void Move()
    {
        if (!isDead)
        {
            Vector2 pos = this.transform.localPosition;


            if (isLevelOneEnemy)
            {
                if(pos.x >= 0)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if (isLevelTwoEnemy)
            {
                if (pos.x >= 2)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if (isLevelThreeEnemy)
            {
                if(pos.x >= 2)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if (isMissleBlimp)
            {
                if(pos.x >= 4)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if(isBoss)
            {
                if(pos.x >= 7)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if (isFlameBoss)
            {
                if(pos.x >= 7)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }
            else if (isSecondaryFlameBoss)
            {
                if(pos.x >= 2)
                {
                    pos.x -= moveSpeed * Time.deltaTime;
                }
            }


            if (pos.y <= (startingPosition.y + bounceAmount) && !movedUp)
            {
                pos.y += verticalMoveSpeed * Time.deltaTime;
            }

            if (pos.y >= (startingPosition.y - bounceAmount) && movedUp)
            {
                pos.y -= verticalMoveSpeed * Time.deltaTime;
            }

            if (pos.y >= (startingPosition.y + bounceAmount))
            {
                movedUp = true;
            }

            if (pos.y <= (startingPosition.y - bounceAmount))
            {
                movedUp = false;
            }

            this.transform.localPosition = pos;
            currentPosition = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If the Enemy is hit by the player's bullet
        if (other.CompareTag("playerBullet"))
        {
            enemyHealth -= 1;

            if(enemyHealth <= 0)
            {
                if (isLevelOneEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 100;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnBlimp = false;

                    GameObject newExplosion = Instantiate(blimpExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isLevelTwoEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 150;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHelicopter = false;

                    GameObject newExplosion = Instantiate(heliExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isLevelThreeEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 200;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAirplane = false;

                    GameObject newExplosion = Instantiate(heliExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isMissleBlimp)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 300;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnMissleBlimp = false;

                    GameObject newExplosion = Instantiate(missileBlimpExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isBoss)
                {
                    if(GameObject.Find("player").GetComponent<Player>().currentLevel <= 14)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 800;

                        GameObject newExplosion = Instantiate(bigBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }
                    else if(GameObject.Find("player").GetComponent<Player>().currentLevel >= 15 || GameObject.Find("player").GetComponent<Player>().currentLevel < 20)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 900;

                        GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }
                    else if(GameObject.Find("player").GetComponent<Player>().currentLevel >= 21)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 1000;

                        GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }

                    if (GameObject.Find("player").GetComponent<Player>().currentLevel <= 20)
                    {
                        Time.timeScale = 5;
                        GameObject.Find("GameManager").GetComponent<GameManager>().didChangeTimeScale = true;
                    }
                }
                else if (isFlameBoss || isSecondaryFlameBoss)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 1500;

                    GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }

                GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawnTimer = 0;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnEnemy = false;

                Destroy(other.gameObject);

                if(explosionAudio != null)
                {
                    explosionAudio.Play();
                }

                Destroy(this.gameObject);
            }
            else
            {
                GameObject.Find("player").GetComponent<Player>().playerScore += 50;

                if (!isBoss && !isFlameBoss && !isSecondaryFlameBoss)
                {
                    GameObject newSparks = Instantiate(bulletSparks, transform.localPosition, Quaternion.Euler(new Vector3(0, 90, 0)));

                    Vector2 sparksPos = newSparks.transform.localPosition;
                    Vector2 enemyPos = this.transform.localPosition;

                    sparksPos.x = enemyPos.x;
                    sparksPos.y = enemyPos.y;

                    newSparks.transform.localPosition = sparksPos;
                    Destroy(newSparks, 2f);
                }
                else if(isBoss || isFlameBoss || isSecondaryFlameBoss)
                {
                    GameObject newSparks = Instantiate(bossSparks, new Vector2(transform.localPosition.x, transform.localPosition.y - 2f), Quaternion.Euler(new Vector3(0, -90, 0)));

                    Vector2 sparksPos = newSparks.transform.localPosition;
                    Vector2 enemyPos = this.transform.localPosition;

                    sparksPos.x = enemyPos.x;
                    sparksPos.y = enemyPos.y - 2f;

                    newSparks.transform.localPosition = sparksPos;

                    Destroy(newSparks, 2f);
                }

                bulletHit.Play();

                Destroy(other.gameObject);
            }
        }
        //If the Enemy is hit by a missile
        if (other.CompareTag("missle") && other.GetComponent<EnemyMissle>().isPlayerMissle)
        {
            enemyHealth -= 5;

            if (enemyHealth <= 0)
            {
                if (isLevelOneEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 100;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnBlimp = false;

                    GameObject newExplosion = Instantiate(blimpExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isLevelTwoEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 150;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHelicopter = false;

                    GameObject newExplosion = Instantiate(heliExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isLevelThreeEnemy)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 200;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAirplane = false;

                    GameObject newExplosion = Instantiate(heliExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isMissleBlimp)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 300;
                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnMissleBlimp = false;

                    GameObject newExplosion = Instantiate(missileBlimpExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }
                else if (isBoss)
                {
                    if (GameObject.Find("player").GetComponent<Player>().currentLevel <= 14)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 800;

                        GameObject newExplosion = Instantiate(bigBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }
                    else if (GameObject.Find("player").GetComponent<Player>().currentLevel >= 15 || GameObject.Find("player").GetComponent<Player>().currentLevel < 20)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 900;

                        GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }
                    else if (GameObject.Find("player").GetComponent<Player>().currentLevel >= 21)
                    {
                        GameObject.Find("player").GetComponent<Player>().playerScore += 1000;

                        GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                        Destroy(newExplosion, 4f);
                    }

                    if(GameObject.Find("player").GetComponent<Player>().currentLevel <= 20)
                    {
                        Time.timeScale = 5;
                        GameObject.Find("GameManager").GetComponent<GameManager>().didChangeTimeScale = true;
                    }
                }
                else if (isFlameBoss || isSecondaryFlameBoss)
                {
                    GameObject.Find("player").GetComponent<Player>().playerScore += 1500;

                    GameObject newExplosion = Instantiate(mediumBossExplosion, transform.localPosition, transform.localRotation);
                    Destroy(newExplosion, 4f);
                }

                GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawnTimer = 0;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnEnemy = false;
                Destroy(other.gameObject);

                if(explosionAudio != null)
                {
                    explosionAudio.Play();
                }

                Destroy(this.gameObject);
            }
            else
            {
                GameObject.Find("player").GetComponent<Player>().playerScore += 100;

                if (!isBoss && !isFlameBoss && !isSecondaryFlameBoss)
                {
                    GameObject newBlast = Instantiate(missileHitBlast, transform.localPosition, transform.localRotation);

                    Vector2 blastPos = newBlast.transform.localPosition;
                    Vector2 enemyPos = this.transform.localPosition;

                    blastPos.x = enemyPos.x;
                    blastPos.y = enemyPos.y;

                    newBlast.transform.localPosition = blastPos;
                    Destroy(newBlast, 2f);
                }
                else if(isBoss || isFlameBoss || isSecondaryFlameBoss)
                {
                    GameObject newBlast = Instantiate(missileHitBlast, transform.localPosition, transform.localRotation);

                    Vector2 blastPos = newBlast.transform.localPosition;
                    Vector2 enemyPos = this.transform.localPosition;

                    blastPos.x = enemyPos.x;
                    blastPos.y = enemyPos.y;

                    newBlast.transform.localPosition = blastPos;
                    Destroy(newBlast, 2f);
                }

                missileHit.Play();

                Destroy(other.gameObject);
            }
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
