using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChemicalCollsionDetection : MonoBehaviour
{
    public String tag;
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    // void Update()
    // {
    // }

    void OnCollisionEnter(Collision col){
        Debug.Log("A collision: " + tag + "; " + col.gameObject.name);
    }

    void OnTriggerEnter(Collider col){
        Debug.Log("A trigger: " + tag + "; " + col.gameObject.name);
    }
}
