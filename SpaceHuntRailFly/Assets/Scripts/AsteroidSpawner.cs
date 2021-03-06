using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class AsteroidSpawner : MonoBehaviour
{
    public Transform player;
    public CinemachineSmoothPath cinemachine;

    public GameObject asteroidPrefab;
    public GameObject asteroidPrefab2;

    public float boundaryX = 0.1f;
    public float boundaryY = 0.1f;

    // offset between spawning gameObjects, currently only working on the z axis
    public float spawnDistance = 2f;

    public bool shouldDelaySpawn = true;
    public bool rotate = true;

    // amount of gameObjects that should spawn in the same z value
    public int spawnAmountCycle = 10;

    private Vector3 start;
    private Vector3 end;
    private float currentZ = 0f;



    // Array of all the instantiated gameobjects
    private GameObject[] goSpawn;

    // with this boolean we can spawn 2 Waypoints at the same time
    private bool isFirstWPCycle = true;
    //index for goSpawn[] after spawn of Waypoint 1
    private int asteroidIndexWP1 = 0;
    public int goAmount = 5000;

    private CinemachineSmoothPath.Waypoint[] waypoints;
    private int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = cinemachine.m_Waypoints;
        goSpawn = new GameObject[goAmount];
        InitiateGameObjects();
       // SpawnGameObjectsAlongWaypoint();
      
    }



    /// <summary>
    /// No Instantiations during game time is for better performance during playing
    /// </summary>
    private void InitiateGameObjects()
    {
        for (int i = 0; i < goAmount; i++)
        {
            if(i % 2 == 0)
            {
            goSpawn[i] = Instantiate(asteroidPrefab, Vector3.zero, Quaternion.identity);

            }

            else if(i % 2 != 0)
            {

            goSpawn[i] = Instantiate(asteroidPrefab2, Vector3.zero, Quaternion.identity);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaypoint < waypoints.Length-1 && player.transform.position.z+30 >= waypoints[currentWaypoint].position.z)
        {
            
            SpawnGameObjectsAlongWaypoint();
        }
    }

    private void SpawnGameObjectsAlongWaypoint()
    {
        //current index of goSpawn[]

        int asteroidIndex;
      
        if (isFirstWPCycle)
        {
            asteroidIndex = 0;
        }
        else
        {
            asteroidIndex = asteroidIndexWP1 + 1;
        }
        
        if (currentWaypoint == waypoints.Length - 1)
        {
            return;
        }
        start = waypoints[currentWaypoint].position;
        end = waypoints[currentWaypoint + 1].position;

        if(currentWaypoint == 0 && shouldDelaySpawn)
        {
            currentZ = start.z + 10 ;
        }
        else
        {
            currentZ = start.z + spawnDistance;

        }

        //spawn gameObjects as long as the end z value is not reached
        while (currentZ < end.z - spawnDistance)
        {
            //spawn the amount of gameObjects for a z value
            for(int i = 0; i < spawnAmountCycle; i++)
            {
                Vector3 pos = new Vector3(UnityEngine.Random.Range(start.x - boundaryX, end.x + boundaryX), UnityEngine.Random.Range(start.y - boundaryY, end.y + boundaryY), currentZ);
                var hitColliders = Physics.OverlapSphere(pos, (float)0.1);

            if(hitColliders.Length > 0.1)
                {
                  

                }else { 
                var tmp = goSpawn[asteroidIndex];
                tmp.transform.position = pos;

                if (rotate)
                {
                    Vector3 randomRotation = new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90));

                        if(asteroidPrefab.name == "Komet_v1") { 
                        tmp.GetComponent<Asteroid>().setRotation(randomRotation);
                        }

                        if (asteroidPrefab.name == "Coin_final1")
                        {
                            tmp.GetComponent<Coin>().setRotation(randomRotation);
                        }
                        if (asteroidPrefab.name == "PowerUp_neu")
                        {
                            tmp.GetComponent<PowerUp>().setRotation(randomRotation);
                        }
                    }


                float randomSize = UnityEngine.Random.Range(1, 3);
                tmp.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

                asteroidIndex++;
                }
            }

            currentZ += spawnDistance;
        }
        asteroidIndexWP1 = isFirstWPCycle ? asteroidIndex :  0;
        isFirstWPCycle = !isFirstWPCycle;
        currentWaypoint++;
   
        Debug.Log("Hello: " + isFirstWPCycle + asteroidIndexWP1 + "asterindex: " +asteroidIndex);
    }
}
