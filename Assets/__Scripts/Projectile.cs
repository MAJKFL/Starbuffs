using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2f;
    public float damage;
    public GameObject projectileImpactEffect;

    void Start()
    {
        Invoke("Die", lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        switch (go.tag)
        {
            default:
                Die();
                break;
        }
    }

    void Die()
    {
        GameObject dF = Instantiate(projectileImpactEffect);
        dF.transform.position = gameObject.transform.position;
        dF.GetComponent<ParticleSystem>().Play();
        Destroy(dF, 0.20f);
        Destroy(gameObject);
    }
}
