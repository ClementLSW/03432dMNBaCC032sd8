using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHandler : MonoBehaviour {

	public GameObject mainCanvas, ARCam;

	public void back()
	{
		mainCanvas.SetActive(true);
		gameObject.SetActive(false);
		ARCam.SetActive(false);
	}

	public void startMovie()
	{
		Handheld.PlayFullScreenMovie ("VR.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
	}

}
