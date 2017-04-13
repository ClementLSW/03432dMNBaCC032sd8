using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	public MovieTexture splash;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTexture = splash;
		splash.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!splash.isPlaying) 
		{
			canvas.SetActive (true);
			Destroy (gameObject);
		}
			
	}
}
