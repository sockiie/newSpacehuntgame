using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float rotationSpeed = 1f;
    public GameObject explosionEffect;

    private Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
    }

    public void setRotation(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            

            GameManager.Instance.PlayerHit();
            StartCoroutine(Explode());

            transform.position = Vector3.zero;
        }
    }

    private IEnumerator Explode()
    {
        GameObject firework = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        firework.GetComponent<ParticleSystem>().Play();
        while (firework.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
