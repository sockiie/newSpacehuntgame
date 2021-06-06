using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   public Vector3 rotation = new Vector3(0,0,30);
    public GameObject explosionEffect;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            

            GameManager.Instance.CoinPickUp();
            StartCoroutine(Explode());

            transform.position = Vector3.zero;

        }

    }

    private IEnumerator Explode()
    {
        Debug.Log(transform.position);
        GameObject firework = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        firework.GetComponent<ParticleSystem>().Play();
        while (firework.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
