using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscReceiveCtrl : MonoBehaviour
{
    public OSC osc;
    public CharacterController controller;
    public Transform playerBody;
    public Vector3 resetPosition;
    public Vector3 resetRotation;
    bool posSemaphore = true;
    // public float rotation=0;
    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler( "/forward" , OnForward );
       osc.SetAddressHandler("/backward", OnBackward);
       osc.SetAddressHandler("/rotate_left", OnRotLeft);
       osc.SetAddressHandler("/rotate_right", OnRotRight);
       osc.SetAddressHandler("/reset", OnReset);
    }

    // Update is called once per frame
    void Update()
    {
        // get osc messages
    }

    void OnForward(OscMessage message){
        //if(posSemaphore){
            posSemaphore = false;
            // get intensity of forward
            float z = message.GetFloat(0);
            Debug.Log("osc update, received forward: " + z);	

            Vector3 move = transform.forward * z; 
            controller.Move(move * Time.deltaTime);
            //controller.transform.position += move* Time.deltaTime;
            posSemaphore = true;
        //}
		
	}

    void OnBackward(OscMessage message){
        float z = message.GetFloat(0);
        Debug.Log("osc update, received backward: " + z);	

        Vector3 move = transform.forward * -z; 
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

    void OnReset(OscMessage message){
        Quaternion resetQuat = Quaternion.Euler(resetRotation);
        //Quaternion q = new Quaternion();
        //q.SetFromToRotation(playerBody.rotation, tmp);
        
        //playerBody.position = resetPosition;
        controller.transform.rotation = Quaternion.identity*resetQuat;
        //if(posSemaphore){
            posSemaphore=false;
            //controller.transform.position = resetPosition;
            //Debug.Log("reset pos=" + resetPosition);
            //playerBody.SetPositionAndRotation(resetPosition, q);
            Vector3 tmp = Vector3.up * 100;
            controller.Move(tmp - playerBody.position);
            controller.Move(resetPosition - tmp);
            //playerBody.Rotate(q);
            Debug.Log("received reset: " + playerBody.position + "; " + playerBody);
            posSemaphore = true;
        //}
    }
}
