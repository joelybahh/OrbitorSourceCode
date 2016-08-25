using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Analytics;

// <copyright file="GameManager.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/28/2016 10:08:03 AM </date>
// <summary>Class for managing the whole game</summary>

public class GameManager : MonoBehaviour {

    public GameObject m_player;
    public GameObject m_camera;
    public GameObject m_highscores;
    public GameObject GameOverText;
    public GameObject[] menuObjs;
    
    public Text m_scoreTxt;
    public Text m_localHighScoreTxt;
    public Text m_gamesPlayedTxt;
    public Text m_chainTxt;
    public Text m_highScoreTxt;
    public Text m_coinTxt;

    public Text[] TopTenNames;
    public Text[] TopTenScores;

    AchievementManager aManager;

    public string m_nameOfPlayer;

    public static int m_gamesPlayed;
    public int m_highScore;
    public int m_coins;
    int m_localHighscore;

    public bool m_isDead = false;
    bool m_hasSetScore = false;
    public bool m_hasLoadedScores = false;

    public float m_score;

    void Awake()
    {
        aManager = GameObject.Find("AchievementManager").GetComponent<AchievementManager>();
        if (PlayerPrefs.HasKey("playerName")) {
            m_nameOfPlayer = PlayerPrefs.GetString("playerName");
            menuObjs[0].GetComponent<InputField>().text = m_nameOfPlayer;
        } else {
            m_nameOfPlayer = "";
            PlayerPrefs.SetString("playerName", m_nameOfPlayer);
        }
        if (PlayerPrefs.HasKey("locHighScore")){
            m_localHighscore = PlayerPrefs.GetInt("locHighScore");
        } else {
            m_localHighscore = 0;
            PlayerPrefs.SetInt("locHighScore", m_localHighscore);
        }
        if (PlayerPrefs.HasKey("Coins"))
        {
            m_coins = PlayerPrefs.GetInt("Coins");
        }else{
            PlayerPrefs.SetInt("Coins", m_coins);
        }

        if (PlayerPrefs.HasKey("gamesPlayed"))
        {
            m_gamesPlayed = PlayerPrefs.GetInt("gamesPlayed");
        }
        else {
            m_gamesPlayed = 0;
            PlayerPrefs.SetInt("gamesPlayed", m_gamesPlayed);
        }

        m_highScoreTxt.text = "Loading Highscore....";
    }

    void Update () {
#if UNITY_ANALYTICS

#endif
        m_localHighScoreTxt.text = "BEST SCORE: " + m_localHighscore;
        m_gamesPlayedTxt.text = "GAMES PLAYED: " + m_gamesPlayed;
        m_scoreTxt.text = "Score: " + (int)m_score;
        m_coinTxt.text = "Coins: " + m_coins;
        m_highScoreTxt.text = m_highscores.GetComponent<Highscores>().nameOfScorer + " has the high score of: " + m_highscores.GetComponent<Highscores>().highestScore + "!";

        PlayerPrefs.SetInt("Coins", m_coins);

        if (m_camera.GetComponent<SmoothCamera2D>().isMoving && m_player.GetComponent<RotateAroundObj>().TimeInAir < 4.0f) {
            m_score += 1f * Time.deltaTime * 10f;
        } 

        if (m_isDead){
            m_highscores.GetComponent<Highscores>().AddNewHighscore(m_nameOfPlayer, (int)m_score);
            GameOverText.SetActive(true);
            
        } else if (!m_isDead) {
            GameOverText.SetActive(false);
        }

        if (m_score > m_highscores.GetComponent<Highscores>().highestScore)
        {
            aManager.AwardAndSaveAchievement("Champion");
        }

        if (m_score >= 500)
        {
            aManager.AwardAndSaveAchievement("High Roller");
        }

        if (m_score > m_localHighscore) {
            m_localHighscore = (int)m_score;
            PlayerPrefs.SetInt("locHighScore", m_localHighscore);
        }
        
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    m_nameOfPlayer = "";
        //    menuObjs[0].GetComponent<InputField>().text = m_nameOfPlayer;
        //    PlayerPrefs.SetString("playerName", m_nameOfPlayer);
        //}

        for(int i = 0; i < 10;){
            if (!m_hasLoadedScores && m_highscores.GetComponent<Highscores>().highscoreList.Length >= 10)
            {
                if(i<9) TopTenNames[i].text = (i + 1) + ":" + "   " + m_highscores.GetComponent<Highscores>().highscoreList[i].username;
                else TopTenNames[i].text = (i + 1) + ":" + " " + m_highscores.GetComponent<Highscores>().highscoreList[i].username;
                TopTenScores[i].text = "" + m_highscores.GetComponent<Highscores>().highscoreList[i].score;
                i++;
                if (i >= 10) m_hasLoadedScores = true;
            } else break;        
        }
    }

    public void ResetGame()
    {
        m_score = 0;
        m_highScoreTxt.text = m_highscores.GetComponent<Highscores>().nameOfScorer + " has the high score of: " + m_highscores.GetComponent<Highscores>().highestScore + "!";
        m_gamesPlayed++;
        Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = false;
        PlayerPrefs.SetInt("gamesPlayed", m_gamesPlayed);
        Application.LoadLevel(0);
    }
}
