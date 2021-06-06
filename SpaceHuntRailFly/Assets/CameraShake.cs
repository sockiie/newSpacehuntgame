using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0F;

        while (elapsed < duration)
        {

            float x = Random.Range(20f, 20f) * magnitude;
            float y = Random.Range(-20f, 20f) * magnitude;


            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;

        }

        transform.localPosition = originalPos;

    }


}
