using UnityEngine;
using System.Collections;

public class ResetBackground : MonoBehaviour {
    public Vector3 resetPos;
    public GameObject bg;
    float occurances = 1;
    void OnTriggerEnter(Collider col) {
        if (col.tag == "BGMarker"){
            bg.transform.position = resetPos * occurances;
            occurances++;
        } 
    }
}
