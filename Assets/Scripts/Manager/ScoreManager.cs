using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public static int score, highScore, tmpScore;
    [SerializeField]
    Text scoreText, hScoreText, scoreText2, hScoreText2, tScoreText;
    [SerializeField]
    Animator animScore;
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
    private void Start()
    {
        if (PlayerPrefs.GetString("SaveGame") != "-1") score = PlayerPrefs.GetInt("SaveScore");
        else score = 0;
        Debug.Log(PlayerPrefs.GetInt("SaveScore"));
        highScore = PlayerPrefs.GetInt("HighScore");
        scoreText.text = scoreText2.text = score.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (tScoreText.color.a > 0) tScoreText.color += new Color(0, 0, 0, -1 * Time.deltaTime);
        if (tmpScore > 0)
        {
            tScoreText.text ="+ " + tmpScore.ToString();
            score += tmpScore;
            tScoreText.color = new Color(1, 0, 0, 1);
            animScore.Play("Score");
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", score);
            }
            scoreText.text = scoreText2.text = score.ToString();
            hScoreText.text = "BEST " + highScore.ToString();
            hScoreText2.text = highScore.ToString();
            tmpScore = 0;
            PlayerPrefs.SetInt("SaveScore", score);
        }
    }
    public void Increascore(int TmpScore)
    {
        tmpScore = TmpScore;
    }
}
