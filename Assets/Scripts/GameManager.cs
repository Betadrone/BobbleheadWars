
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;

    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;

    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;

    public GameObject upgradePrefab;
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;
    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

    public GameObject deathFloor;
    public Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        currentUpgradeTime += Time.deltaTime;
        if (currentUpgradeTime > actualUpgradeTime)
        {
            // 1
            if (!spawnedUpgrade)
            {
                // 2
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // 3
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // 4
                spawnedUpgrade = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        //currentSpawnTime is how much time has passed since last alien spawn in Update
        currentSpawnTime += Time.deltaTime;

        //conditional statement to spawn a new batch of aliens as long as time is larger than preset random value
        if (currentSpawnTime > generatedSpawnTime)
        {
            //resets the timer after spawn
            currentSpawnTime = 0;

            //randomizer for spawn time delay
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            //Checks if the number of aliens is within limits
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                //Keeps track of where you have already spawned aliens
                List<int> previousSpawnLocations = new List<int>();

                //Limits Aliens' amount to number of spawnPoints
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                //Preventative code to make sure you do not spawn more Aliens than you have configured
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                //Runs once for each spawned Alien
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        //Keeps track of the number of Aliens spawned
                        aliensOnScreen += 1;

                        //Value of -1 means no index has been assigned or found for the spawnpoint
                        int spawnPoint = -1;

                        //While loop keeps looking for a spawnpoint that has not been used yet
                        while (spawnPoint == -1)
                        {
                            //create a random index of List(array) between 0 and number of spawnpoints
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            //Checks if random spawnpoint has already been used
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                //Add this random number to the list
                                previousSpawnLocations.Add(randomNumber);
                                //Use this random number as the spawn index
                                spawnPoint = randomNumber;
                            }
                        }

                        //Actual point(label) on arena to spawn next alien
                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        //creates a new alien from prefab
                        GameObject newAlien = Instantiate(alien) as GameObject;

                        //position the new alien to the random unused spawnpoint
                        newAlien.transform.position = spawnLocation.transform.position;

                        //get the alien script from the new alien spawned
                        Alien alienScript = newAlien.GetComponent<Alien>();

                        //Set the alien's target as the player's position
                        alienScript.target = player.transform;

                        //gets alien to turn towards the target
                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                            newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);

                        alienScript.OnDestroy.AddListener(AlienDestroyed);
                        alienScript.GetDeathParticles().SetDeathFloor(deathFloor);
                    }
                }
            }
        }
    }

    public void AlienDestroyed()
    {
        aliensOnScreen -= 1;
        totalAliens -= 1;
        if(totalAliens == 0)
        {
            Invoke("endGame", 2.0f);
        }
    }

    private void endGame()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.elevatorArrived);
        arenaAnimator.SetTrigger("PlayerWon");
    }
}
