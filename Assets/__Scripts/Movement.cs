using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1;
    public Canvas canvas;

    bool isBoosting;
    bool down;
    float bannedAngle;
    Transform cameraHolder;
    Camera camera;
    Vector2 movement;
    Rigidbody2D rb;
    Vector2 mousePos;
    
    void Awake()
    {
        cameraHolder = GameObject.Find("CameraHolder").GetComponent<Transform>();
        camera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        if (!camera.GetComponent<Main>().isInMapView)cameraHolder.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
        canvas.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.1f);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting) Boost();
        if (Input.GetKeyUp(KeyCode.LeftShift) && isBoosting) Boost();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void Boost()
    {
        if (!isBoosting)
        {
            moveSpeed *= 1.1f;
            isBoosting = true;
        }
        else { 
            moveSpeed = moveSpeed / 11 * 10;
            isBoosting = false;
        }
    }
}
