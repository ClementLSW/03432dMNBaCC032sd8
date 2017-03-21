using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

	public GameObject[] pageList;
	public AnimationClip slideIn, slideOut;
	public Sprite back, burger;
	bool dir = true;

	public void navigationSlide()
	{
		//true = in, false = out
		pageList [10].GetComponent<Animator> ().Play (dir?"Navigation Slide In":"Navigation Slide Out");
		pageList[12].GetComponent<Image>().sprite = dir? back:burger;
		dir = !dir;
	}

	public void changePage(int pageNo)
	{
		pageList[0].SetActive(false);
		pageList[1].SetActive(false);
		pageList[2].SetActive(false);
		pageList[7].SetActive(false);
		pageList[8].SetActive(false);
		pageList[9].SetActive(false);

		switch(pageNo)
		{
		case 0:
			pageList[1].SetActive(true);
			pageList[11].GetComponent<Text>().text = "Company";
			break;
		case 1:
			pageList[2].SetActive(true);
			pageList[11].GetComponent<Text>().text = "Services";
			break;
		case 2:
			pageList[7].SetActive(true);
			pageList[11].GetComponent<Text>().text = "Portfolio";
			break;
		case 3:
			pageList[8].SetActive(true);
			pageList[11].GetComponent<Text>().text = "AR and VR";
			break;
		case 4:
			pageList[9].SetActive(true);
			pageList[11].GetComponent<Text>().text = "Contact us";
			break;
		}
		navigationSlide();
	}

	public void changeServicesPage (int pageNo)
	{
		for (int i = 2; i< 7; ++i)
			pageList[i].SetActive(false);

		switch(pageNo)
		{
		case 0:
			pageList[3].SetActive(true);
			break;
		case 1:
			pageList[4].SetActive(true);
			break;
		case 2:
			pageList[5].SetActive(true);
			break;
		case 3:
			pageList[6].SetActive(true);
			break;
		}
	}

	public void home()
	{
		if (pageList[0].activeSelf)
			return;
		for (int i = 1; i< 10; ++i)
			pageList[i].SetActive(false);
		pageList[0].SetActive(true);
		pageList[11].GetComponent<Text>().text = "Hammer Studio";
	}
}
