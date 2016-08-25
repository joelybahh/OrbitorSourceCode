using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour {
    public GameObject[] orderVariations;
    GameManager gm;
    Highscores hs;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        hs = GameObject.Find("HighscoreManager").GetComponent<Highscores>();
    }

    public void Clicked(GameObject button){
        if (!button.GetComponent<ButtonState>().isMain)
        {
            switch (button.tag)
            {
                case "HomeIcon":
                    orderVariations[0].SetActive(true);
                    orderVariations[1].SetActive(false);
                    orderVariations[2].SetActive(false);
                    orderVariations[3].SetActive(false);
                    orderVariations[4].SetActive(false);
                    break;
                case "LeaderboardIcon":
                    orderVariations[0].SetActive(false);
                    orderVariations[1].SetActive(true);
                    orderVariations[2].SetActive(false);
                    orderVariations[3].SetActive(false);
                    orderVariations[4].SetActive(false);
                    gm.m_hasLoadedScores = false;
                    hs.DownloadHighscores();                
                    break;
                case "ShopIcon":
                    orderVariations[0].SetActive(false);
                    orderVariations[1].SetActive(false);
                    orderVariations[2].SetActive(true);
                    orderVariations[3].SetActive(false);
                    orderVariations[4].SetActive(false);
                    break;
                case "SettingsIcon":
                    orderVariations[0].SetActive(false);
                    orderVariations[1].SetActive(false);
                    orderVariations[2].SetActive(false);
                    orderVariations[3].SetActive(true);
                    orderVariations[4].SetActive(false);
                    break;
                case "AchievementIcon":
                    orderVariations[0].SetActive(false);
                    orderVariations[1].SetActive(false);
                    orderVariations[2].SetActive(false);
                    orderVariations[3].SetActive(false);
                    orderVariations[4].SetActive(true);
                    break;
            }
        }
    }
}