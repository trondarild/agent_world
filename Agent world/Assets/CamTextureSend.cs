using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TextureSendReceive;
using System;
public class CamTextureSend : MonoBehaviour
{
    // Start is called before the first frame update
    //TextureSender sender;
	//public Texture2D sendTextureLeft;
	//public RenderTexture renderTextureLeft;
	public Camera sourceCamLeft;

	// public Texture2D sendTextureRight;
	// public RenderTexture renderTextureRight;
	public Camera sourceCamRight;
	
	 public OSC osc;
	 public int maxSz = 24;
	 public bool send_depth=true;
	 public bool send_right=true;
	 public bool send_left=true;
	 enum PixelType {eRGB, eGray};


	void Start () {
		//osc = new OSC();
		//osc.Awake();
		
	  // Get sender instance
	  //sender = GetComponent<TextureSender>();
	  
	  // Initialize Texture2D, in this case with webcamTexture dimensions
	  
	  
	  // Set send texture
	  //sender.SetSourceTexture(sendTexture);
	}

    // Update is called once per frame
	void Update(){
		// left cam can send depth image; send and turn off
		if(send_depth){
			sourceCamLeft.GetComponent<RenderDepth>().enabled = true;
			SendCamera(sourceCamLeft,
						"left/depth", PixelType.eGray);
			sourceCamLeft.GetComponent<RenderDepth>().enabled = false;	
		}
		if(send_left)
			SendCamera(sourceCamLeft,
					"left", PixelType.eRGB);			
		if(send_right)
			SendCamera(sourceCamRight,
					"right", PixelType.eRGB);
	}

    void SendCamera(//Texture2D sendTexture,
					//RenderTexture renderTexture,
					Camera sourceCam,
					String msgTag,
					PixelType pxType) {
    		RenderTexture renderTexture = new RenderTexture(sourceCam.pixelWidth, sourceCam.pixelHeight, 24);
    		sourceCam.targetTexture = renderTexture;
    		sourceCam.Render();
    		RenderTexture.active = renderTexture;
    		Rect r = new Rect(0, 0, renderTexture.width, renderTexture.height);
    		var textureFormat = TextureFormat.RGB24;
			if(pxType == PixelType.eRGB)
				textureFormat = TextureFormat.RGB24;
			else
				textureFormat = TextureFormat.R8;
    		Texture2D sendTexture = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);	
    		sendTexture.ReadPixels(r, 0, 0);
 			sendTexture.Apply();
 			
 			Color[] pixels = sendTexture.GetPixels();
 			//debug("**before pixels = ", pixels);
 			
 			//sendTexture.Resize(maxSz, maxSz);
 			//sendTexture.Apply();
 			Texture2D newTex = Instantiate(sendTexture);	
 			TextureScale.Bilinear (newTex, maxSz, maxSz);
    		
    		pixels = newTex.GetPixels();
    		//debug("**after pixels = ", pixels);
    		float[] pixelvals = new float[maxSz*maxSz];
    		//Debug.Log("***Pixels: " + pixels.ToString());
    	if(pxType == PixelType.eRGB){
			// get texture and send it over osc
			OscMessage message_r;
			OscMessage message_g;
			OscMessage message_b;

			message_r = new OscMessage();
			message_g = new OscMessage();
			message_b = new OscMessage();
			message_r.address = "/" + msgTag + "/camera_r";
			message_g.address = "/" + msgTag + "/camera_g";
			message_b.address = "/" + msgTag + "/camera_b";
			//for(int i=0; i<renderTexture.width * renderTexture.height; i++){
			//Debug.Log("osc update, pixel length: " + pixels.Length);	
			for(int i=0; i<Mathf.Min(pixels.Length, maxSz*maxSz); i++){
					message_r.values.Add((float)pixels[i].r);
					message_g.values.Add((float)pixels[i].g);
					message_b.values.Add((float)pixels[i].b);
			}
			//message.values.Add(pixelvals);
			osc.Send(message_r);
			osc.Send(message_g);
			osc.Send(message_b);
			//DestroyImmediate(renderTexture);
			message_r = null;
			message_g = null;
			message_b = null;
		}
		else{
			 // get texture and send it over osc
			OscMessage message_r;
			
			message_r = new OscMessage();
			message_r.address = "/" + msgTag + "/camera";
			//for(int i=0; i<renderTexture.width * renderTexture.height; i++){
			//Debug.Log("osc update, pixel length: " + pixels.Length);	
			for(int i=0; i<Mathf.Min(pixels.Length, maxSz*maxSz); i++){
					message_r.values.Add((float)pixels[i].r);
					
			}
			//message.values.Add(pixelvals);
			osc.Send(message_r);
			//DestroyImmediate(renderTexture);
			message_r = null;
			
		}
		
       

		renderTexture = null;
		sendTexture = null;
    }

    void debug(String txt, Color[] a){
    	Debug.Log(txt +String.Join("",
             new List<Color>(a)
             .ConvertAll(i => i.ToString())
             .ToArray()));
    }
}
