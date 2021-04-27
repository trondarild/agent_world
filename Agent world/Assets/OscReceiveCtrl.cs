using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscReceiveCtrl : MonoBehaviour
{
    public OSC osc;
    public CharacterController controller;
    public Transform playerBody;
    // public float rotation=0;
    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler( "/forward" , OnForward );
       osc.SetAddressHandler("/backward", OnBackward);
       osc.SetAddressHandler("/rotate_left", OnRotLeft);
       osc.SetAddressHandler("/rotate_right", OnRotRight);
    }

    // Update is called once per frame
    void Update()
    {
        // get osc messages
    }

    void OnForward(OscMessage message){
        // get intensity of forward
		float z = message.GetFloat(0);
        Debug.Log("osc update, received forward: " + z);	

        Vector3 move = transform.forward * z; 
        controller.Move(move * Time.deltaTime);
        
		
	}

    void OnBackward(OscMessage message){
        float x = message.GetFloat(0);
        Vector3 move = transform.forward * -x; 
        controller.Move(move * Time.deltaTime);
    }

    void OnRotLeft(OscMessage message){
        float x = message.GetFloat(0);
        float rotation = -x*Time.deltaTime;
        playerBody.Rotate(Vector3.up * rotation);
        Debug.Log("osc update, received rotleft: " + x);
    }

    void OnRotRight(OscMessage message){
        float x = message.GetFloat(0);
        float rotation = x*Time.deltaTime;
        playerBody.Rotate(Vector3.up * rotation);
        Debug.Log("osc update, received rotright: " + x);
    }
}
