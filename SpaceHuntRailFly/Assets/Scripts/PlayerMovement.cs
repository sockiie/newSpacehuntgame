using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float xSpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;
    private float rotationZ = 0f;
    public float RotationSpeed = 200f;
    private bool isReset;
    private bool isMoving = false;
    private bool startRotation = false;
    private float idleTime = 0f;
    public float resetSpeed = 2f;
    public CinemachineDollyCart dolly;

    private void Start()
    {

        dolly.m_Speed = forwardSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rotationZ = Input.GetAxis("Horizontal") * RotationSpeed;
        rotationZ = Mathf.Clamp(rotationZ, -50, 50);

        //rotationX = Input.GetAxis("Vertical") * RotationSpeed * -1;
        //rotationX = Mathf.Clamp(rotationX, -30, 30);

        transform.localRotation = Quaternion.Euler(0, 0, -rotationZ);

        LocalMove(h*1.5f, v*1.5f, xSpeed);

        dolly.m_Speed += Time.deltaTime/5;

    }

    void LocalMove(float x, float y, float speed)
    {
        if (isOnEdge(Camera.main.WorldToViewportPoint(transform.position).x, x))
        {
            x = 0;
        }
        if (isOnEdge(Camera.main.WorldToViewportPoint(transform.position).y, y))
        {
            y = 0;
        }
        transform.localPosition += new Vector3(x * speed, y * (speed* 9/16), 0) * Time.deltaTime;
        ClampPosition();
    }

    internal void LockMovement()
    {
        dolly.m_Speed = 0f;
        xSpeed = 0f;
    }

    private bool isOnEdge(float x, float moveDir)
    {
        return (x <= 0.2f && moveDir < 0)|| (x >= 0.8f && moveDir > 0);
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.05f, 0.95f);
        if (!ignoreSmallChanges(transform.position, Camera.main.ViewportToWorldPoint(pos)))
        {
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        
    }

    private bool ignoreSmallChanges(Vector3 position, Vector3 rel)
    {
        var xDif = Mathf.Abs(position.x - rel.x);
        var yDif = Mathf.Abs(position.y - rel.y);
        var zDif = Mathf.Abs(position.z - rel.z);
        return xDif + yDif + zDif < 0.1f;
    }

    private IEnumerator ResetRotation()
    {
        if (isReset) yield break;
        isReset = true;
        float timer = 0f;
        Quaternion startRot = transform.rotation;
        while (timer <= 1f)
        {
            if (isMoving)
            {
                isReset = false;
                yield break;
            }
            timer += Time.deltaTime * resetSpeed;
            transform.rotation = Quaternion.Slerp(startRot, Quaternion.identity, timer);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.identity;
        isReset = false;
        yield break;
    }
}
