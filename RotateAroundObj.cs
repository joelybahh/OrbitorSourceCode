using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// <copyright file="RotateAroundObj.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/28/2016 09:58:43 AM </date>
// <summary>Class the handles the logic and mechanics of the player</summary>

public class RotateAroundObj : MonoBehaviour
{
    AchievementManager aManager;
    enum curState { LEFT, RIGHT, NULL };
    curState state = curState.LEFT;

    public GameObject m_toRotAround;

    public Transform m_center;

    public Vector3 m_axis = Vector3.up;
    public Vector3 m_desiredPosition;
    private Vector2 m_touchOrigin = -Vector2.one;

    public float m_radius = 2.0f;
    public float m_radiusSpeed = 0.5f;
    public float m_rotationSpeed = 80.0f;

    public int m_chainCount = 0;
    bool m_nameSet = false;

    float m_timeInAir = 0.0f;
    public float TimeInAir
    {
        get { return m_timeInAir; }
    }

    bool m_moveLeft = true;
    bool m_moveRight = false;
    bool m_inBlackHoleRange = false;

    void Start()
    {
        aManager = GameObject.Find("AchievementManager").GetComponent<AchievementManager>();
        if (m_toRotAround != null) m_center = m_toRotAround.transform;
        transform.position = (transform.position - m_center.position).normalized * m_radius + m_center.position;
        m_radius = 2.0f;
    }

    void Update()
    {

#if UNITY_STANDALONE || UNITY_WEBPLAYER

        if (!m_nameSet)
        {
            //GameObject.Find("GameManager").GetComponent<GameManager>().inputField.SetActive(true);

            GameObject.Find("GameManager").GetComponent<GameManager>().m_gamesPlayedTxt.gameObject.SetActive(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().m_localHighScoreTxt.gameObject.SetActive(true);


            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs.Length; i++)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[i].SetActive(true);
            }

            //if (GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text != "")
            //{
            //    if (Input.GetMouseButtonUp(0))
            //    {
            //        m_nameSet = true;
            //        PlayerPrefs.SetString("playerName", GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text);
            //        GameObject.Find("GameManager").GetComponent<GameManager>().m_nameOfPlayer = GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text;
            //    }
            //}
        }

        if (m_nameSet)
        {
            //GameObject.Find("GameManager").GetComponent<GameManager>().inputField.SetActive(false);
            if (Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blurSpread > 0) Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blurSpread -= Time.deltaTime;
            else Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blurSpread = 0;

            if (Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blur > 0) Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blur -= Time.deltaTime;
            else Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().blur = 0;

            if (Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().chromaticAberration > 0) Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().chromaticAberration -= Time.deltaTime * 10;
            else Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>().chromaticAberration = 0;

            GameObject.Find("GameManager").GetComponent<GameManager>().m_gamesPlayedTxt.gameObject.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().m_localHighScoreTxt.gameObject.SetActive(false);

            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs.Length; i++)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[i].SetActive(false);
            }

            if (Input.GetMouseButtonDown(0) && m_toRotAround != null)
            {
                m_toRotAround = null;
            }
            else if (Input.GetMouseButtonDown(0) && m_toRotAround == null) { }

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0){
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Ended){
                m_toRotAround = null;
            }
        }   

