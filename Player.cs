using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float gravity = 10f;
    public float upForce = 8f;
    public float bulletSpeed = 400f;
    public float gameTimer;
    public float levelTime = 30f;
    public float powerUpTimer = 0;
    public float powerUpTime = 30f;

    public int currentLevel;

    public float playerScore;
    public int playerHealth;
    public int playerAmmo;
    public int playerMissleAmmo;

    public bool isPlaying = false;
    public bool didGetWeaponUpgarde = false;
    public bool didGetExtraLife = false;
    public bool canTakeAwayExtraLife = false;
    public bool didGetHighScore = false;

    public bool isStartMode = false;
    public bool isPaused = false;
    public bool shouldUpdateSubText = false;
    public bool playerIsDead = false;
    public bool isBurning = false;
    public bool didPlayExplosion = false;
    public bool isOnFire;
    public bool madeFire;

    public AudioSource buttonClick;
    public AudioSource bulletAudio;
    public AudioSource bulletHit;
    public AudioSource missileAudio;
    public AudioSource missileHit;
    public AudioSource explosionAudio;
    public AudioSource burningAudio;

    public AudioClip explosion;

    public Sprite playerNormal;
    public Sprite playerFlying;

    public Text playerMissleText;
    public Text HighScoreText;

    public Text pauseScoreText;
    public Text pauseLevelText;

    public Canvas HighScoreCanvas;
    public Canvas pauseCanvas;
    public Canvas flyingTextCanvas;
    public Canvas canvasHUD;
    public Canvas flyingSubTextCanvas;

    public GameObject deathFire;
    public GameObject burntFire;
    public GameObject bulletSparks;
    public GameObject missileHitBlast;

    public InputField playerName;

    public List<int> burnTickTimers = new List<int>();

    //Cursor Icon Variables:
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public enum PlayerMode
    {
        Start,
        Fly,
        Falling,
        Dead
    }

    public PlayerMode playerMode;

    Vector2 startPosition;

    public GameObject bullet;
    public GameObject missle;
    public GameObject extraLifeIcon;

    GameObject player;

    private float highScore1;
    private float highScore2;
    private float highScore3;
    private float highScore4;
    private float highScore5;

    private string playerName1;
    private string playerName2;
    private string playerName3;
    private string playerName4;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;

        playerMode = PlayerMode.Start;

        startPosition = new Vector2 (-9, 0);

        gameTimer = 0;

        hotSpot = new Vector2(cursorTexture.height / 2, cursorTexture.width / 2);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        SwitchMode();

        CheckScore();

        CheckCollision();

        UpdateLevel();

        IsOnFire();

        if (didGetWeaponUpgarde)
        {
            powerUpTimer += 1 * Time.deltaTime;
        }
        else
        {
            powerUpTimer = 0;
        }

        highScore1 = PlayerPrefs.GetFloat("highscore1");
        highScore2 = PlayerPrefs.GetFloat("highscore2");
        highScore3 = PlayerPrefs.GetFloat("highscore3");
        highScore4 = PlayerPrefs.GetFloat("highscore4");
        highScore5 = PlayerPrefs.GetFloat("highscore5");

        playerName1 = PlayerPrefs.GetString("playername1");
        playerName2 = PlayerPrefs.GetString("playername2");
        playerName3 = PlayerPrefs.GetString("playername3");
        playerName4 = PlayerPrefs.GetString("playername4");

        pauseScoreText.text = "~ Current Score: " + playerScore + " ~";
        pauseLevelText.text = "~ Current Level: " + currentLevel + " ~";
    }

    void SwitchMode()
    {
        Vector2 pos = new Vector2(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y);

        switch (playerMode)
        {
            case PlayerMode.Start:

                if (!didGetExtraLife)
                {
                    //This is used to show Game Title
                    isStartMode = true;
                    shouldUpdateSubText = false;

                    didGetHighScore = false;

                    currentLevel = 1;

                    playerScore = 0;
                    playerHealth = 5;
                    playerAmmo = 5;
                    playerMissleAmmo = 0;
                    playerMissleText.enabled = false;

                    GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnExtraLife = false;
                }
                else
                {
                    canTakeAwayExtraLife = true;
                    shouldUpdateSubText = true;
                    
                    if(playerHealth < 5)
                    {
                        playerHealth = 5;
                    }

                    if(playerAmmo < 5)
                    {
                        playerAmmo = 5;
                    }
                }

                canvasHUD.GetComponent<Canvas>().enabled = true;
                isBurning = false;
                playerIsDead = false;
                player.GetComponent<SpriteRenderer>().sprite = playerNormal;
                CancelInvoke();

                gameTimer = 0;

                didGetWeaponUpgarde = false;
                powerUpTimer = 0;

                pos = startPosition;
                player.transform.localPosition = pos;

                GameObject.Find("GameManager").GetComponent<GameManager>().didMoveText = false;

                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnBoss = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnEnemy = false;

                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnBlimp = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHelicopter = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAirplane = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnMissleBlimp = false;

                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnAmmoPowerUp = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnHealthPowerUp = false;

                GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawnTimer = 0;

                isPlaying = false;

                break;

            case PlayerMode.Fly:

                if (canTakeAwayExtraLife)
                {
                    didGetExtraLife = false;
                    canTakeAwayExtraLife = false;
                    shouldUpdateSubText = false;
                }

                if(Time.timeScale == 1)
                {
                    ApplyGravity();
                    pos.y += upForce * Time.deltaTime;
                    player.transform.localPosition = pos;
                }
                else
                {
                    if(currentLevel < 15)
                    {
                        if (pos.y <= 0)
                        {
                            pos.y += (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                        else if (pos.y >= 0)
                        {
                            pos.y -= (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                    }
                    else
                    {
                        if (pos.y <= 1)
                        {
                            pos.y += (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                        else if (pos.y >= 1)
                        {
                            pos.y -= (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                    }

                }

                GameObject burningFire = GameObject.FindGameObjectWithTag("burnFire");

                if (burningFire != null)
                {
                    Vector2 firePos = burningFire.transform.localPosition;
                    Vector2 playerPos = this.transform.localPosition;

                    firePos.x = playerPos.x;
                    firePos.y = playerPos.y + 0.5f;

                    burningFire.transform.localPosition = firePos;
                }

                playerIsDead = false;
                isStartMode = false;
                player.GetComponent<SpriteRenderer>().sprite = playerFlying;
                isPlaying = true;
                break;

            case PlayerMode.Falling:

                if(Time.timeScale == 1)
                {
                    ApplyGravity();
                }
                else
                {
                    if (currentLevel < 15)
                    {
                        if (pos.y <= 0)
                        {
                            pos.y += (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                        else if (pos.y >= 0)
                        {
                            pos.y -= (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                    }
                    else
                    {
                        if (pos.y <= 1)
                        {
                            pos.y += (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                        else if (pos.y >= 1)
                        {
                            pos.y -= (upForce / 10) * Time.deltaTime;
                            player.transform.localPosition = pos;
                        }
                    }

                }

                GameObject burnFire = GameObject.FindGameObjectWithTag("burnFire");

                if (burnFire != null)
                {
                    Vector2 firePos = burnFire.transform.localPosition;
                    Vector2 playerPos = this.transform.localPosition;

                    firePos.x = playerPos.x;
                    firePos.y = playerPos.y + 0.5f;

                    burnFire.transform.localPosition = firePos;
                }

                playerIsDead = false;
                isStartMode = false;
                player.GetComponent<SpriteRenderer>().sprite = playerNormal;
                isPlaying = true;
                break;

            case PlayerMode.Dead:
                //Checking if player has an extra life and if they beat the highscore
                if(playerScore > PlayerPrefs.GetFloat("highscore5") && !didGetExtraLife && !didGetHighScore)
                {
                    //Set Score to show when High Score Panel shows
                    HighScoreText.text = playerScore.ToString();

                    //Set High Score Panel to show
                    HighScoreCanvas.GetComponent<Canvas>().enabled = true;

                    //Turn off Player Sprite
                    player.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    HighScoreCanvas.GetComponent<Canvas>().enabled = false;
                    player.GetComponent<SpriteRenderer>().enabled = true;
                    BurnBalloon();

                    Invoke("ApplyGravity", 1f);
                }

                GameObject newFire = GameObject.FindGameObjectWithTag("fire");

                if(newFire != null)
                {
                    Vector2 firePos = newFire.transform.localPosition;
                    Vector2 playerPos = this.transform.localPosition;

                    firePos.x = playerPos.x;
                    firePos.y = playerPos.y + 0.5f;

                    newFire.transform.localPosition = firePos;
                }

                Time.timeScale = 1;
                canvasHUD.GetComponent<Canvas>().enabled = false;
                playerIsDead = true;
                isStartMode=false;
                player.GetComponent<SpriteRenderer>().sprite = playerNormal;
                isPlaying = false;
                player.transform.localPosition = pos;
                break;
        }
    }

    void BurnBalloon()
    {
        if (!isBurning)
        {
            GameObject newFire = Instantiate(deathFire, new Vector2(transform.localPosition.x, transform.localPosition.y + 0.5f), transform.localRotation);
            Destroy(newFire, 4f);
            
            isBurning = true;
        }
    }

    public void ContinueBTN()
    {
        //If player got high score, this function sets score ranking
        string newInput = playerName.text;

        if (playerScore > highScore1)
        {
            PlayerPrefs.SetFloat("highscore5", highScore4);
            PlayerPrefs.SetFloat("highscore4", highScore3);
            PlayerPrefs.SetFloat("highscore3", highScore2);
            PlayerPrefs.SetFloat("highscore2", highScore1);
            PlayerPrefs.SetFloat("highscore1", playerScore);

            PlayerPrefs.SetString("playername5", playerName4);
            PlayerPrefs.SetString("playername4", playerName3);
            PlayerPrefs.SetString("playername3", playerName2);
            PlayerPrefs.SetString("playername2", playerName1);
            PlayerPrefs.SetString("playername1", newInput);
        }
        else if (playerScore > highScore2)
        {
            PlayerPrefs.SetFloat("highscore5", highScore4);
            PlayerPrefs.SetFloat("highscore4", highScore3);
            PlayerPrefs.SetFloat("highscore3", highScore2);
            PlayerPrefs.SetFloat("highscore2", playerScore);

            PlayerPrefs.SetString("playername5", playerName4);
            PlayerPrefs.SetString("playername4", playerName3);
            PlayerPrefs.SetString("playername3", playerName2);
            PlayerPrefs.SetString("playername2", newInput);
        }
        else if (playerScore > highScore3)
        {
            PlayerPrefs.SetFloat("highscore5", highScore4);
            PlayerPrefs.SetFloat("highscore4", highScore3);
            PlayerPrefs.SetFloat("highscore3", playerScore);

            PlayerPrefs.SetString("playername5", playerName4);
            PlayerPrefs.SetString("playername4", playerName3);
            PlayerPrefs.SetString("playername3", newInput);
        }
        else if (playerScore > highScore4)
        {
            PlayerPrefs.SetFloat("highscore5", highScore4);
            PlayerPrefs.SetFloat("highscore4", playerScore);

            PlayerPrefs.SetString("playername5", playerName4);
            PlayerPrefs.SetString("playername4", newInput);
        }
        else if (playerScore > highScore5)
        {
            PlayerPrefs.SetFloat("highscore5", playerScore);
            PlayerPrefs.SetString("playername5", newInput);
        }

        buttonClick.Play();
        didGetHighScore = true;
    }

    void ApplyGravity()
    {
        if (playerMode != PlayerMode.Start)
        {
            Vector2 pos = new Vector2(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y);

            pos.y -= gravity * Time.deltaTime;
            player.transform.localPosition = pos;
        }
    }

    void SpawnBullet()
    {
        if (!didGetWeaponUpgarde)
        {
            if (playerAmmo > 0)
            {
                GameObject newBullet = Instantiate(bullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletSpeed);
                playerAmmo--;
                bulletAudio.Play();
                Destroy(newBullet, 3f);
            }
        }
        else
        {
            if(powerUpTimer <= powerUpTime)
            {
                if (playerAmmo > 0)
                {
                    GameObject newBulletOne = Instantiate(bullet, transform.localPosition, transform.localRotation);
                    GameObject newBulletTwo = Instantiate(bullet, new Vector2(transform.localPosition.x, transform.localPosition.y - .5f), transform.localRotation);
                    newBulletOne.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletSpeed);
                    newBulletTwo.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletSpeed);
                    playerAmmo--;
                    bulletAudio.Play();
                    Destroy(newBulletOne, 3f);
                    Destroy(newBulletTwo, 3f);
                }
            }
            else
            {
                powerUpTimer = 0;
                didGetWeaponUpgarde = false;
            }
        }
    }

    void SpawnMissle()
    {
        Vector3 mousePos = Input.mousePosition;

        if(mousePos.x >= 900)
        {
            if(playerMissleAmmo > 0)
            {
                GameObject newMissle = Instantiate(missle, transform.localPosition, transform.localRotation);
                newMissle.GetComponent<EnemyMissle>().isPlayerMissle = true;
                missileAudio.Play();
                playerMissleAmmo--;
                playerMissleText.text = ("Missles: " + playerMissleAmmo.ToString());
            }
        }
    }
    
    void CheckInput()
    {
        if(playerMode != PlayerMode.Dead)
        {
            if (Input.GetMouseButtonDown(0) && isPlaying && !isPaused)
            {
                //-Left Button Was Clicked
                SpawnBullet();
            }

            if (Input.GetMouseButtonDown(1) && isPlaying && !isPaused)
            {
                //-Right Button was Clicked
                SpawnMissle();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerMode = PlayerMode.Fly;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                playerMode = PlayerMode.Falling;
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            // -Resets
            playerMode = PlayerMode.Dead;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        */

        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && isPlaying)
        {
            buttonClick.Play();
            Pause();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            pauseCanvas.GetComponent<Canvas>().enabled = true;
            extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
            canvasHUD.GetComponent<Canvas>().enabled = false;
            flyingTextCanvas.GetComponent<Canvas>().enabled = false;
            flyingSubTextCanvas.GetComponent<Canvas>().enabled = false;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseCanvas.GetComponent<Canvas>().enabled = false;
            extraLifeIcon.GetComponent<SpriteRenderer>().enabled = true;
            canvasHUD.GetComponent<Canvas>().enabled = true;
            flyingTextCanvas.GetComponent<Canvas>().enabled = true;
            flyingSubTextCanvas.GetComponent<Canvas>().enabled = true;
            isPaused = false;
        }
    }

    void CheckScore()
    {
        GameObject[] Gates = (GameObject.FindGameObjectsWithTag("gate"));

        foreach(GameObject gate in Gates)
        {
            if(gate.transform.localPosition.x < -8 && !gate.GetComponent<Gate>().didScore)
            {
                playerScore += .5f;
                gate.GetComponent<Gate>().didScore = true;
            }
        }
        //Debug.Log("Player Score: " + playerScore);
    }

    void CheckCollision()
    {
        GameObject[] Gates = (GameObject.FindGameObjectsWithTag("gate"));
        BoxCollider2D gateCollider;
        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();

        foreach (GameObject gate in Gates)
        {
            gateCollider = gate.GetComponentInChildren<BoxCollider2D>();

            if (gateCollider.bounds.Intersects(playerCollider.bounds))
            {
                if (!didPlayExplosion)
                {
                    explosionAudio.PlayOneShot(explosion);
                    didPlayExplosion = true;
                }

                playerMode = PlayerMode.Dead;
                extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        if((playerMode == PlayerMode.Fly || playerMode == PlayerMode.Falling) && (player.transform.localPosition.y < -6 || player.transform.localPosition.y > 6.5))
        {
            explosionAudio.PlayOneShot(explosion);  
            playerMode = PlayerMode.Dead;
            extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
        }

        if(playerMode == PlayerMode.Dead && player.transform.localPosition.y < -15)
        {
            player.transform.localPosition = startPosition;
            didPlayExplosion = false;
            playerMode = PlayerMode.Start;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlaying)
        {
            if (other.CompareTag("enemyBullet"))
            {
                if(Time.timeScale == 1)
                {
                    Destroy(other.gameObject);
                    playerHealth--;

                    if (playerHealth <= 0)
                    {
                        explosionAudio.Play();
                        playerMode = PlayerMode.Dead;
                        extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        bulletHit.Play();
                        GameObject newSparks = Instantiate(bulletSparks, new Vector2(transform.localPosition.x - 1f, transform.localPosition.y - 0.5f), transform.localRotation);

                        Vector2 sparksPos = newSparks.transform.localPosition;
                        Vector2 playerPos = this.transform.localPosition;

                        sparksPos.x = playerPos.x - 1f;
                        sparksPos.y = playerPos.y - 0.5f;

                        newSparks.transform.localPosition = sparksPos;
                        Destroy(newSparks, 2f);
                    }
                }
            }

            if (other.CompareTag("missle"))
            {
                if (!other.GetComponent<EnemyMissle>().isPlayerMissle)
                {
                    if (Time.timeScale == 1)
                    {
                        Destroy(other.gameObject);
                        playerHealth -= 5;

                        if (playerHealth <= 0)
                        {
                            explosionAudio.Play();
                            playerMode = PlayerMode.Dead;
                            extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        else
                        {
                            missileHit.Play();
                            GameObject newMissileHit = Instantiate(missileHitBlast, transform.localPosition, transform.localRotation);

                            Vector2 blastPos = newMissileHit.transform.localPosition;
                            Vector2 playerPos = this.transform.localPosition;

                            blastPos.x = playerPos.x;
                            blastPos.y = playerPos.y;

                            newMissileHit.transform.localPosition = blastPos;
                            Destroy(newMissileHit, 2f);
                        }
                    }
                }
            }

            if (other.CompareTag("powerUp"))
            {
                GameObject powerUp = other.gameObject;

                //AMMO POWER UP:
                if (powerUp.GetComponent<PowerUp>().isAmmo)
                {
                    if(currentLevel <= 5)
                    {
                        playerAmmo += Random.Range(7, 10);
                    }
                    else if(currentLevel >= 6 && currentLevel <= 10)
                    {
                        playerAmmo += Random.Range(8, 12);
                    }
                    else if(currentLevel >= 11 && currentLevel <= 15)
                    {
                        playerAmmo += Random.Range(10, 15);
                    }
                    else if(currentLevel >= 16 && currentLevel <= 30)
                    {
                        playerAmmo += Random.Range(15, 20);
                    }
                    else
                    {
                        playerAmmo += Random.Range(25, 30);
                    }

                }
                //HEALTH POWER UP:
                if (powerUp.GetComponent<PowerUp>().isHealth)
                {
                    if(currentLevel <= 5)
                    {
                        playerHealth += Random.Range(5, 8);
                    }
                    else if(currentLevel >= 6 && currentLevel <= 10)
                    {
                        playerHealth += Random.Range(5, 10);
                    }
                    else if(currentLevel >= 11 && currentLevel <= 15)
                    {
                        playerHealth += Random.Range(10, 15);
                    }
                    else if(currentLevel >= 16 && currentLevel <= 30)
                    {
                        playerHealth += Random.Range(15, 20);
                    }
                    else
                    {
                        playerHealth += Random.Range(25, 30);
                    }

                }
                //DOUBLE SHOT WEAPON UPGRADE:
                if (powerUp.GetComponent<PowerUp>().isWeapon)
                {
                    playerAmmo += 5;

                    if (!didGetWeaponUpgarde)
                    {
                        didGetWeaponUpgarde = true;
                    }
                    else
                    {
                        powerUpTimer = 0;
                    }

                }
                //EXTRA LIFE POWER UP:
                if (powerUp.GetComponent<PowerUp>().isExtraLife)
                {
                    //Turn on icon
                    extraLifeIcon.GetComponent<SpriteRenderer>().enabled = true;

                    //If the player already has an extra life, they are awarded 1,000 points
                    if (!didGetExtraLife)
                    {
                        didGetExtraLife = true;
                    }
                    else
                    {
                        playerHealth += currentLevel;
                    }
                }
                //MISSLE WEAPON UPGRADE:
                if (powerUp.GetComponent<PowerUp>().isMissle)
                {
                    if(currentLevel <= 30)
                    {
                        playerMissleAmmo += 3;

                        if (playerMissleAmmo > 3)
                        {
                            playerMissleAmmo = 3;
                            playerScore += 50;
                        }
                    }
                    else
                    {
                        playerMissleAmmo += 5;

                        if (playerMissleAmmo > 5)
                        {
                            playerMissleAmmo = 5;
                            playerScore += 75;
                        }
                    }

                    playerMissleText.enabled = true;
                    playerMissleText.text = ("Missiles: " + playerMissleAmmo.ToString());
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        ApplyBurn(3);
    }

    public void ApplyBurn(int ticks)
    {
        if(burnTickTimers.Count <= 0)
        {
            burnTickTimers.Add(ticks);
            StartCoroutine(Burn());
        }
        else
        {
            burnTickTimers.Add(ticks);
        }
    }

    IEnumerator Burn()
    {
        while(burnTickTimers.Count > 0)
        {
            isOnFire = true;

            for(int i = 0; i < burnTickTimers.Count; i++)
            {
                burnTickTimers[i]--;
            }
            playerHealth -= 2;

            //removing any tickTimers where the 'number' we're looking for is equal to a 'number' that is zero
            burnTickTimers.RemoveAll(number => number == 0);

            if (playerHealth <= 0)
            {
                if (!didPlayExplosion)
                {
                    explosionAudio.PlayOneShot(explosion);
                    didPlayExplosion = true;
                }
                playerMode = PlayerMode.Dead;
                isOnFire = false;
                extraLifeIcon.GetComponent<SpriteRenderer>().enabled = false;
            }

            yield return new WaitForSeconds(0.75f);
        }

        isOnFire = false;
    }

    void IsOnFire()
    {
        if (isOnFire && !madeFire)
        {
            GameObject newFire = Instantiate(burntFire, new Vector2(transform.localPosition.x, transform.localPosition.y + 0.11f), transform.localRotation);
            madeFire = true;
            burningAudio.Play();
        }
        else if(!isOnFire)
        {
            GameObject newFire = GameObject.FindGameObjectWithTag("burnFire");
            Destroy(newFire);
            madeFire = false;
            burningAudio.Stop();
        }
    }

    void UpdateLevel()
    {
        if (isPlaying)
        {
            gameTimer += 1 * Time.deltaTime;
        }

        if (gameTimer >= levelTime)
        {
            currentLevel++;

            //THINGS THAT SHOULD HAPPEN ONCE AT THE START OF THE LEVEL DURING GAMEPLAY GO HERE://

            GameObject.Find("GameManager").GetComponent<GameManager>().didMoveText = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnExtraLife = false;
            gameTimer = 0;
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
