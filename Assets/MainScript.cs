using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

	public GameObject[] pageList;
	public AnimationClip slideIn, slideOut;
	public Sprite back, burger;
	bool dir = true;

    public Image[] galleryCircles;

    public GameObject galleryImages;

    public float swipeSpeed;

    public float snapSpeed;
    
    Vector3 swipeStartPos;

    // Variable to store the start of the swipe position
    Vector3 swipeLastPos;

    private bool isSwiping = false;

    private int currentGalleryImageID = 0;

    private int targetedLerpImageID = 0;

    private float timeHeldOnSwipe = 0;

    private bool lerpingToNearestGallery = false;

    private bool fastScroll = false;

    private float fastScrollSpeed = 250;

    private int fastScrollDir = -1;

    private bool fastScrollEnd = false;

    void Update()
    {
        if(isSwiping)
        {
            timeHeldOnSwipe += Time.deltaTime;

            // Check whether user is swiping elft or right
            Vector3 dir = Input.mousePosition - swipeLastPos;

            // If swiped right
            if(dir.x > 0)
            {
                galleryImages.transform.Translate(swipeSpeed * Time.deltaTime, 0, 0);
            }
            else if(dir.x < 0)// else if swiped left
            {
                galleryImages.transform.Translate(-swipeSpeed * Time.deltaTime, 0, 0);
            }

            swipeLastPos = Input.mousePosition;

            // Calculate current gallery image ID
            CalculateGalleryIndex();
        }

        if(lerpingToNearestGallery)
        {
            galleryImages.transform.localPosition = Vector3.Lerp(galleryImages.transform.localPosition, new Vector3(0.5f + -currentGalleryImageID * 720f, 0, 0), Time.deltaTime * 5f);
        }

        // If hit the left most or right most boundary, stop everything
        if (isSwiping || fastScroll)
        {
            CheckGalleryBoundaries();

            for(int i = 0; i < galleryCircles.Length; i++)
            {
                galleryCircles[i].color = new Color(1, 1, 1, 0.25f);
            }

            galleryCircles[currentGalleryImageID].color = Color.white;
        }

        if (fastScroll)
        {
            galleryImages.transform.Translate(fastScrollSpeed * (float)fastScrollDir * Time.deltaTime, 0, 0);

            // Reduce fast scroll speed
            fastScrollSpeed *= 0.98f;

            CalculateGalleryIndex();

            if (fastScrollEnd)
            {
                CheckGalleryBoundaries();
                LerpToNearestGallery();
                fastScroll = false;
                fastScrollEnd = false;
            }
        }
    }

    private void CalculateGalleryIndex()
    {
        for (int i = 0; i < 5; i++)  // 5 being the number of gallery images
        {
            float currentImageX = 0.5f + (-720f * i - 350f);

            currentGalleryImageID = i;

            if (galleryImages.transform.localPosition.x > currentImageX)
            {
                break;
            }
        }
    }

    private void CheckGalleryBoundaries()
    {
        float x = galleryImages.transform.localPosition.x;
        if (x >= 0 || x < -720f * 4f)
        {
            fastScrollEnd = true;
            if (x >= 0)
            {
                x = 0;
                currentGalleryImageID = 0;
            }
            else
            {
                x = -720f * 4f;
                currentGalleryImageID = 4;
            }
        }
    }

    public void navigationSlide()
	{
		//true = in, false = out
		pageList [10].GetComponent<Animator> ().Play (dir?"Navigation Slide In":"Navigation Slide Out");
		pageList[12].GetComponent<Image>().sprite = dir? back:burger;
		dir = !dir;
	}

	public void changePage(int pageNo)
	{
		offScreen();

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
		offScreen();

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

	public void offScreen()
	{
		for (int i = 0; i< 9; ++i)
			pageList[i].SetActive(false);
	}

    public void startGallerySwipe()
    {
        swipeStartPos = Input.mousePosition;
        swipeLastPos = Input.mousePosition;
        isSwiping = true;
        lerpingToNearestGallery = false;
        fastScroll = false;
    }

    public void endGallerySwipe()
    {
        isSwiping = false;

        // If ratio is over threshold, do not snap to nearest instantly. let gallery scroll through

        float distSwiped = Vector3.Distance(Input.mousePosition, swipeStartPos);

        float ratio = distSwiped / timeHeldOnSwipe;
        
        if(ratio > 3000)
        {
            // Scroll through everything
            fastScrollSpeed = ratio;
            if(swipeStartPos.x > Input.mousePosition.x)
            {
                fastScrollDir = -1;
            }
            else
            {
                fastScrollDir = 1;
            }
            SetFastScroll();
        }
        else
        {
            // Ratio is not high enough, snap to the nearest gallery image
            LerpToNearestGallery();
        }

        timeHeldOnSwipe = 0;
    }

    void LerpToNearestGallery()
    {
        lerpingToNearestGallery = true;
    }

    void SetFastScroll()
    {
        fastScroll = true;
    }
}
