using UnityEngine;
using System.Collections;

// <copyright file="SmoothCamera2D.cs" company="BlackPandaStudios">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Joel Gabriel</author>
// <date>04/28/2016 12:15:01 PM </date>
// <summary>Class for smooth camera follow in the game</summary>

public class SmoothCamera2D : MonoBehaviour {
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Vector3 highestPosReached;
    public bool isMoving = false;

    // Update is called once per frame
    void Update(){
        if (target){
            if (target.position.y > highestPosReached.y){
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
                Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(point.x, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                highestPosReached.y = target.position.y;
                isMoving = true;
            } else { isMoving = false; }
        }
    }
}
