using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// <copyright file="AchievementManager.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>05/12/2016 02:38:23 PM </date>
// <summary>Class for managing in game achievements</summary>

public class AchievementManager : MonoBehaviour {
    public Acievement[] achievements;
    public Sprite[] achievementMenuSprites;
    public Text curAchievementText;
    Animator anim;
    GameManager gManager;

    void Awake(){
        curAchievementText.text = "Achievement Unlocked: ";
        anim = GameObject.Find("DropdownPanel").GetComponent<Animator>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for (int i = 1; i < achievements.Length; i++){
            if ((PlayerPrefs.GetInt(achievements[i].m_name)) == i){
                achievements[i].m_earned = true;
                achievements[i].m_slotObj.GetComponent<Image>().sprite = achievementMenuSprites[i];
                Debug.Log(achievements[i].m_name);
            }
        }
    }

    public void AwardAndSaveAchievement(string a_name){
        for (int i = 0; i < achievements.Length; i++){
            if (achievements[i].m_name == a_name){
                if (!achievements[i].m_earned){
                    achievements[i].m_earned = true;
                    anim.SetTrigger("EarnedAchievement");
                    curAchievementText.text = curAchievementText.text = "<b><color=Black>Achievement Unlocked: </color></b> \n <i><color=#272727FF>" + 
                                                                                    achievements[i].m_name + "</color></i>\n<color=White>" + 
                                                                                    achievements[i].m_desc + "</color>" + "\n\n" + "<b>REWARD:</b> <color=Yellow>" + 
                                                                                    achievements[i].m_coinReward + "</color>";
       
                    achievements[i].m_slotObj.GetComponent<Image>().sprite = achievementMenuSprites[i];
                    gManager.m_coins += achievements[i].m_coinReward;
                }
                PlayerPrefs.SetInt(a_name, i);
            }
        }
    }

    void Update(){

    }


    [System.Serializable]
    public struct Acievement {
        public string m_name;
        public string m_desc;
        public bool m_earned;
        public GameObject m_slotObj;
        public int m_coinReward;
    }
}