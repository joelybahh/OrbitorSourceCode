using UnityEngine;
using System.Collections;

// <copyright file="Highscores.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/29/2016 11:46:13 AM </date>
// <summary>Class uploading and downloading highscores</summary>

public class Highscores : MonoBehaviour {

    const string privateCode = "LdbUAuiLyUK3ZNTns5W-Qg1xHkwGRqMEaWPzJJQ6Vvwg";
    const string publicCode = "572bd16e6e51b61a74f6297a";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoreList;

    public int highestScore;
    public string nameOfScorer;

    public Highscore[] topTen;

    void Awake(){
        DownloadHighscores();
    }

    public void AddNewHighscore(string username, int score){
        StartCoroutine(UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score){
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error)){
            print("Error uploading (null link entry): " + www.error);
        } else if (!string.IsNullOrEmpty(www.error)){
            print("Upload Success! " + www.error);
        }
        else {
            print("Error uploading: " + www.error);
        }
    }


    public void DownloadHighscores(){
        StartCoroutine("DownloadHighscoreFromDatabase");
    }

    IEnumerator DownloadHighscoreFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
            if (highscoreList.Length > 0)
            {
                highestScore = highscoreList[0].score;
                nameOfScorer = highscoreList[0].username;
            }
        }
        else {
            print("Error downloading: " + www.error);
        }
    }
    void FormatHighscores(string testStream){
        string[] entries = testStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoreList = new Highscore[entries.Length];

        for(int i = 0;i < entries.Length; i++){
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoreList[i] = new Highscore(username, score);

            //print(highscoreList[i].username + ": " + highscoreList[i].score);
        }
    }
}

[System.Serializable]
public struct Highscore {
    public string username;
    public int score;

    public Highscore(string username, int score){
        this.username = username;
        this.score = score;
    }
}

