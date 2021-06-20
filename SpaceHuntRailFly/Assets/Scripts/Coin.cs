using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector3 rotation;
    public float rotationSpeed = 1f;
    public GameObject explosionEffect;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
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

    public void setRotation(Vector3 _rotation)
    {
        rotation = _rotation;
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
