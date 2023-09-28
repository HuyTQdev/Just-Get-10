using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GiftManager : MonoBehaviour
{
    [SerializeField]
    Image[] Button, Image;
    [SerializeField]
    Text text;
    [SerializeField]
    Sprite block, row, darkItem, yellowItem;
    bool isReceive;
    public DateTime oldTime;
    TimeSpan coolDown;
    public static bool outOfCoolDown;
    [SerializeField]
    Text time;
    public static GiftManager instance;
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
        coolDown = new TimeSpan(0, 5, 0);
        Debug.Log(PlayerPrefs.GetString("Old Time"));
        if(PlayerPrefs.HasKey("Old Time"))oldTime = DateTime.ParseExact(PlayerPrefs.GetString("Old Time"), "dd/MM/yyyy HH:mm:ss", null);
    }
    public void ResetGift()
    {
        isReceive = false;
        foreach (Image i in Button) i.sprite = yellowItem;
        text.transform.position = Image[0].transform.position = Image[1].transform.position = new Vector3(-50, 0, 0);
    }
    private void Update()
    {
        TimeSpan disTime = System.DateTime.Now - oldTime;
        if (disTime.Days > 0)
        {
            time.text = "00:00:00";
            outOfCoolDown = true;
        }
        else
        {
            disTime = coolDown - disTime;
            if (disTime.Hours < 0 || disTime.Minutes < 0 || disTime.Seconds < 0)
            {
                time.text = "00:00:00";
                outOfCoolDown = true;
            }
            else
            {
                time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", disTime.Hours, disTime.Minutes, disTime.Seconds);
                outOfCoolDown = false;
            }
        }
    }
    // Start is called before the first frame update
    void OpenGift(int i)
    {
        if (isReceive) return;
        isReceive = true;
        int num = Random.Range(1, 4);
        int kind = Random.Range(1, 3);
        text.text = num.ToString();

        for(int j = 0; j < 3; j++)
        {
            if (i == j)
            {
                for (int k = 0; k < 3; k++)
                {
                    Image[0].transform.position = Button[j].transform.position;
                    Image[1].transform.position = Button[j].transform.position + new Vector3(0.1f, 0, 0);
                    text.transform.position = Button[j].transform.position - new Vector3(0.1f, 0, 0);
                }
            }
            else Button[j].sprite = darkItem;
        }
        if (kind == 1)
        {
            PlayerPrefs.SetInt("DeleteBlock", PlayerPrefs.GetInt("DeleteBlock") + num);
            Image[1].sprite = block;
        }
        else
        {
            PlayerPrefs.SetInt("DeleteRow", PlayerPrefs.GetInt("DeleteRow") + num);
            Image[1].sprite = row;
        }
        oldTime = System.DateTime.Now;
        PlayerPrefs.SetString("Old Time", string.Format("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}", oldTime.Day, oldTime.Month, oldTime.Year, oldTime.Hour, oldTime.Minute, oldTime.Second));
    }

    // Update is called once per frame
    public void Gift0(){ OpenGift(0); }
    public void Gift1() { OpenGift(1); }
    public void Gift2() { OpenGift(2); }
}
