using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// <copyright file="CreateLevel.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/29/2016 10:23:10 AM </date>
// <summary>Class for generating the level around the player</summary>

public class CreateLevel : MonoBehaviour
{
    public GameObject orbitObj;
    public GameObject player;
    public GameObject spawnMin;

    List<GameObject> orbitZones = new List<GameObject>();
    int currentInd = 0;

    bool canSpawnOrbitZone = false;

    const float spawnDelay = 0.5f;
    float timer = 0.0f;

    int randVar;
    float randX = 0;
    float randY = 0;

    const int maxOrbitObj = 5;
    int orbitCount;

    public int OrbitCount { get { return orbitCount; } set { orbitCount = value; } }

    void Update()
    {
        if (player.GetComponent<RotateAroundObj>().TimeInAir >= 4.0f)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().m_isDead = true;
            canSpawnOrbitZone = false;
        }
        if (player.GetComponent<RotateAroundObj>().TimeInAir >= 3.0f)
        {
            canSpawnOrbitZone = false;
        }
        else { canSpawnOrbitZone = true; }

        if (canSpawnOrbitZone)
        {
            float prevX = randX;
            float prevY = randY;

            timer += Time.deltaTime;

            randX = Random.Range(-7.0f, 7.0f);
            randY = Random.Range(spawnMin.transform.position.y, spawnMin.transform.position.y + 7.0f);

            if (orbitCount < maxOrbitObj)
            {
                orbitZones.Add(Instantiate(orbitObj, new Vector3(randX, randY), Quaternion.identity) as GameObject);

                timer = 0.0f;
                if (orbitCount < maxOrbitObj) orbitCount++;

            }
        }
    }
}
