using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1;
    public Canvas canvas;
    public HealthBar slowMotionBar;
    public Hero hero;
    public AudioManager audioManager;

    bool slowMotion;
    bool down;
    float bannedAngle;
    Transform cameraHolder;
    Camera camera;
    Vector2 movement;
    Rigidbody2D rb;
    Vector2 mousePos;
    bool tooLongSlowMotion = false;
    
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !slowMotion && !tooLongSlowMotion) ToggleSlowMotion();
        if (Input.GetKeyUp(KeyCode.LeftShift) && slowMotion) ToggleSlowMotion();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * 1.1f * Time.fixedDeltaTime);
        if (slowMotion)
        {
            slowMotionBar.changeValue -= 1.75f;
        }
        if (!slowMotion && slowMotionBar.changeValue <= slowMotionBar.setMaxHealth)
        {
            slowMotionBar.changeValue += 0.5f;
        }
        if (slowMotionBar.changeValue >= slowMotionBar.setMaxHealth)
        {
            tooLongSlowMotion = false;
        }
        if (slowMotionBar.changeValue <= 0)
        {
            audioManager.Play("ToLongSlowMo");
            tooLongSlowMotion = true;
            ToggleSlowMotion();
        }
    }

    public void ToggleSlowMotion()
    {
        if (!slowMotion)
        {
            Time.timeScale = 0.7f;
            hero.fireRate = hero.fireRate / 2;
            moveSpeed *= 1.2f;
            slowMotion = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            hero.fireRate = hero.fireRate * 2;
            moveSpeed /= 1.2f;
            slowMotion = false;
        }
    }
}
