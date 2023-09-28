using Array = System.Array;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds, musics;

    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        Play("GameMusic");
        if (PlayerPrefs.GetInt("Sound") == 0) SoundOn();
        else SoundOff();
        if (PlayerPrefs.GetInt("Music") == 0) MusicOn();
        else MusicOff();
    }
    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) sound = Array.Find(musics, sound => sound.name == name);
        if (sound == null) return;
        sound.source.Play();
    }
    public void SoundOff()
    {
        PlayerPrefs.SetInt("Sound", 1);
        foreach (Sound s in sounds)
            s.source.volume = 0;
    }
    public void MusicOff()
    {
        PlayerPrefs.SetInt("Music", 1);
        foreach (Sound s in musics)
        {
            s.source.Pause();
        }
    }
    public void SoundOn()
    {
        PlayerPrefs.SetInt("Sound", 0);
        foreach (Sound s in sounds)
            s.source.volume = 1;
    }
    public void MusicOn()
    {
        PlayerPrefs.SetInt("Music", 0);
        foreach (Sound s in musics)
        {
            s.source.UnPause();
        }
    }
}
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    [HideInInspector] public AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    public bool loop;

}