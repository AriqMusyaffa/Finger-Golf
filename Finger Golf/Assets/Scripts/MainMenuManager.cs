using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel, stageSelectPanel, settingsPanel, comingSoonPanel;
    [SerializeField] Scrollbar stageScrollbar;
    MusicManager MM;
    AmbienceManager AM;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource buttonSound;
    bool isQuit = false;
    float quitTime = 0f;

    void Start()
    {
        MM = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
        AM = GameObject.FindWithTag("AmbienceManager").GetComponent<AmbienceManager>();

        if (SaveLoad.StagePanelOn)
        {
            StageSelect();
        }

        if (SaveLoad.comingSoonOn)
        {
            ComingSoon();
        }

        SaveLoad.LoadData();
        stageScrollbar.value = SaveLoad.stageScrollValue;
        volumeSlider.value = SaveLoad.soundVolume;

        if (MM.EverAudio.clip != MM.menuMusic)
        {
            MM.EverAudio.clip = MM.menuMusic;
            MM.EverAudio.Play();
        }

        if (AM.EverAmbience.isPlaying)
        {
            AM.EverAmbience.Stop();
        }

        buttonSound.volume = SaveLoad.soundVolume;
        buttonSound.Play();
    }

    void Update()
    {
        SaveLoad.soundVolume = volumeSlider.value;
        MM.EverAudio.volume = SaveLoad.soundVolume;
        AM.EverAmbience.volume = SaveLoad.soundVolume;

        if (isQuit)
        {
            if (quitTime < 0.5f)
            {
                quitTime += Time.deltaTime;
            }
            else
            {
                Application.Quit();
            }
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

    public void MainMenu()
    {
        SaveLoad.StagePanelOn = false;
        SaveLoad.SaveData();
        SceneLoader.Load("Main Menu");
    }

    public void StageSelect()
    {
        SaveLoad.StagePanelOn = true;
        stageSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        buttonSound.Play();
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        buttonSound.Play();
    }

    public void ComingSoon()
    {
        SaveLoad.StagePanelOn = true;
        SaveLoad.comingSoonOn = false;
        mainMenuPanel.SetActive(false);
        stageSelectPanel.SetActive(false);
        comingSoonPanel.SetActive(true);
        SaveLoad.stageScrollValue = 1;
        buttonSound.Play();
    }

    public void ReloadStageSelect()
    {
        SaveLoad.comingSoonOn = false;
        SaveLoad.StagePanelOn = true;
        SceneLoader.Load("Main Menu");
    }

    public void GoToStage(int num)
    {
        SaveLoad.stageScrollValue = stageScrollbar.value;
        SceneLoader.ProgressLoad("Stage " + num);
    }

    public void QuitGame()
    {
        buttonSound.Play();
        isQuit = true;
    }
}
