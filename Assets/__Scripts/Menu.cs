using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public AudioManager audioManager;

    bool waitForLoad;
    float timer;
    float timerWait;
    Animator fade;
    AsyncOperation async;
    
    void Awake()
    {
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        if (highScoreText!= null) highScoreText.text = "High score: " + Save.HighScore;
    }

    void Start()
    {
        waitForLoad = false;
        timerWait = 1.0f;
        if (highScoreText == null)
        {
            audioManager.Play("Music");
        } else
        {
            audioManager.Play("TitleScreen");
        }
    }

    void Update()
    {
        if (waitForLoad)
        {
            timer += Time.deltaTime;
            if(timer >= timerWait)
            {
                waitForLoad = false;
                async.allowSceneActivation = true;
            }
        }
    }

    public void LoadScene(string a)
    {
        Time.timeScale = 1;
        fade.SetBool("show", true);
        waitForLoad = true;
        async = SceneManager.LoadSceneAsync(a);
        async.allowSceneActivation = false;
    }

    public void LoadScene(int a)
    {
        Time.timeScale = 1;
        fade.SetBool("show", true);
        waitForLoad = true;
        async = SceneManager.LoadSceneAsync(/*SceneManager.GetActiveScene().buildIndex + */a);
        async.allowSceneActivation = false;
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
