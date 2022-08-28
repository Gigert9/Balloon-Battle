using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gateSize = 23;

    public float gateSpawnTimer = 0;
    public float gateSpawnSpeed = .1f;
    public float sameGatesInRow = 0.04f;
    public float gatesSpawnedCounter = 0;

    public float powerUpSpawnTimer = 0;
    public float powerUpSpawnTime = 10f;
    public float spawnUpgradeChance;

    public float enemySpawnTimer = 0;
    public float enemySpawnTime = 5;
    public float extraEnemySpawnTimer = 0;
    public float extraEnemySpawnTime = 10;

    public float topHieght = 10;

    public float titleXPos;

    public AudioSource buttonClick;

    public Text playerScore;
    public Text playerHealth;
    public Text playerAmmo;

    public GameObject player;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject missleBlimp;
    public GameObject boss;
    public GameObject missleBoss;
    public GameObject flameBoss;

    public GameObject ammoPowerUp;
    public GameObject healthPowerUp;
    public GameObject weaponPowerUp;
    public GameObject misslePowerUp;
    public GameObject extraLife;

    public GameObject levelText;
    public GameObject titleText;
    public GameObject subText;
    public GameObject shootText;
    public GameObject backButton;
    public GameObject missleText;

    public bool didMoveText = false;
    public bool didChangeTimeScale = false;

    public bool didSpawnGate = false;
    public bool shouldGetRandom = true;

    public bool didSpawnEnemy = false;
    public bool didSpawnBoss = false;
    public bool didKillBoss = false;

    public bool didSpawnBlimp = false;
    public bool didSpawnAirplane = false;
    public bool didSpawnHelicopter = false;
    public bool didSpawnMissleBlimp = false;

    public bool didSpawnAmmoPowerUp = false;
    public bool didSpawnHealthPowerUp = false;
    public bool didSpawnWeaponPowerUp = false;
    public bool didSpawnExtraLife = false;
    public bool didSpawnMisslePowerUp = false;

    private bool spawnedTopGate = false;
    private bool spawnedBottomGate = false;

    private Vector2 enemyStartingPosition;

    private Vector2 ammoPowerUpStartingPosition;
    private Vector2 healthPowerUpStartingPosition;
    private Vector2 weaponUpgradeStartingPosition;
    private Vector2 extraLifeStartingPosition;
    private Vector2 missleUpgradeStartingPosition;

    //Cursor Icon Variables:
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        hotSpot = new Vector2(cursorTexture.height/2, cursorTexture.width/2);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnGate();
        CheckSpawnGateTime();
        CheckLevel();
        RandomizeGatesInRow();
        UpdateUI();
        SpawnEnemy();
        SpawnPowerUp();
        MoveLevelText();
        MoveTitleText();
        MoveSubText();
        MoveShootText();
        MoveMissleText();
        HideBackButton();
    }

    void SpawnPowerUp()
    {
        powerUpSpawnTimer += 1 * Time.deltaTime;

        spawnUpgradeChance = Random.Range(1, 100);
        
        if(powerUpSpawnTimer >= powerUpSpawnTime && Time.timeScale == 1)
        {
            //AMMO UPGRADE:
            if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnAmmoPowerUp)
            {
                if (player.GetComponent<Player>().currentLevel <= 14)
                {
                    ammoPowerUpStartingPosition.x = Random.Range(14f, 20f);
                    ammoPowerUpStartingPosition.y = Random.Range(-3, 3);
                    GameObject powerUp = Instantiate(ammoPowerUp, ammoPowerUpStartingPosition, transform.localRotation);
                    didSpawnAmmoPowerUp = true;
                    powerUp.GetComponent<PowerUp>().isAmmo = true;
                    powerUpSpawnTimer = 0;
                }
                else
                {
                    ammoPowerUpStartingPosition.x = Random.Range(14f, 20f);
                    ammoPowerUpStartingPosition.y = Random.Range(-0.5f, 4.5f);
                    GameObject powerUp = Instantiate(ammoPowerUp, ammoPowerUpStartingPosition, transform.localRotation);
                    didSpawnAmmoPowerUp = true;
                    powerUp.GetComponent<PowerUp>().isAmmo = true;
                    powerUpSpawnTimer = 0;
                }
            }
            //HEALTH UPGRADE:
            if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnHealthPowerUp)
            {
                if (GameObject.Find("player").GetComponent<Player>().currentLevel == 1)
                {
                    if (spawnUpgradeChance <= 100f)
                    {
                        healthPowerUpStartingPosition.x = Random.Range(20f, 30f);
                        healthPowerUpStartingPosition.y = Random.Range(-2, 3);
                        GameObject powerUp = Instantiate(healthPowerUp, healthPowerUpStartingPosition, transform.localRotation);
                        didSpawnHealthPowerUp = true;
                        powerUp.GetComponent<PowerUp>().isHealth = true;
                    }
                }
                else if(player.GetComponent<Player>().currentLevel <= 15)
                {
                    if (spawnUpgradeChance <= 25f)
                    {
                        healthPowerUpStartingPosition.x = Random.Range(20f, 30f);
                        healthPowerUpStartingPosition.y = Random.Range(-2, 3);
                        GameObject powerUp = Instantiate(healthPowerUp, healthPowerUpStartingPosition, transform.localRotation);
                        didSpawnHealthPowerUp = true;
                        powerUp.GetComponent<PowerUp>().isHealth = true;
                    }
                }
                else
                {
                    if (spawnUpgradeChance <= 25f)
                    {
                        healthPowerUpStartingPosition.x = Random.Range(20f, 30f);
                        healthPowerUpStartingPosition.y = Random.Range(-0.5f, 4.5f);
                        GameObject powerUp = Instantiate(healthPowerUp, healthPowerUpStartingPosition, transform.localRotation);
                        didSpawnHealthPowerUp = true;
                        powerUp.GetComponent<PowerUp>().isHealth = true;
                    }
                }
            }
            //DOUBLE SHOT WEAPON POWER UP:
            if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnWeaponPowerUp)
            {
                if (player.GetComponent<Player>().currentLevel >= 6 && spawnUpgradeChance >= 26 && spawnUpgradeChance <= 50)
                {
                    if(player.GetComponent<Player>().currentLevel <= 15)
                    {
                        weaponUpgradeStartingPosition.x = Random.Range(25f, 35f);
                        weaponUpgradeStartingPosition.y = Random.Range(-2, 4);
                        GameObject powerUp = Instantiate(weaponPowerUp, weaponUpgradeStartingPosition, transform.localRotation);
                        didSpawnAmmoPowerUp = true;
                        powerUp.GetComponent<PowerUp>().isWeapon = true;
                    }
                    else
                    {
                        weaponUpgradeStartingPosition.x = Random.Range(25f, 35f);
                        weaponUpgradeStartingPosition.y = Random.Range(-0.5f, 4.5f);
                        GameObject powerUp = Instantiate(weaponPowerUp, weaponUpgradeStartingPosition, transform.localRotation);
                        didSpawnAmmoPowerUp = true;
                        powerUp.GetComponent<PowerUp>().isWeapon = true;
                    }
                }
            }
            //MISSLE POWER UP:
            if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnMisslePowerUp)
            {
                if(player.GetComponent<Player>().currentLevel >= 11 && spawnUpgradeChance >= 51 && spawnUpgradeChance <= 75)
                {
                    if(player.GetComponent<Player>().currentLevel <= 15)
                    {
                        missleUpgradeStartingPosition.x = Random.Range(30f, 40f);
                        missleUpgradeStartingPosition.y = Random.Range(-1, 3);
                        GameObject powerUp = Instantiate(misslePowerUp, missleUpgradeStartingPosition, transform.localRotation);
                        didSpawnMisslePowerUp = true;
                        powerUp.GetComponent<PowerUp>().isMissle = true;
                    }
                    else
                    {
                        missleUpgradeStartingPosition.x = Random.Range(30f, 40f);
                        missleUpgradeStartingPosition.y = Random.Range(-0.5f, 4.5f);
                        GameObject powerUp = Instantiate(misslePowerUp, missleUpgradeStartingPosition, transform.localRotation);
                        didSpawnMisslePowerUp = true;
                        powerUp.GetComponent<PowerUp>().isMissle = true;
                    }
                }
            }
        }

        //Is it a boss level? If so we spawn an extra life power up:
        float remainder = player.GetComponent<Player>().currentLevel % 5;

        if(remainder == 0)
        {
            if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnExtraLife)
            {
                extraLifeStartingPosition.x = 14f;
                extraLifeStartingPosition.y = Random.Range(-0.5f, 1.5f);
                GameObject powerUp = Instantiate(extraLife, extraLifeStartingPosition, transform.localRotation);
                didSpawnExtraLife = true;
                powerUp.GetComponent<PowerUp>().isExtraLife = true;
            }
            powerUpSpawnTime = 5;
        }
        else
        {
            didSpawnExtraLife = false;
            powerUpSpawnTime = 10;
        }
    }

    void SpawnEnemy()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnEnemy)
        {
            enemySpawnTimer += 1 * Time.deltaTime;

            if(player.GetComponent<Player>().currentLevel == 1)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-4, 3);
                    enemyStartingPosition.x = 14f;
                    GameObject newEnemy = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 2)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-4, 3);
                    enemyStartingPosition.x = 14f;
                    GameObject newEnemy = Instantiate(enemy2, enemyStartingPosition, transform.localRotation);
                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 3)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-4, 3);
                    enemyStartingPosition.x = 14f;
                    GameObject newEnemy = Instantiate(enemy3, enemyStartingPosition, transform.localRotation);
                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 4)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-2, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject newEnemy1 = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                    GameObject newEnemy2 = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 1.5f), transform.localRotation);

                    int enemyThreeChance = Random.Range(1, 100);

                    if (enemyThreeChance <= 15)
                    {
                        GameObject newEnemy3 = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y - 1.5f), transform.localRotation);
                    }

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 5 && !didSpawnBoss)
            {
                enemyStartingPosition.y = Random.Range(-3, 1);
                enemyStartingPosition.x = 17f;
                GameObject newBoss = Instantiate(boss, enemyStartingPosition, transform.localRotation);
                didSpawnBoss = true;
                didKillBoss = false;
            }
            else if(player.GetComponent<Player>().currentLevel == 6)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                Invoke("KillBoss" , 0.001f);

                if (didKillBoss)
                {
                    didSpawnBoss = false;

                    if (enemySpawnTimer >= enemySpawnTime)
                    {
                        enemyStartingPosition.y = Random.Range(-2, 2);
                        enemyStartingPosition.x = 14f;
                        GameObject enemyOne = Instantiate(enemy, enemyStartingPosition, transform.localRotation);

                        int secondEnemyChance = Random.Range(1, 100);

                        if (secondEnemyChance <= 15)
                        {
                            GameObject enemyTwo = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 6, enemyStartingPosition.y - 2), transform.localRotation);
                        }

                        didSpawnEnemy = true;
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 7)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-2, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject enemyOne = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 9, enemyStartingPosition.y - 1.5f), transform.localRotation);

                    int secondEnemyChance = Random.Range(1, 100);

                    if (secondEnemyChance <= 15)
                    {
                        GameObject enemyTwo = Instantiate(enemy3, enemyStartingPosition, transform.localRotation);
                    }

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 8)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-2, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject enemyOne = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y - 2f), transform.localRotation);

                    int secondEnemyChance = Random.Range(1, 100);

                    if (secondEnemyChance <= 25)
                    {
                        GameObject enemyTwo = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                    }

                    int thirdEnemyChance = Random.Range(1, 100);

                    if(thirdEnemyChance <= 15)
                    {
                        GameObject enemyThree = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 2f), transform.localRotation);
                    }

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 9)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-2, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject emeyOne = Instantiate(enemy2, enemyStartingPosition, transform.localRotation);
                    GameObject emeyTwo = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 4, enemyStartingPosition.y - 2f), transform.localRotation);
                    GameObject enemyThree = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y + 2f), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 10 && !didSpawnBoss)
            {
                enemyStartingPosition.y = Random.Range(-1, 1);
                enemyStartingPosition.x = 17f;
                GameObject newBoss = Instantiate(boss, enemyStartingPosition, transform.localRotation);
                newBoss.GetComponent<Enemy>().enemyHealth = 50;
                didSpawnBoss = true;
                didKillBoss = false;
            }
            else if(player.GetComponent<Player>().currentLevel == 11)
            {
                if (didChangeTimeScale)
                {

                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                Invoke("KillBoss", 0.001f);

                if (didKillBoss)
                {
                    didSpawnBoss = false;

                    if (enemySpawnTimer >= enemySpawnTime)
                    {
                        //Spawn Missle Blimps
                        enemyStartingPosition.y = Random.Range(0, 2);
                        enemyStartingPosition.x = 14f;
                        GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);
                        didSpawnEnemy = true;
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 12)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    //Spawn Missle Blimps
                    enemyStartingPosition.y = Random.Range(-1, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);
                    didSpawnEnemy = true;

                    //Spawn Helicopter
                    GameObject enemyOne = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y - 1), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 13)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    //Spawn Missle Blimps
                    enemyStartingPosition.y = Random.Range(-1, 1);
                    enemyStartingPosition.x = 14f;
                    GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);
                    didSpawnEnemy = true;

                    //Spawn three blimps
                    GameObject enemyOne = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 4, enemyStartingPosition.y - 1), transform.localRotation);
                    GameObject enemyTwo = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y + 1), transform.localRotation);
                    GameObject newEnemyThree = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 2, enemyStartingPosition.y + 2), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 14)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    //Spawn Missle Blimps
                    enemyStartingPosition.y = Random.Range(0, 2);
                    enemyStartingPosition.x = 14f;
                    GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);

                    //Spawn Airplane
                    GameObject enemyOne = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y - 2), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 15 && !didSpawnBoss)
            {
                enemyStartingPosition.y = Random.Range(-1, 2);
                enemyStartingPosition.x = 15f;
                GameObject newBoss = Instantiate(missleBoss, enemyStartingPosition, transform.localRotation);
                newBoss.GetComponent<Enemy>().enemyHealth = 75;
                didSpawnBoss = true;
                didKillBoss = false;
            }
            else if(player.GetComponent<Player>().currentLevel == 16)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                Invoke("KillBoss", 0.001f);

                if (didKillBoss)
                {
                    didSpawnBoss = false;

                    if(enemySpawnTimer >= enemySpawnTime)
                    {
                        //Spawn more enemies...
                        enemyStartingPosition.y = Random.Range(0, 2);
                        enemyStartingPosition.x = 14f;
                        GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);
                        GameObject secondMissleBlimp = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + Random.Range(5, 10), enemyStartingPosition.y - 2), transform.localRotation);

                        didSpawnEnemy = true;
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 17)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-1, 2);
                    enemyStartingPosition.x = 14f;

                    GameObject enemyOne = Instantiate(enemy3, enemyStartingPosition, transform.localRotation);
                    GameObject enemyTwo = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 6, enemyStartingPosition.y -1), transform.localRotation);
                    GameObject enemyThree = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 12, enemyStartingPosition.y + 1), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 18)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = Random.Range(-1, 1);
                    enemyStartingPosition.x = 14f;

                    GameObject enemyOne = Instantiate(enemy2, enemyStartingPosition, transform.localRotation);
                    GameObject enemyTwo = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 2, enemyStartingPosition.y - 0.5f), transform.localRotation);
                    GameObject enemyThree = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 4, enemyStartingPosition.y + 1), transform.localRotation);
                    GameObject enemyFour = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 6, enemyStartingPosition.y), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 19)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0f;
                    enemyStartingPosition.x = 14f;

                    GameObject newMissleBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);
                    GameObject newBlimpTwo = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 3, enemyStartingPosition.y + 2), transform.localRotation);
                    GameObject newBlimpThree = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y - 2), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 20 && !didSpawnBoss)
            {
                //Spawn another Boss...
                enemyStartingPosition.y = Random.Range(1, 1.5f);
                enemyStartingPosition.x = 15f;
                GameObject newBoss = Instantiate(missleBoss, enemyStartingPosition, transform.localRotation);
                newBoss.GetComponent<Enemy>().enemyHealth = 100;
                didSpawnBoss = true;
                didKillBoss = false;
            }
            else if(player.GetComponent<Player>().currentLevel >= 21 && player.GetComponent<Player>().currentLevel <= 30)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                float remainder = player.GetComponent<Player>().currentLevel % 5;

                //Spawning Bosses every 5 levels:
                if(remainder == 0 && !didSpawnBoss)
                {
                    Vector2 tempBossPos;

                    tempBossPos.y = 1f;
                    tempBossPos.x = 15f;
                    GameObject newBoss = Instantiate(missleBoss, new Vector2(tempBossPos.x, tempBossPos.y), transform.localRotation);
                    newBoss.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                    newBoss.GetComponent<BoxCollider2D>().isTrigger = true;
                    didSpawnBoss = true;
                }
                else if(remainder == 0)
                {
                    if(enemySpawnTimer >= enemySpawnTime)
                    {
                        enemyStartingPosition.x = 14;
                        enemyStartingPosition.y = 0;

                        if (!didSpawnBlimp)
                        {
                            GameObject extraEnemy = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                            GameObject extraEnemy2 = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 3, enemyStartingPosition.y + 1), transform.localRotation);
                            GameObject extraEnemy3 = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 6, enemyStartingPosition.y + 2), transform.localRotation);

                            didSpawnBlimp = true;
                        }
                    }
                }
                else if(remainder != 0)
                {
                    didSpawnBoss = false;

                    if (enemySpawnTimer >= enemySpawnTime)
                    {
                        enemyStartingPosition.y = 0;
                        enemyStartingPosition.x = 14;

                        if (!didSpawnBlimp)
                        {
                            GameObject newBlimp = Instantiate(enemy, new Vector2(enemyStartingPosition.x, enemyStartingPosition.y + (Random.Range(0, 2))), transform.localRotation);
                            didSpawnBlimp = true;
                        }

                        if (!didSpawnHelicopter)
                        {
                            GameObject newCopter = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y + 2), transform.localRotation);
                            didSpawnHelicopter = true;
                        }

                        if (!didSpawnAirplane)
                        {
                            GameObject newAirplane = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 15, enemyStartingPosition.y + 2f), transform.localRotation);
                            didSpawnAirplane = true;
                        }

                        if (!didSpawnMissleBlimp)
                        {
                            GameObject newMissleBlimp = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 1), transform.localRotation);
                            didSpawnMissleBlimp = true;
                        }
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel >= 31 && player.GetComponent<Player>().currentLevel <= 40)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                float remainder = player.GetComponent<Player>().currentLevel % 5;

                //Spawning Bosses every 5 levels:
                if (remainder == 0 && !didSpawnBoss)
                {
                    Vector2 tempBossPos;

                    tempBossPos.y = 1f;
                    tempBossPos.x = 15f;
                    GameObject newBoss = Instantiate(flameBoss, new Vector2(tempBossPos.x, tempBossPos.y), transform.localRotation);
                    newBoss.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                    newBoss.GetComponent<BoxCollider2D>().isTrigger = true;
                    didSpawnBoss = true;
                }
                else if(remainder == 0)
                {
                    if (enemySpawnTimer >= enemySpawnTime)
                    {
                        enemyStartingPosition.x = 14;
                        enemyStartingPosition.y = 0;

                        if (!didSpawnBlimp)
                        {
                            GameObject extraEnemy = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                            GameObject extraEnemy2 = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 3, enemyStartingPosition.y + 1), transform.localRotation);
                            GameObject extraEnemy3 = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 6, enemyStartingPosition.y + 2), transform.localRotation);

                            didSpawnBlimp = true;
                        }
                    }
                }
                else if (remainder != 0)
                {
                    didSpawnBoss = false;

                    if (enemySpawnTimer >= enemySpawnTime)
                    {
                        enemyStartingPosition.y = 0;
                        enemyStartingPosition.x = 14;

                        if (!didSpawnBlimp)
                        {
                            GameObject newBlimp = Instantiate(enemy, new Vector2(enemyStartingPosition.x, enemyStartingPosition.y + (Random.Range(0, 2))), transform.localRotation);
                            GameObject newBlimp2 = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + (Random.Range(0, 2))), transform.localRotation);

                            didSpawnBlimp = true;
                        }

                        if (!didSpawnHelicopter)
                        {
                            GameObject newCopter = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y + 2), transform.localRotation);

                            didSpawnHelicopter = true;
                        }

                        if (!didSpawnAirplane)
                        {
                            GameObject newAirplane = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 15, enemyStartingPosition.y + 1f), transform.localRotation);
                            GameObject newAirplane2 = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 25, enemyStartingPosition.y + 2f), transform.localRotation);

                            didSpawnAirplane = true;
                        }
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 41)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                didSpawnBoss = false;

                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0;
                    enemyStartingPosition.x = 14;
                    GameObject newBlimp = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                    GameObject newBlimpTwo = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y -1), transform.localRotation);
                    GameObject newBlimpThree = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y + 1), transform.localRotation);
                    GameObject newBlimpFour = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y - 0.5f), transform.localRotation);
                    GameObject newBlimpFive = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 12, enemyStartingPosition.y + 0.5f), transform.localRotation);
                    GameObject newBlimpSix = Instantiate(enemy, enemyStartingPosition, transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 42)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0;
                    enemyStartingPosition.x = 14;
                    GameObject newHelicopter = Instantiate(enemy2, enemyStartingPosition, transform.localRotation);
                    GameObject newHelicopterTwo = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 1), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 43)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0;
                    enemyStartingPosition.x = 14;
                    GameObject newAirplane = Instantiate(enemy3, enemyStartingPosition, transform.localRotation);
                    GameObject newBlimp = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 1), transform.localRotation);
                    GameObject newBlimpTwo = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y - 0.5f), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 44)
            {
                if(enemySpawnTimer >= enemySpawnTime)
                {
                    GameObject newAirplane = Instantiate(enemy3, enemyStartingPosition, transform.localRotation);
                    GameObject newMissileBlimp = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 15, enemyStartingPosition.y + 1f), transform.localRotation);
                    GameObject newMissileBlimpTwo = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 20, enemyStartingPosition.y - 0.5f), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 45 && !didSpawnBoss)
            {
                enemyStartingPosition.y = 1f;
                enemyStartingPosition.x = 15f;
                GameObject newBoss = Instantiate(flameBoss, enemyStartingPosition, transform.localRotation);
                GameObject newBossTwo = Instantiate(flameBoss, new Vector2(enemyStartingPosition.x, enemyStartingPosition.y + 1f), transform.localRotation);
                newBoss.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                newBoss.GetComponent<BoxCollider2D>().isTrigger = true;
                newBossTwo.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                newBossTwo.GetComponent<BoxCollider2D>().isTrigger = true;
                newBossTwo.GetComponent<Enemy>().isFlameBoss = false;
                newBossTwo.GetComponent<Enemy>().isSecondaryFlameBoss = true;
                didSpawnBoss = true;
            }
            else if(player.GetComponent<Player>().currentLevel == 46)
            {
                if (didChangeTimeScale)
                {
                    Time.timeScale = 1;
                    didChangeTimeScale = false;
                }

                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0;
                    enemyStartingPosition.x = 14;

                    if (!didSpawnBlimp)
                    {
                        GameObject newBlimp = Instantiate(enemy, new Vector2(enemyStartingPosition.x, enemyStartingPosition.y + (Random.Range(0, 2))), transform.localRotation);
                        didSpawnBlimp = true;
                    }

                    if (!didSpawnHelicopter)
                    {
                        GameObject newCopter = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y + 2), transform.localRotation);
                        didSpawnHelicopter = true;
                    }

                    if (!didSpawnAirplane)
                    {
                        GameObject newAirplane = Instantiate(enemy3, new Vector2(enemyStartingPosition.x + 15, enemyStartingPosition.y + 2f), transform.localRotation);
                        didSpawnAirplane = true;
                    }

                    if (!didSpawnMissleBlimp)
                    {
                        GameObject newMissleBlimp = Instantiate(missleBlimp, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y + 1), transform.localRotation);
                        didSpawnMissleBlimp = true;
                    }
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 47)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 0;
                    enemyStartingPosition.x = 14;
                    GameObject newBlimp = Instantiate(enemy, enemyStartingPosition, transform.localRotation);
                    GameObject newBlimpTwo = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y - 1), transform.localRotation);
                    GameObject newBlimpThree = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 8, enemyStartingPosition.y + 1), transform.localRotation);
                    GameObject newBlimpFour = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 10, enemyStartingPosition.y - 0.5f), transform.localRotation);
                    GameObject newBlimpFive = Instantiate(enemy, new Vector2(enemyStartingPosition.x + 12, enemyStartingPosition.y + 2), transform.localRotation);
                    GameObject newBlimpSix = Instantiate(enemy, enemyStartingPosition, transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 48)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 1;
                    enemyStartingPosition.x = 14;
                    GameObject newHelicopter = Instantiate(enemy2, enemyStartingPosition, transform.localRotation);
                    GameObject newHelicopterTwo = Instantiate(enemy2, new Vector2(enemyStartingPosition.x + 5, enemyStartingPosition.y - 1), transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 49)
            {
                if (enemySpawnTimer >= enemySpawnTime)
                {
                    enemyStartingPosition.y = 1;
                    enemyStartingPosition.x = 14;
                    GameObject newMissileBlimp = Instantiate(missleBlimp, enemyStartingPosition, transform.localRotation);

                    didSpawnEnemy = true;
                }
            }
            else if(player.GetComponent<Player>().currentLevel == 50 && !didSpawnBoss)
            {
                enemyStartingPosition.y = 1f;
                enemyStartingPosition.x = 15f;

                GameObject newBoss = Instantiate(missleBoss, enemyStartingPosition, transform.localRotation);
                newBoss.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                newBoss.GetComponent<BoxCollider2D>().isTrigger = true;

                GameObject newBossTwo = Instantiate(flameBoss, new Vector2(enemyStartingPosition.x, enemyStartingPosition.y + 1f), transform.localRotation);
                newBossTwo.GetComponent<Enemy>().enemyHealth = (100 + player.GetComponent<Player>().currentLevel);
                newBossTwo.GetComponent<BoxCollider2D>().isTrigger = true;
                newBossTwo.GetComponent<Enemy>().isFlameBoss = false;
                newBossTwo.GetComponent<Enemy>().isSecondaryFlameBoss = true;

                didSpawnBoss = true;
            }
        }
        else if(GameObject.Find("player").GetComponent<Player>().isPlaying && didSpawnEnemy)
        {
            //If the level's enemy has already spawned, we still force basic enemies to spawn if the player doesn't engage with the primary target

            extraEnemySpawnTimer += 1 * Time.deltaTime;

            float remainder = player.GetComponent<Player>().currentLevel % 5;

            //Checking that the player is NOT on a boss level:
            if (remainder != 0)
            {
                if (extraEnemySpawnTimer >= extraEnemySpawnTime)
                {
                    GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag("enemy");

                    float tempPos;
                    Vector2 extraEnemyStartPos;

                    if(player.GetComponent<Player>().currentLevel >= 10 && player.GetComponent<Player>().currentLevel < 15)
                    {
                        extraEnemyStartPos.y = Random.Range(-2, 3);
                        extraEnemyStartPos.x = 14f;
                    }
                    else if(player.GetComponent<Player>().currentLevel >= 15 && player.GetComponent<Player>().currentLevel < 18)
                    {
                        extraEnemyStartPos.y = Random.Range(0, 1.5f);
                        extraEnemyStartPos.x = 14f;
                    }
                    else if(player.GetComponent<Player>().currentLevel >= 18)
                    {
                        extraEnemyStartPos.y = Random.Range(1f, 1.5f);
                        extraEnemyStartPos.x = 14f;
                    }
                    else
                    {
                        extraEnemyStartPos.y = Random.Range(-4, 3);
                        extraEnemyStartPos.x = 14f;
                    }

                    foreach (GameObject enemy in enemiesOnScreen)
                    {
                        tempPos = enemy.transform.localPosition.y;

                        if (extraEnemyStartPos.y != tempPos)
                        {
                            Instantiate(Resources.Load("Prefabs/Enemy"), extraEnemyStartPos, transform.localRotation);
                        }
                    }

                    extraEnemySpawnTimer = 0;
                }
            }
        }
    }

    void KillBoss()
    {
        GameObject boss = GameObject.Find("Boss(Clone)");
        GameObject missleBoss = GameObject.Find("MissleBoss(Clone)");

        if(boss != null)
        {
            boss.GetComponent<Enemy>().isDead = true;

            boss.GetComponent<BoxCollider2D>().isTrigger = true;

            boss.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * 150);

            Destroy(boss, 2f);
        }

        if(missleBoss != null)
        {
            missleBoss.GetComponent<Enemy>().isDead = true;

            missleBoss.GetComponent<BoxCollider2D>().isTrigger = true;

            missleBoss.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * 150);

            Destroy(missleBoss, 2f);
        }
        didKillBoss = true;
    }

    void SpawnGate()
    {
        if(GameObject.Find("player").GetComponent<Player>().isPlaying && !didSpawnGate)
        {
            gatesSpawnedCounter += 1 * Time.deltaTime;
            float tempHieght;

            if (shouldGetRandom)
            {
                GetRandom();
                shouldGetRandom = false;
            }

            tempHieght = topHieght;

            if (gatesSpawnedCounter <= sameGatesInRow)
            {
                GameObject gateTop = Instantiate(Resources.Load("Prefabs/gate_top") as GameObject);
                Vector2 topPos = new Vector2(14, tempHieght);
                gateTop.transform.localPosition = topPos;
                spawnedTopGate = true;

                GameObject gateBottom = Instantiate(Resources.Load("Prefabs/gate_bottom") as GameObject);
                float bottomHieght = tempHieght - gateSize;
                Vector2 bottomPos = new Vector2(14, bottomHieght);
                gateBottom.transform.localPosition = bottomPos;
                spawnedBottomGate = true;
            }
            
            if(gatesSpawnedCounter >= sameGatesInRow)
            {
                shouldGetRandom = true;
                gatesSpawnedCounter = 0;
            }
        }

        if (spawnedBottomGate && spawnedTopGate)
        {
            didSpawnGate = true;
            spawnedBottomGate = false;
            spawnedTopGate = false;
        }
    }

    void RandomizeGatesInRow()
    {
        sameGatesInRow = Random.Range(0.009f, 0.02f);
    }

    float GetRandom()
    {
        float tempHieght = Random.Range(9, 13);
        topHieght = tempHieght;
        return topHieght;
    }

    void CheckSpawnGateTime()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            gateSpawnTimer += 1 * Time.deltaTime;

            if (didSpawnGate && gateSpawnTimer >= gateSpawnSpeed)
            {
                didSpawnGate = false;
                gateSpawnTimer = 0;
            }
        }

    }

    //Check Level is responsible for Gate Size
    void CheckLevel()
    { 
        //Making the gates close overtime starting at level 1 and going through level 50
        if(GameObject.Find("player").GetComponent<Player>().currentLevel >= 1 && GameObject.Find("player").GetComponent<Player>().currentLevel <= 50)
        {
            if (GameObject.Find("player").GetComponent<Player>().isPlaying)
            {
                if(Time.timeScale == 1)
                {
                    if (gateSize >= 17)
                    {
                        gateSize -= 0.01f * Time.deltaTime;
                    }
                }
            }
        }
        //Making the gates reopen starting at level 51
        else if(GameObject.Find("player").GetComponent<Player>().currentLevel >= 51)
        {
            if (GameObject.Find("player").GetComponent<Player>().isPlaying)
            {
                if(Time.timeScale == 1)
                {
                    if (gateSize <= 24)
                    {
                        gateSize += 0.1f * Time.deltaTime;
                    }
                }
            }
        }

        if(GameObject.Find("player").GetComponent<Player>().isPlaying == false)
        {
            if (!GameObject.Find("player").GetComponent<Player>().didGetExtraLife)
            {
                gateSize = 24f;
            }
        }
    }

    void UpdateUI()
    {
        if(GameObject.Find("player").GetComponent<Player>().playerHealth <= 0)
        {
            playerHealth.text = "Health: 0";
        }
        else
        {
            playerHealth.text = "Health: " + GameObject.Find("player").GetComponent<Player>().playerHealth.ToString();
        }

        playerScore.text = "Score: " + GameObject.Find("player").GetComponent<Player>().playerScore.ToString();
        playerAmmo.text = "Ammo: " + GameObject.Find("player").GetComponent<Player>().playerAmmo.ToString();

        if (player.GetComponent<Player>().playerIsDead)
        {
            playerHealth.GetComponent<Text>().enabled = false;
            playerScore.GetComponent<Text>().enabled = false;
            playerAmmo.GetComponent<Text>().enabled = false;
        }
        else
        {
            playerHealth.GetComponent<Text>().enabled = true;
            playerScore.GetComponent<Text>().enabled = true;
            playerAmmo.GetComponent<Text>().enabled = true;
        }

        //Updating the UI Level Text
        if (GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            if (GameObject.Find("player").GetComponent<Player>().currentLevel == 1)
            {
                levelText.GetComponent<Text>().text = "~ Level: 1 ~";

                if (!didMoveText)
                {
                    titleXPos = levelText.transform.localPosition.x;

                    if (titleXPos >= -20)
                    {
                        titleXPos -= 3 * Time.deltaTime;
                        levelText.transform.localPosition = new Vector2(titleXPos, levelText.transform.localPosition.y);
                    }
                    else
                    {
                        Vector2 levelTextStartingPos = new Vector2(18, 1.2f);
                        levelText.transform.localPosition = levelTextStartingPos;

                        didMoveText = true;
                    }
                }
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 2)
            {
                levelText.GetComponent<Text>().text = "~ Level: 2 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 3)
            {
                levelText.GetComponent<Text>().text = "~ Level: 3 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 4)
            {
                levelText.GetComponent<Text>().text = "~ Level: 4 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 5)
            {
                levelText.GetComponent<Text>().text = "~ Level: 5 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 6)
            {
                levelText.GetComponent<Text>().text = "~ Level: 6 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 7)
            {
                levelText.GetComponent<Text>().text = "~ Level: 7 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 8)
            {
                levelText.GetComponent<Text>().text = "~ Level: 8 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 9)
            {
                levelText.GetComponent<Text>().text = "~ Level: 9 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 10)
            {
                levelText.GetComponent<Text>().text = "~ Level: 10 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 11)
            {
                levelText.GetComponent<Text>().text = "~ Level: 11 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 12)
            {
                levelText.GetComponent<Text>().text = "~ Level: 12 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 13)
            {
                levelText.GetComponent<Text>().text = "~ Level: 13 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 14)
            {
                levelText.GetComponent<Text>().text = "~ Level: 14 ~";

                MoveLevelText();
            }
            else if (GameObject.Find("player").GetComponent<Player>().currentLevel == 15)
            {
                levelText.GetComponent<Text>().text = "~ Level: 15 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 16)
            {
                levelText.GetComponent<Text>().text = "~ Level: 16 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 17)
            {
                levelText.GetComponent<Text>().text = "~ Level: 17 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 18)
            {
                levelText.GetComponent<Text>().text = "~ Level: 18 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 19)
            {
                levelText.GetComponent<Text>().text = "~ Level: 19 ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel == 20)
            {
                levelText.GetComponent<Text>().text = "~ Level 20: ~";

                MoveLevelText();
            }
            else if(GameObject.Find("player").GetComponent<Player>().currentLevel >= 21)
            {
                levelText.GetComponent<Text>().text = "~ Level: " + GameObject.Find("player").GetComponent<Player>().currentLevel + " ~";

                MoveLevelText();
            }
        }
        //If the player is NOT playing...
        else
        {
            Vector2 levelTextStartingPos = new Vector2(18, 1.2f);
            levelText.transform.localPosition = levelTextStartingPos;
        }
    }

    public void MoveLevelText()
    {
        if (!didMoveText)
        {
            float titleXPos = levelText.transform.localPosition.x;

            if (titleXPos >= -20)
            {
                titleXPos -= 3 * Time.deltaTime;
                levelText.transform.localPosition = new Vector2(titleXPos, levelText.transform.localPosition.y);
            }
            else
            {
                Vector2 levelTextStartingPos = new Vector2(18, 1.2f);
                levelText.transform.localPosition = levelTextStartingPos;

                didMoveText = true;
            }
        }
    }

    public void MoveTitleText()
    {
        if (!player.GetComponent<Player>().isStartMode)
        {
            float titleXPosition = titleText.transform.localPosition.x;

            if (titleXPosition >= -1600)
            {
                titleXPosition -= 500 * Time.deltaTime;
                titleText.transform.localPosition = new Vector2(titleXPosition, titleText.transform.localPosition.y);
            }
        }
        else
        {
            Vector2 titleStartingPos = new Vector2(0, 300);
            titleText.transform.localPosition = titleStartingPos;
        }

        if (player.GetComponent<Player>().playerIsDead)
        {
            titleText.GetComponent<Image>().enabled = false;
        }
        else
        {
            titleText.GetComponent<Image>().enabled = true;
        }
    }

    public void MoveSubText()
    {
        if (!player.GetComponent<Player>().isStartMode)
        {
            float titleXPosition = subText.transform.localPosition.x;

            if(titleXPosition >= -20)
            {
                titleXPosition -= 6 * Time.deltaTime;
                subText.transform.localPosition = new Vector2(titleXPosition, subText.transform.localPosition.y);
            }
        }
        
        if(player.GetComponent<Player>().isStartMode)
        {
            Vector2 subStartingPos = new Vector2(-6.8f, -0.8f);
            subText.transform.localPosition = subStartingPos;
            subText.GetComponent<Text>().text = ("(Press Space to Start)");
        }
        
        if(player.GetComponent<Player>().shouldUpdateSubText)
        {
            Vector2 subStartingPos = new Vector2(-6.6f, -0.8f);
            subText.transform.localPosition = subStartingPos;
            subText.GetComponent<Text>().text = ("(Press Space to Continue)");
        }

        if (player.GetComponent<Player>().playerIsDead)
        {
            subText.GetComponent<Text>().enabled = false;
        }
        else
        {
            subText.GetComponent<Text>().enabled = true;
        }
    }

    public void MoveShootText()
    {
        if (!player.GetComponent<Player>().isStartMode)
        {
            float titleXPosition = shootText.transform.localPosition.x;

            if (titleXPosition >= -20)
            {
                titleXPosition -= 6 * Time.deltaTime;
                shootText.transform.localPosition = new Vector2(titleXPosition, shootText.transform.localPosition.y);
            }
        }
        else
        {
            Vector2 subStartingPos = new Vector2(18, 0);
            shootText.transform.localPosition = subStartingPos;
        }

        if (player.GetComponent<Player>().playerIsDead)
        {
            shootText.GetComponent<Text>().enabled = false;
        }
        else
        {
            shootText.GetComponent<Text>().enabled = true;
        }
    }

    public void MoveMissleText()
    {
        if (!player.GetComponent<Player>().isStartMode && player.GetComponent<Player>().currentLevel == 11)
        {
            float titleXPosition = missleText.transform.localPosition.x;
            missleText.GetComponent<Text>().text = "(Click Right Mouse to Shoot Missiles)";

            if (titleXPosition >= -20)
            {
                titleXPosition -= 6 * Time.deltaTime;
                missleText.transform.localPosition = new Vector2(titleXPosition, missleText.transform.localPosition.y);
            }
        }
        else if (!player.GetComponent<Player>().isStartMode && player.GetComponent<Player>().currentLevel == 31)
        {
            float titleXPosition = missleText.transform.localPosition.x;
            missleText.GetComponent<Text>().text = "(Missile Capacity Increased)";

            if(titleXPosition >= -20)
            {
                titleXPosition -= 6 * Time.deltaTime;
                missleText.transform.localPosition = new Vector2(titleXPosition, missleText.transform.localPosition.y);
            }
        }
        else
        {
            Vector2 subStartingPos = new Vector2(18, 0);
            missleText.transform.localPosition = subStartingPos;
        }

        if (player.GetComponent<Player>().playerIsDead)
        {
            missleText.GetComponent<Text>().enabled = false;
        }
        else
        {
            missleText.GetComponent<Text>().enabled = true;
        }
    }

    void HideBackButton()
    {
        if (GameObject.Find("player").GetComponent<Player>().isStartMode)
        {
            backButton.GetComponent<Image>().enabled = true;
            backButton.GetComponent<Button>().enabled = true;
        }
        else if (player.GetComponent<Player>().shouldUpdateSubText)
        {
            backButton.GetComponent<Image>().enabled = true;
            backButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            backButton.GetComponent<Image>().enabled = false;
            backButton.GetComponent<Button>().enabled = false;
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        buttonClick.Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void UnPause()
    {
        buttonClick.Play();
        GameObject.Find("player").GetComponent<Player>().Pause();
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
