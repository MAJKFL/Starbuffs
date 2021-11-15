using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Spawner : MonoBehaviour
{
    public int reminingBuffs;
    public int buffCounter;
    public float spawnRate;
    public Vector3 center;
    public Vector3 size;
    public GameObject enemyPrefab;
    public Hero hero;

    bool spawned = true;
    bool buffed = true;
    bool spawns = true;
    TextMeshProUGUI remainingBuffsText;
    List<GameObject> enemyList;
    
    void Awake()
    {
        enemyList = new List<GameObject>();
        hero = GameObject.Find("Player").GetComponent<Hero>();
        spawnRate -= hero.buffed / 10;
        if (spawnRate < 0.5f) spawnRate = 0.5f;
        remainingBuffsText = GameObject.Find("RemainingBuffs").GetComponent<TextMeshProUGUI>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(center, size);
    }

    public void SpawnEnemy()
    {
        if (spawns)
        {
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), 0);
            enemyList.Add(Instantiate(enemyPrefab, pos, Quaternion.identity));
            for (int i = 0; buffCounter > i; i++)
            {
                Buff(enemyList[enemyList.Count - 1].GetComponent<Enemy>());
            }
            spawned = true;
        }
        spawned = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && reminingBuffs > 0){
            Buff();
            buffed = false;
            buffCounter++;
            reminingBuffs--;
            FindObjectOfType<AudioManager>().Play("Buff");
        }
        if (Input.GetButtonUp("Jump"))
        {
            buffed = true;
        }
        if (spawned)
        {
            Invoke("SpawnEnemy", spawnRate);
            spawned = false;
        }
        remainingBuffsText.text = "" + reminingBuffs;
    }

    void Buff()
    {
        if (GameObject.Find("Player") != null)   hero.Heal(hero.maxHealth / 5);
        for (int i = 0; enemyList.Count > i; i++)
        {
            Enemy enemy = enemyList[i].GetComponent<Enemy>();
            enemy.SetMaxHealth(Random.Range(5f, 15f));
            enemy.Heal(enemy.maxHealth);
            switch(Random.Range(1, 5))
            {
                case 1:
                {
                    enemy.moveSpeed += Random.Range(0.03f, 0.06f);
                    break;
                }
                case 2:
                {
                    enemy.fireRate -= Random.Range(0.03f, 0.06f);
                    break;
                }
                case 3:
                {
                    enemy.projectileSpeed += Random.Range(0.03f, 0.06f);
                    break;
                }
                case 4:
                {
                    enemy.damage += Random.Range(0.03f, 0.06f);
                    break;
                }
            }
        }
    }

    void Buff(Enemy enem)
    {
        enem.SetMaxHealth(Random.Range(5f, 15f));
        switch (Random.Range(1, 5))
        {
            case 1:
            {
                enem.moveSpeed += Random.Range(0.03f, 0.06f);
                break;
            }
            case 2:
            {
                enem.fireRate -= Random.Range(0.03f, 0.06f);
                break;
            }
            case 3:
            {
                enem.projectileSpeed += Random.Range(0.03f, 0.06f);
                break;
            }
            case 4:
            {
                enem.damage += Random.Range(0.03f, 0.06f);
                break;
            }
        }
    }

    public GameObject GetFirstEnemy()
    {
        return enemyList[0];
    }

    public void DeleteEnemyFromList(GameObject enem)
    {
        enemyList.Remove(enem);
    }

    public void DestroyAllEnemies()
    {
        for (int i = 0; enemyList.Count > i; i++)
        {
            enemyList[i].GetComponent<Enemy>().Die();
            spawns = false;
        }
        enemyList.Clear();
    }
}
