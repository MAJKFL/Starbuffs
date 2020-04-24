using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class Hero : MonoBehaviour
{
    public float projectileSpeed;
    public float fireRate;
    public float maxHealth;
    public float health;
    public float damage;
    public float currentHealthLoosing = 0.5f;
    public float buffed;
    public GameObject projectile;
    public ParticleSystem dieEffect;
    public GameObject spawnerPrefab;
    public ParticleSystem takeOverEffect;
    public HealthBar healthBar;
    public Spawner spawner;

    float lastShoot;
    bool rotate;
    Transform firePoint;
    Main main;
    
    void Awake()
    {
        firePoint = transform.Find("FirePoint").GetComponent<Transform>();
        main = Camera.main.GetComponent<Main>();
        FindObjectOfType<AudioManager>().Play("PlayerSpawn");
    }

    void Update()
    {
        if (rotate) transform.Rotate(Vector3.forward *5* Time.deltaTime);
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetKeyDown(KeyCode.E)) TakeOver();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        healthBar.setMaxHealth = maxHealth;
        healthBar.changeValue = health;
    }

    void FixedUpdate()
    {
        TakeDamage(currentHealthLoosing *Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        switch (go.tag)
        {
            case "ProjectileEnemy":
                CameraShaker.Instance.ShakeOnce(0.25f, 4f, .1f, 1f);
                Projectile proj = go.GetComponent<Projectile>();
                TakeDamage(proj.damage);
                FindObjectOfType<AudioManager>().Play("DamageToPlayer");
                break;
        }
    }

    public void Shoot()
    {
        if (Time.time - lastShoot > fireRate)
        {
            
            GameObject go1 = Instantiate<GameObject>(projectile, firePoint.position, transform.rotation);
            Rigidbody2D rb1 = go1.GetComponent<Rigidbody2D>();
            SpriteRenderer sp1 = go1.GetComponent<SpriteRenderer>();
            Projectile proj1 = go1.GetComponent<Projectile>();
            rb1.AddForce(firePoint.up * projectileSpeed, ForceMode2D.Impulse);
            lastShoot = Time.time;
            proj1.damage = damage;
            FindObjectOfType<AudioManager>().Play("ProjectileDestroyPlayer");
        }
    }

    public void TakeDamage(float value)
    {
        health = health - value;
        healthBar.changeValue = healthBar.changeValue - value;
        if (health <= 0) Die();
    }

    void Die()
    {
        CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
        ParticleSystem dF = Instantiate(dieEffect);
        dF.transform.position = gameObject.transform.position;
        dF.Play();
        main.Defeat();
        Destroy(gameObject);
        Destroy(dF, 0.5f);
        FindObjectOfType<AudioManager>().Play("Explosion");
    }

    public void Heal(float h)
    {
        health += h;
        if (health > maxHealth) health = maxHealth;
        healthBar.changeValue = health;
    }

    public void TakeOver()
    {
        if (main.points >= main.pointsNeedToTakeOver && spawner.GetFirstEnemy() != null)
        {
            ParticleSystem dF = Instantiate(dieEffect);
            dF.transform.position = gameObject.transform.position;
            dF.Play();
            GameObject go = spawner.GetFirstEnemy();
            gameObject.transform.position = go.transform.position;
            gameObject.GetComponent<Rigidbody2D>().SetRotation(go.transform.rotation);
            Enemy enem = go.GetComponent<Enemy>();
            buffed += spawner.buffCounter;
            currentHealthLoosing += 0.5f;
            gameObject.GetComponent<Movement>().moveSpeed = enem.moveSpeed;
            fireRate = enem.fireRate;
            projectileSpeed = enem.projectileSpeed;
            damage = enem.damage;
            main.points -= main.pointsNeedToTakeOver;
            spawner.DeleteEnemyFromList(enem.gameObject);
            Destroy(go);
            main.pointsNeedToTakeOver += 500;
            spawner.buffCounter = 0;
            spawner.DestroyAllEnemies();
            Destroy(spawner.gameObject);
            Instantiate<GameObject>(spawnerPrefab);
            takeOverEffect.Play();
            CameraShaker.Instance.ShakeOnce(1f, 4f, .1f, 1f);
            FindObjectOfType<AudioManager>().Play("PlayerSpawn");
            for (int i = 0; i < buffed; i++)
            {
                Buff();
            }
            healthBar.setMaxHealth = maxHealth;
            healthBar.changeValue = health;
            health = maxHealth;
        }
    }

    void Buff()
    {
        SetMaxHealth(3);
        switch (Random.Range(1, 5))
        {
            case 1:
            {
                gameObject.GetComponent<Movement>().moveSpeed += Random.Range(0.01f, 0.03f);
                break;
            }
            case 2:
            {
                fireRate -= Random.Range(0.01f, 0.03f);
                break;
            }
            case 3:
            {
                projectileSpeed += Random.Range(0.01f, 0.03f);
                break;
            }
            case 4:
            {
                damage += Random.Range(0.01f, 0.03f);
                break;
            }
        }
    }

    public void SetMaxHealth(float f)
    {
        maxHealth += f;
        healthBar.setMaxHealth = maxHealth;
        health += f;
    }
}