#endif
            // if your 'chain' is greater than one, hence if you are actually in a chain
            if (m_chainCount > 1)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().m_chainTxt.gameObject.SetActive(true);
                GameObject.Find("GameManager").GetComponent<GameManager>().m_chainTxt.text = "Chain x" + m_chainCount + "!";
            }
            else {
                GameObject.Find("GameManager").GetComponent<GameManager>().m_chainTxt.gameObject.SetActive(false);
            }

            // Checks for any chain achievements
            CheckForChainAchievement();

            // If you are currently orbitting an atom or have just go into orbit of an atom
            if (m_toRotAround != null)
            {
                // set the timeInAir to equal nothing
                m_timeInAir = 0.0f;

                // ----------------------------------------------------------------------------------------------------------------                                                                                        
                transform.RotateAround(m_center.position, m_axis, m_rotationSpeed * Time.deltaTime);                            //-       This chunk of code put simply         
                m_desiredPosition = (transform.position - m_center.position).normalized * m_radius + m_center.position;         //-       rotates you around an object           
                transform.position = Vector3.MoveTowards(transform.position, m_desiredPosition, Time.deltaTime * m_radiusSpeed);//-    
                // ----------------------------------------------------------------------------------------------------------------

                // forces you to always be looking at the object you are orbitting
                transform.LookAt(m_center);

                // resets your rotation if you enter the orbit on a weird orientation, it will correct itself
                transform.rotation = new Quaternion(0, 0, transform.rotation.z, 1);

                // Sets the 'state' of your player, depending on the side of the orbit zone you are on.
                if (transform.position.x < m_center.position.x)
                {
                    state = curState.LEFT;
                }
                else if (transform.position.x > m_center.position.x)
                {
                    state = curState.RIGHT;
                }
            }
            // else if, you are not rotating around an object, hence, in space
            else if (m_toRotAround == null)
            {
                // increase your time in air value.
                m_timeInAir += Time.deltaTime;

                // Checks your state, and moves you accordingly 
                if (state == curState.LEFT)
                {
                    transform.Translate(-Vector3.up * Time.deltaTime * (m_rotationSpeed / 30), Space.Self);
                }
                else if (state == curState.RIGHT)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * (m_rotationSpeed / 30), Space.Self);
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider col){
        if (col.tag == "Cube"){
            m_toRotAround = col.gameObject;
            m_center = m_toRotAround.transform;
            transform.position = (transform.position - m_center.position).normalized * m_radius + m_center.position;

            if (m_toRotAround.GetComponent<OrbStats>().beenVisited == false)
            {
                m_toRotAround.GetComponent<OrbStats>().beenVisited = true;
                m_chainCount++;
            }

            if (transform.position.x < m_center.position.x){
                state = curState.LEFT;
                m_rotationSpeed = -m_rotationSpeed;
            }
            else if (transform.position.x > m_center.position.x){
                state = curState.RIGHT;
                m_rotationSpeed = -m_rotationSpeed;
            }
        }
        if (col.tag == "Destroyer"){
            if(m_toRotAround == null) GameObject.Find("GameManager").GetComponent<GameManager>().m_isDead = true;
        }
        if (col.tag == "Blackhole") { 

        }
    }

    void OnTriggerExit(Collider col){
        if (col.tag == "Cube"){
            if (m_toRotAround != null){
                if (m_toRotAround.GetComponent<OrbStats>().beenVisited == false){
                    m_toRotAround.GetComponent<OrbStats>().beenVisited = true;
                    m_chainCount++;
                }
            }
            if (m_toRotAround == null){            
                Debug.Log("Chain x" + m_chainCount + "!");
                m_chainCount = 0;
            }
        }
    }

    void CheckForChainAchievement(){
        if (m_chainCount < 4) return;
        if (m_chainCount >= 4)  aManager.AwardAndSaveAchievement("4Chain");    
        if (m_chainCount >= 6)  aManager.AwardAndSaveAchievement("6Chain");    
        if (m_chainCount >= 8)  aManager.AwardAndSaveAchievement("8Chain");    
        if (m_chainCount >= 10) aManager.AwardAndSaveAchievement("MegaChain"); 
    }

    public void PlayButtonHit(){
        if (GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text != "")
        {
            m_nameSet = true;
            PlayerPrefs.SetString("playerName", GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text);
            GameObject.Find("GameManager").GetComponent<GameManager>().m_nameOfPlayer = GameObject.Find("GameManager").GetComponent<GameManager>().menuObjs[0].GetComponent<InputField>().text;
        }
    }
}
