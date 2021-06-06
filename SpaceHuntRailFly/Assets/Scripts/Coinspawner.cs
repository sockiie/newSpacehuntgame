using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CoinSpawner : MonoBehaviour
{
    public Transform player;
    public CinemachineSmoothPath cinemachine;

    public GameObject coinPrefab;


    public float boundaryX = 7.390032f;
    public float boundaryY = 4.450223f;

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
            goSpawn[i] = Instantiate(coinPrefab, Vector3.zero, Quaternion.identity);
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
                var tmp = goSpawn[asteroidIndex];
                tmp.transform.position = pos;
                if (rotate)
                {
                    Vector3 randomRotation = new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90));
                    tmp.GetComponent<Asteroid>().setRotation(randomRotation);
                }

                
                tmp.transform.localScale = new Vector3(2, 2, 2);

                asteroidIndex++;
            }

            currentZ += spawnDistance;
        }
        asteroidIndexWP1 = isFirstWPCycle ? asteroidIndex :  0;
        isFirstWPCycle = !isFirstWPCycle;
        currentWaypoint++;
   
        Debug.Log("Hello: " + isFirstWPCycle + asteroidIndexWP1 + "asterindex: " +asteroidIndex);
    }
}
