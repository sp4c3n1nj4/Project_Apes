using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //make menu slider for this
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float yScroll = 0.5f;
    [SerializeField]
    private Vector2 minMaxScroll;

    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 10f;
    public float minY = -10f;

    private void Update()
    {
        MoveCamera();
        ScrollCamera();
    }

    private void MoveCamera()
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

    private void ScrollCamera()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll == 0)
            return;

        if (gameObject.transform.position.y + yScroll * scroll > minMaxScroll.y || gameObject.transform.position.y + yScroll * scroll < minMaxScroll.x)
            return;

        gameObject.transform.Translate(Vector3.back * yScroll * scroll);
    }
}
