using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscReceiveCtrl : MonoBehaviour
{
    public OSC osc;
    public CharacterController controller;
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
        //float y = message.GetInt(1);
		//float z = message.GetInt(2);
        Vector3 move = transform.forward * z; 
        controller.Move(move * Time.deltaTime);
        
		//transform.position = new Vector3(x,y,z);

        /*
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    	  if(isGrounded && velocity.y < 0){
    	  		velocity.y = -2f;
    	  }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; 
        controller.Move(move * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && isGrounded)
        		velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        */
	}

    void OnBackward(OscMessage message){
        float x = message.GetFloat(0);
    }

    void OnRotLeft(OscMessage message){
        float x = message.GetFloat(0);
    }

    void OnRotRight(OscMessage message){
        float x = message.GetFloat(0);
    }
}
