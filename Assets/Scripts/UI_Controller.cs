using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image Music, Sound;
    [SerializeField]
    Sprite musicOn, musicOff, soundOn, soundOff;
    [SerializeField]
    GameObject[] panels;
    [SerializeField]
    GameObject ui, playGame;

    public static UI_Controller instance;
    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
    }
    public void OnEnable()
    {
        Time.timeScale = 1;
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void PlayGame()
    {
        foreach (GameObject g in panels) g.SetActive(false);
        panels[4].SetActive(true);
        ui.SetActive(false);
        playGame.GetComponent<Animator>().Play("PlayGame");
        StartCoroutine(WaitLoadSecene());
    }
    public void PlayNewGame()
    {
        PlayerPrefs.SetString("SaveGame", "-1");
        PlayGame();
    }
    public void OpenPlayPanel()
    {
        if (PlayerPrefs.GetString("SaveGame") == "-1" || !PlayerPrefs.HasKey("SaveGame"))
        {
            PlayGame();
            return;
        }
        foreach (GameObject g in panels) g.SetActive(false);
        panels[6].SetActive(true);
    }
    IEnumerator WaitLoadSecene()
    {
        yield return new WaitForSecondsRealtime(.7f);
        PlayAgain();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 0) Music.sprite = musicOn;
        else Music.sprite = musicOff;
        if (PlayerPrefs.GetInt("Sound") == 0) Sound.sprite = soundOn;
        else Sound.sprite = soundOff;
    }

    public void ChangeMusicMode()
    {
        if (PlayerPrefs.GetInt("Music") == 0) AudioManager.instance.MusicOff();
        else AudioManager.instance.MusicOn();
        if (PlayerPrefs.GetInt("Music") == 0) Music.sprite = musicOn;
        else Music.sprite = musicOff;
    }

    public void ChangeSoundMode()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) AudioManager.instance.SoundOff();
        else AudioManager.instance.SoundOn();
        if (PlayerPrefs.GetInt("Sound") == 0) Sound.sprite = soundOn;
        else Sound.sprite = soundOff;
    }

    public void SoundButtonClick()
    {
        AudioManager.instance.Play("Click");
    }
    public void OpenMainPanel()
    {
        foreach (GameObject g in panels) g.SetActive(false);
        panels[0].SetActive(true);
        panels[1].SetActive(true);
        StartCoroutine(Wait());
    }
    public void OpenSettingPanel()
    {
        foreach (GameObject g in panels) g.SetActive(false);
        panels[2].SetActive(true);
    }
    public void OpenPausePanel()
    {
        Time.timeScale = 0;
        foreach (GameObject g in panels) g.SetActive(false);
        panels[2].SetActive(true);
    }
    public void OpenGameOverPanel()
    {
        AudioManager.instance.Play("Lose");
        foreach (GameObject g in panels) g.SetActive(false);
        panels[5].SetActive(true);
    }
    public void OpenHighScorePanel()
    {
        foreach (GameObject g in panels) g.SetActive(false);
        panels[3].SetActive(true);
    }
    public void OpenGiftPanel()
    {
        if (!GiftManager.outOfCoolDown) return;
        foreach (GameObject g in panels) g.SetActive(false);
        panels[5].SetActive(true);
        GiftManager.instance.ResetGift();
    }
    public void OpenReplayPanel()
    {
        Time.timeScale = 0;
        foreach (GameObject g in panels) g.SetActive(false);
        panels[4].SetActive(true);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 1;
    }
    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/deliafeva.H/");
    }
}
