using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;



public class Main : MonoBehaviour
{
    public int points;
    public int pointsNeedToTakeOver = 2000;
    public bool isInMapView = false;
    public TextMeshProUGUI killCounter;
    public TextMeshProUGUI pointsCounter;
    public TextMeshProUGUI pointsNeedToTakeOverText;
    public GameObject highScoreText;
    public Spawner spawner;
    public Transform cameraHolder;
    public GameObject mapButton;
    
    int killCount;
    int buffCounter;
    bool defeat;
    Camera camera;
    Animator defeatPanel;

    void Awake()
    {
        defeatPanel = GameObject.Find("DefeatPanel").GetComponent<Animator>();
        camera = Camera.main;
    }

    void Update()
    {
        pointsCounter.text = "" + points;
        pointsNeedToTakeOverText.text = "" + pointsNeedToTakeOver;
        if (GameObject.Find("Spawner") != null) spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    public void Defeat()
    {
        defeatPanel.SetBool("show", true);
        spawner.DestroyAllEnemies();
        Time.timeScale = 0.5f;
        defeat = true;
        Destroy(mapButton.GetComponent<Button>());
        if (Save.HighScore < killCount) 
        { 
            Save.HighScore = killCount;
            highScoreText.SetActive(true);
        }
        if (spawner != null) Destroy(spawner.gameObject);
    }

    public void AddToKillCounter()
    {
        killCount++;
        killCounter.text = "" + killCount;
    }

    public void MapView()
    {
        if (!isInMapView && !defeat)
        {
            Time.timeScale = 0;
            isInMapView = true;
            camera.orthographicSize = 4;
            cameraHolder.transform.position = new Vector3(0, 0, -10);
        }
        else if (!defeat)
        {
            Time.timeScale = 1;
            isInMapView = false;
            camera.orthographicSize = 0.75f;
        }
    }
}
