using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;

    [Header("Limits")]
    public Vector2 limits = new Vector2(5, 3);

    [Header("Smooth Damp Time")]
    [Range(0, 1)]
    public float smoothTime;

     //Update is called once per frame
    void Update()
    {
        Vector3 localPos = transform.localPosition;
        Vector3 targetLocalPos = player.transform.localPosition;
        transform.localPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x, targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothTime);
    }

    void LateUpdate()
    {
        Vector3 localPos = transform.localPosition;

        transform.localPosition = new Vector3(Mathf.Clamp(localPos.x, -limits.x, limits.x), Mathf.Clamp(localPos.y, -limits.y, limits.y), localPos.z);
    }
}
