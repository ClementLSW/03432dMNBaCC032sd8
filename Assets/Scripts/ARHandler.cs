using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHandler : MonoBehaviour {

	public GameObject mainCanvas, ARCam;
	public MovieTexture movie;
	AudioSource aud;

	public void Awake()
	{
		GetComponent<Renderer>().material.mainTexture = movie;
		AudioSource aud = GetComponent<AudioSource>();
	}

	public void back()
	{
		mainCanvas.SetActive(true);
		gameObject.SetActive(false);
		ARCam.SetActive(false);
	}

	public void startMovie()
	{
		movie.Stop();
		movie.Play();
		aud.Play();
		StartCoroutine(movieEnd());
	}

	private IEnumerator movieEnd()
	{
		while(movie.isPlaying)
        {
         	yield return 0;
        }

		back();
     	yield break;
	}
}
