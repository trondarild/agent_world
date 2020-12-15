using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChemicalCollsionDetection : MonoBehaviour
{
    public String osc_adr;
    public OSC osc;
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
        OscMessage msg = new OscMessage();
        msg.address = osc_adr;
        msg.values.Add((float)1);
        osc.Send(msg);
        msg = null;
        Debug.Log("A collision: " + osc_adr + "; " + col.gameObject.name);
    }

    void OnTriggerEnter(Collider col){
        OscMessage msg = new OscMessage();
        msg.address = osc_adr;
        msg.values.Add((float)1);
        osc.Send(msg);
        msg = null;
        
        Debug.Log("A trigger: " + osc_adr + "; " + col.gameObject.name);
    }
}
