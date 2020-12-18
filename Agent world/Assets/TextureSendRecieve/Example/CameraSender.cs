using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace TextureSendReceive {
	[RequireComponent(typeof(Camera), typeof(TextureSender))]
	public class CameraSender : MonoBehaviour {
		Camera cam;
		TextureSender sender;
		Texture2D sendTexture;
		RenderTexture videoTexture;

		public RawImage image;

		// Use this for initialization
		void Start () {
			cam = GetComponent<Camera>();
			sender = GetComponent<TextureSender>();

			sendTexture = new Texture2D((int)cam.targetTexture.width, (int)GetComponent<Camera>().targetTexture.width);
			
			// Set send texture
			sender.SetSourceTexture(sendTexture);
		}
		
		// Update is called once per frame
		void Update () {
			RenderTexture.active = cam.targetTexture;
			sendTexture.ReadPixels(new Rect(0, 0, GetComponent<Camera>().targetTexture.width, GetComponent<Camera>().targetTexture.height), 0, 0, false);
				
			// Set preview image target
			image.texture = cam.targetTexture;
		}
	}
}