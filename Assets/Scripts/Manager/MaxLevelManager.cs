using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Text text;
    [SerializeField]
    Image[] frames, stars;
    [SerializeField]
    Sprite yStar, gStar;
    [SerializeField]
    Image render;
    public static int maxLevel;

    // Update is called once per frame
    void Update()
    {
        text.text = maxLevel.ToString();
        render.sprite = GameManager.instance.dataDragon.dragons[maxLevel].sprite;
        render.SetNativeSize();
        for(int i = 0; i < frames.Length; i++) frames[i].enabled = (GameManager.instance.dataDragon.dragons[maxLevel].idFrame == i);
        for (int i = 0; i < stars.Length; i++) stars[i].sprite = maxLevel >= 4 * (i + 1) ? yStar : gStar;
        if (maxLevel > PlayerPrefs.GetInt("MaxLevel")) PlayerPrefs.SetInt("MaxLevel", maxLevel);
    }
}
