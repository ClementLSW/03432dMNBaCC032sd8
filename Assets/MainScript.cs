using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

	public GameObject[] pageList;
	public AnimationClip slideIn, slideOut;
	public Texture back, burger;
	bool dir = true;
	byte isService = 0;

    public void navigationSlide()
	{
		if (isService > 1)
		{
			if (isService == 2)
				pageList[3].GetComponent<Animator> ().Play ("Navigation Slide In");
			if (isService == 3)
				pageList[4].GetComponent<Animator> ().Play ("Navigation Slide In");
			if (isService == 4)
				pageList[5].GetComponent<Animator> ().Play ("Navigation Slide In");

			pageList[11].GetComponent<RawImage>().texture = burger;
			pageList[2].GetComponent<Animator> ().Play ("Navigation Slide Out");
			isService = 1;
			return;
		}

		//true = in, false = out
		pageList [9].GetComponent<Animator> ().Play (dir?"Navigation Slide In":"Navigation Slide Out");
		pageList[11].GetComponent<RawImage>().texture = dir? back:burger;
		if (isService == 1)
		{
			if (dir)
				pageList[2].GetComponent<Animator> ().Play ("Navigation Slide In");
			else
				pageList[2].GetComponent<Animator> ().Play ("Navigation Slide Out");

		}
		dir = !dir;
	}

	public void changePage(int pageNo)
	{
		offScreen();

		switch(pageNo)
		{
		case 0:
			pageList[1].SetActive(true);
			pageList[10].GetComponent<Text>().text = "Company";
			isService = 0;
			break;
		case 1:
			pageList[2].SetActive(true);
			pageList[10].GetComponent<Text>().text = "Services";
			pageList[2].GetComponent<Animator> ().Play ("Navigation Slide Out");
			isService = 1;
			break;
		case 2:
			pageList[6].SetActive(true);
			pageList[10].GetComponent<Text>().text = "Portfolio";
			isService = 0;
			break;
		case 3:
			pageList[7].SetActive(true);
			pageList[10].GetComponent<Text>().text = "AR and VR";
			isService = 0;
			break;
		case 4:
			pageList[8].SetActive(true);
			pageList[10].GetComponent<Text>().text = "Contact us";
			isService = 0;
			break;
		}
		navigationSlide();
	}

	public void changeServicesPage (int pageNo)
	{
		offScreen();

		switch(pageNo)
		{
		case 0:
			pageList[3].GetComponent<Animator> ().Play ("Navigation Slide Out");
			isService = 2;
			break;
		case 1:
			pageList[4].GetComponent<Animator> ().Play ("Navigation Slide Out");
			isService = 3;
			break;
		case 2:
			pageList[5].GetComponent<Animator> ().Play ("Navigation Slide Out");
			isService = 4;
			break;
		}
		dir = true;
		pageList[11].GetComponent<RawImage>().texture = back;
		pageList [2].GetComponent<Animator>().Play ("Navigation Slide In");
	}

	public void home()
	{
		if (pageList[0].activeSelf)
			return;
		offScreen();
		pageList[0].SetActive(true);
		pageList[10].GetComponent<Text>().text = "Hammer Studio";
		if (isService == 1)
			pageList[2].GetComponent<Animator> ().Play ("Navigation Slide In");
		if (isService == 2)
			pageList[3].GetComponent<Animator> ().Play ("Navigation Slide In");
		if (isService == 3)
			pageList[4].GetComponent<Animator> ().Play ("Navigation Slide In");
		if (isService == 4)
			pageList[5].GetComponent<Animator> ().Play ("Navigation Slide In");
		isService = 0;
		pageList[11].GetComponent<RawImage>().texture = burger;
	}

	private void offScreen()
	{
		pageList[0].SetActive(false);
		pageList[1].SetActive(false);
		pageList[6].SetActive(false);
		pageList[7].SetActive(false);
		pageList[8].SetActive(false);
	}

	public void socialMedia(int mediaNo)
	{
		switch(mediaNo)
		{
		case 0:
			Application.OpenURL("https://www.facebook.com/hammer1studio/");
		break;
		case 1:
			Application.OpenURL("");
		break;
		case 2:
			Application.OpenURL("https://www.instagram.com/hammerstudio.media/");
		break;
		case 3:
			Application.OpenURL("https://sg.linkedin.com/in/hammer-studio-509039131/");
		break;
		case 4:

		break;
		case 5:
			Application.OpenURL("https://www.youtube.com");
		break;
		}
	}


}
