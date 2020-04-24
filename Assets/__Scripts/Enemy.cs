using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : MonoBehaviour
{
    public int pointsForKill;
    public int pointsForShoot;
    public float moveSpeed;
    public float fireRate = 3;
    public float projectileSpeed;
    public float damage;
    public float maxHealth;
    public float health;
    public float buffed;
    public GameObject projectile;
    public HealthBar healthBar;
    public ParticleSystem dieEffect;
    public Transform firePoint;
    public Canvas canvas;

    Spawner spawner;
    Transform player;
    Rigidbody2D rb;
    Vector2 movement;
    bool shot = true;
    Main main;
    bool canBeTaken;
    
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        main = Camera.main.GetComponent<Main>();
        if (GameObject.Find("Player") != null) player = GameObject.Find("Player").GetComponent<Transform>();
        FindObjectOfType<AudioManager>().Play("EnemySpawn");
    }

    void Update()
    {
        if (player != null) 
        { 
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction.Normalize();
            movement = direction;  
        }
        if (shot)
        {
            Invoke("Shoot", fireRate);
            shot = false;
        }
        healthBar.setMaxHealth = maxHealth;
        healthBar.changeValue = health;
    }

    void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        switch (go.tag)
        {
            case "ProjectileHero":
                Projectile proj = go.GetComponent<Projectile>();
                TakeDamage(proj.damage);
                FindObjectOfType<AudioManager>().Play("DamageToEnemy");
                break;
        }
    }

    void MoveCharacter(Vector2 direction)
    {
        if (player != null) if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 0.2f) rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        canvas.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.1f);
    }

    public void Shoot()
    {
        GameObject go1 = Instantiate<GameObject>(projectile, firePoint.position, firePoint.rotation);
        go1.GetComponent<Projectile>().damage = damage;
        Rigidbody2D rb1 = go1.GetComponent<Rigidbody2D>();
        rb1.AddForce(firePoint.up * projectileSpeed, ForceMode2D.Impulse);
        shot = true;
        FindObjectOfType<AudioManager>().Play("ProjectileDestroyEnemy");
    }

    public void TakeDamage(float value)
    {
        health = health - value;
        healthBar.changeValue = healthBar.changeValue - value;
        main.points += pointsForShoot;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        CameraShaker.Instance.ShakeOnce(1f, 4f, .1f, 1f);
        ParticleSystem dF = Instantiate(dieEffect);
        dF.transform.position = gameObject.transform.position;
        dF.Play();
        Destroy(dF.gameObject, 2f);
        Destroy(gameObject);
        main.points += pointsForKill;
        main.AddToKillCounter();
        spawner.DeleteEnemyFromList(gameObject);
        FindObjectOfType<AudioManager>().Play("Explosion");
    }

    public void Die()
    {
        CameraShaker.Instance.ShakeOnce(1f, 4f, .1f, 1f);
        ParticleSystem dF = Instantiate(dieEffect);
        dF.transform.position = gameObject.transform.position;
        dF.Play();
        Destroy(dF.gameObject, 2f);
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("Explosion");
    }

    public void Heal(float h)
    {
        health += h;
        if (health > maxHealth) health = maxHealth;
        healthBar.changeValue = health;
    }

    public void SetMaxHealth(float f)
    {
        maxHealth += f;
        healthBar.setMaxHealth = maxHealth;
        health += f;
    }
}
