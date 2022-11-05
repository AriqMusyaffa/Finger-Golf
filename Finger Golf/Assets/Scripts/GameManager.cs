using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    MusicManager MM;
    AmbienceManager AM;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text finalShootCount;
    [SerializeField] TMP_Text finalTimeCount;
    [SerializeField] PlayerController player;
    [SerializeField] Hole hole;
    [SerializeField] GameObject confetti;
    [SerializeField] TMP_Text shootCountText;
    [SerializeField] TMP_Text scoreText;

    bool gameWin = false;
    public bool GameWin => gameWin;

    [SerializeField] int par;
    [SerializeField] string score;
    [SerializeField] TMP_Text stageText;
    [SerializeField] TMP_Text parText;
    [SerializeField] AudioSource buttonSound;

    void Start()
    {
        gameOverPanel.SetActive(false);
        scoreText.gameObject.SetActive(false);

        var currentSceneName = SceneManager.GetActiveScene().name;
        var currentStage = int.Parse(currentSceneName.Split("Stage ")[1]);
        
        switch (currentStage)
        {
            case 1:
                par = 1;
                break;
            case 2:
                par = 2;
                break;
            case 3:
                par = 2;
                break;
            case 4:
                par = 2;
                break;
            case 5:
                par = 1;
                break;
            case 6:
                par = 2;
                break;
            case 7:
                par = 3;
                break;
            case 8:
                par = 3;
                break;
            case 9:
                par = 5;
                break;
        }

        stageText.text = "Stage " + currentStage;
        parText.text = "Par : " + par;

        MM = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
        if (MM.EverAudio.clip != MM.ingameMusic)
        {
            MM.EverAudio.clip = MM.ingameMusic;
            MM.EverAudio.Play();
        }

        AM = GameObject.FindWithTag("AmbienceManager").GetComponent<AmbienceManager>();
        if (AM.EverAmbience.clip != AM.ingameAmbience)
        {
            AM.EverAmbience.clip = AM.ingameAmbience;
            AM.EverAmbience.Play();
        }

        buttonSound.volume = SaveLoad.soundVolume;
        buttonSound.Play();
    }

    void Update()
    {
        if (!gameWin)
        {
            if (hole.Entered && !gameOverPanel.activeInHierarchy)
            {
                gameWin = true;
                confetti.SetActive(true);
                stageText.gameObject.SetActive(false);
                parText.gameObject.SetActive(false);
                shootCountText.gameObject.SetActive(false);
                GetComponent<TimerManager>().HideTimer();

                if (player.ShootCount == par && par > 1)
                {
                    score = "Par!";
                }
                else if (player.ShootCount == 1)
                {
                    score = "Hole-in-one!";
                }
                else if (player.ShootCount == par + 1)
                {
                    score = "Bogey!";
                }
                else if (player.ShootCount == par + 2)
                {
                    score = "Double Bogey!";
                }
                else if (player.ShootCount == par + 3)
                {
                    score = "Triple Bogey!";
                }
                else if (player.ShootCount == par - 1)
                {
                    score = "Birdie!";
                }
                else if (player.ShootCount == par - 2)
                {
                    score = "Eagle!";
                }
                else if (player.ShootCount == par - 13)
                {
                    score = "Albatross!";
                }
                else
                {
                    score = "";
                }

                scoreText.text = score;
                scoreText.gameObject.SetActive(true);
                StartCoroutine(StageOver());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    IEnumerator StageOver()
    {
        TimerManager TM = GetComponent<TimerManager>();
        TM.counting = false;
        yield return new WaitForSeconds(2);
        gameOverPanel.SetActive(true);
        finalShootCount.text = "Shoot Count : " + player.ShootCount;
        finalTimeCount.text = "Time : " + TM.timerTxt.text;
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("Main Menu");
    }

    public void Replay()
    {
        SceneLoader.ReloadStage();
    }

    public void PlayNext()
    {
        if (SceneManager.GetActiveScene().name == "Stage 9")
        {
            SaveLoad.comingSoonOn = true;
            SceneLoader.Load("Main Menu");
        }
        else
        {
            SceneLoader.LoadNextStage();
        }
    }

    public void TextHover(TMP_Text text)
    {
        text.color = Color.yellow;
    }

    public void TextUnhover(TMP_Text text)
    {
        text.color = Color.white;
    }
}
