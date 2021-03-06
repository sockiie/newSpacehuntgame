using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public Text healthText;
    public Text pointsText;
    public GameObject gameoverText;
    public PlayerMovement player;
    public GameObject explosionEffect;
    public GameObject playerShield;
    public GameObject[] health;

    public int playerHealth;
    public bool isGameOver = false;
    public bool PowerUpp = false;

    public static int points = 0;

    private void Awake()

    {
        isGameOver = false;
        gameoverText.SetActive(false);
        playerShield.SetActive(false);
        points = 0;
        if (Instance == null)
        {
            Instance = this;
           
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(this.gameObject);

        }
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }

    }

        internal void CoinPickUp()
    {
        if (playerHealth != 0)
        {
            FindObjectOfType<AudioManager>().Play("CollcectCoin1");
            points++;
            pointsText.text = "Points: " + points;
        }
    }

    public void PlayerHit()
    {

        Explode();
        //StartCoroutine(cameraShake.Shake(.15f, .4f));
        if (PowerUpp == true)
        {

            FindObjectOfType<AudioManager>().Play("CrashKometSchild");

            PowerUpp = false;
            playerShield.SetActive(false);

        }
        else if(playerHealth != 0){ 
        FindObjectOfType<AudioManager>().Play("CrashKomet");
        playerHealth--;
        
        healthText.text = "Health: " + playerHealth;
        }

        if (playerHealth < 1)
        {
            Destroy(health[0].gameObject);
        } 
        else if (playerHealth < 2)
        {
            Destroy(health[1].gameObject);
        }
        else if (playerHealth < 3)
        {
            Destroy(health[2].gameObject);
        }


        if (playerHealth == 0 && isGameOver==false)
        {
            gameoverText.SetActive(true);
            isGameOver = true;
            FindObjectOfType<AudioManager>().Play("PlayerDeath");

            GameManager.Instance.GameOver();
            player.LockMovement();
          

        }
    }

    public void ResetGame()
    {
        isGameOver = false;
        
        
            
        playerHealth = 3;
        
       
    }
    public void GameOver()
    {
        isGameOver = true;
    
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Debug.Log("Boom");
    }

    public void PowerUp(Collider player)
    {
        if (isGameOver == false) {
            FindObjectOfType<AudioManager>().Play("CollectPU");
            PowerUpp = true;
        playerShield.SetActive(true);
    Debug.Log("POOOWEER");
        }
    }



}
