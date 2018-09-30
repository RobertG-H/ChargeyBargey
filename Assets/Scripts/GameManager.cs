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

    AudioSource audioSource;
    public GameObject gameSongObject;
    AudioSource gameMusic;
    public AudioClip winSong;
    public AudioClip pauseSong;
    public AudioClip readyGoClip;

    private bool playOnce = true;

	// Use this for initialization
	void Start () {
        isPaused = false;
        winCondition = GetComponent<WinCondition>();
        winCondition.OnRoundComplete += OnRoundCompleteHandler;
        audioSource = GetComponent<AudioSource>();
        gameMusic = gameSongObject.GetComponent<AudioSource>();
    }

    void Update () {

        if (playOnce) {
            playOnce = false;
            audioSource.clip = readyGoClip;
            audioSource.loop = false;
            audioSource.Play();
        }

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
        audioSource.loop = true;
        audioSource.clip = pauseSong;
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseUI.SetActive(isPaused);
        if (isPaused) {
            audioSource.Play();
            gameMusic.volume = 0;
        } else {
            audioSource.Stop();
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
        audioSource.loop = true;
        audioSource.clip = winSong;
        audioSource.Play();
        gameMusic.volume = 0;
        EndUI.SetActive(true);
        gameOver = true;
        WinnerText.GetComponent<Text>().text = msg;
        //Time.timeScale = 0f;
        Debug.Log(msg);
    }
}
