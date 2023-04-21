using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //make menu slider for this
    [SerializeField]
    private float speed = 1f;

    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 10f;
    public float minY = -10f;

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        if (direction.magnitude > 1)
            direction = direction.normalized;

        Vector3 pos = gameObject.transform.position + speed * Time.deltaTime * direction;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minY, maxY);

        gameObject.transform.position = pos;
    }
}
