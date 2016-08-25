using UnityEngine;
using System.Collections;

// <copyright file="DeleteOrbitZone.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/29/2016 03:02:23 PM </date>
// <summary>Class for deleting orbit zones</summary>

public class DeleteOrbitZone : MonoBehaviour
{
    bool fixedPos = false;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Destroyer")
        {
            GameObject.Find("GameSpawnManager").GetComponent<CreateLevel>().OrbitCount--;
            Destroy(gameObject);
            Debug.Log("destroyed");
        }


    }

    void OnTriggerStay(Collider col)
    {

        if (col.tag == "Cube")
        {
            if (!fixedPos)
            {
                col.gameObject.transform.position = new Vector3(col.gameObject.transform.position.x + 3.4f, col.gameObject.transform.position.y + 3.4f);
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        fixedPos = true;
    }
}
