using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    [SerializeField]
    string MainMenuScene;

    bool isPaused;

    public GameObject PauseUI;
    public GameObject EndUI;
    public GameObject WinnerText;
    public WinCondition winCondition;
    private bool gameOver = false;

    AudioSource pauseMusic;
    public GameObject gameSongObject;
    AudioSource gameMusic;

	// Use this for initialization
	void Start () {
        isPaused = false;
        winCondition = GetComponent<WinCondition>();
        winCondition.OnRoundComplete += OnRoundCompleteHandler;
        pauseMusic = GetComponent<AudioSource>();
        gameMusic = gameSongObject.GetComponent<AudioSource>();
    }

	void Update () {

		if (Input.GetKeyDown("p") || Input.GetKeyDown("escape")) {
            if (!gameOver) {
                TogglePause();
            }
        }
        else if (Input.GetKeyDown("r")) {
            Restart();
        }
	}

    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseUI.SetActive(isPaused);
        if (isPaused) {
            pauseMusic.Play();
            gameMusic.volume = 0;
        } else {
            pauseMusic.Stop();
            gameMusic.volume = 1;
        }
    }

    public void Restart() {
        Time.timeScale = 1f;
        gameOver = false;
        if (isPaused) {
            TogglePause();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void OnRoundCompleteHandler(string msg)
    {
        EndUI.SetActive(true);
        gameOver = true;
        WinnerText.GetComponent<Text>().text = msg;
        Time.timeScale = 0f;
        Debug.Log(msg);
    }
}
