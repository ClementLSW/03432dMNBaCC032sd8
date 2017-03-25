using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class MainScript : MonoBehaviour {

	public GameObject[] pageList;
	public AnimationClip slideIn, slideOut;
	public Texture back, burger;
	bool dir = true;
	byte isService = 0;
	public GameObject ARCanvas, ARCam, submitBtn, alert;
	public Text title, message;
	public InputField[] contactUsInput;

    // Portfolio page variables
    byte isPortfolio = 0;
    byte nextPortfolioImageIndex = 0;
    byte currentPortfolioImageIndex = 0;
    public float portfolioGallerySwipeSpeed = 5;

    public GameObject portfolioObj;
    public ScrollRect portfolioScrollRect;

    Vector2 initialTouchPosition;

    // Variables for auto mode
    public enum GALLERY_MODE
    {
        AUTO,
        MANUAL,
        MAX_MODE
    }

    GALLERY_MODE mode = GALLERY_MODE.AUTO;

    float modeTimer = 0;

    bool autoStop = false;

    float stopTimer = 0;

    public float autoGlanceDuration = 3;

    Vector3 portfolioGalleryOriginalPos;

    public void Start()
    {
        portfolioGalleryOriginalPos = portfolioObj.transform.localPosition;
    }

    public void Update()
    {
        GetTouchInput();

        GetMouseInput();

        if (mode == GALLERY_MODE.MANUAL)
        {
            if (nextPortfolioImageIndex != currentPortfolioImageIndex)
            {
                StartCoroutine(Swipe(currentPortfolioImageIndex));

                currentPortfolioImageIndex = nextPortfolioImageIndex;
            }
            else // if user is looking at the same screen for more than x seconds, switch to auto mode
            {
                modeTimer += Time.deltaTime;
                if (modeTimer >= 3)
                {
                    mode = GALLERY_MODE.AUTO;
                    isPortfolio = 0;
                    modeTimer = 0;
                }
            }

        }
        else
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= autoGlanceDuration)
            {
                if(nextPortfolioImageIndex + 1 > 4)
                {
                    nextPortfolioImageIndex = 0;
                }
                else
                {
                    nextPortfolioImageIndex += 1;
                }
                StartCoroutine(Swipe(currentPortfolioImageIndex));

                currentPortfolioImageIndex = nextPortfolioImageIndex;

                stopTimer = 0;
            }
        }
    }

    IEnumerator Swipe (byte currentImageIndex)
    {
        modeTimer = 0;
        stopTimer = 0;
        isPortfolio = 0;
        portfolioScrollRect.vertical = false;
        portfolioScrollRect.velocity = Vector2.zero;

        Vector3 targetPos = portfolioObj.transform.localPosition;

        float speedToMoveAt = portfolioGallerySwipeSpeed;
        
        // Swipe left
        if (nextPortfolioImageIndex > currentImageIndex)
        {
            //targetPos -= Vector3.right * 720f;
			targetPos = new Vector3((-nextPortfolioImageIndex * 720f) + portfolioGalleryOriginalPos.x, portfolioObj.transform.localPosition.y, portfolioGalleryOriginalPos.z);
        }
        else if(nextPortfolioImageIndex < currentImageIndex)// Swipe right
        {
            //targetPos += Vector3.right * 720f;
			targetPos = new Vector3((-nextPortfolioImageIndex * 720f) + portfolioGalleryOriginalPos.x, portfolioObj.transform.localPosition.y, portfolioGalleryOriginalPos.z);
            Debug.Log(targetPos);
        }

        if(currentImageIndex == 4 && nextPortfolioImageIndex == 0)
        {
            targetPos = portfolioGalleryOriginalPos;
            speedToMoveAt = 5000;
        }

        float distance = 720;

        GALLERY_MODE modeThatStartedSwipe = mode;

        while(distance > 1)
        {
            portfolioObj.transform.localPosition = Vector3.MoveTowards(portfolioObj.transform.localPosition, targetPos, speedToMoveAt * Time.deltaTime);

            distance = Vector3.Distance(portfolioObj.transform.localPosition, targetPos);
            
            if(modeThatStartedSwipe != mode)
            {
                break;
            }

            yield return null;
        }

        portfolioScrollRect.vertical = true;
        isPortfolio = 1;
    }

    public void GetTouchInput()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                initialTouchPosition = new Vector2(t.position.x, t.position.y);
                portfolioScrollRect.vertical = false;
                portfolioScrollRect.velocity = Vector2.zero;
                mode = GALLERY_MODE.MANUAL;
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                Vector2 endedTouchPosition = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                Vector2 currentSwipe = new Vector3(endedTouchPosition.x - initialTouchPosition.x, endedTouchPosition.y - initialTouchPosition.y);

                //normalize the 2d vector
                currentSwipe.Normalize();
                
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    ++nextPortfolioImageIndex;
                    if(nextPortfolioImageIndex > 4)
                    {
                        nextPortfolioImageIndex = 4;
                    }
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (currentPortfolioImageIndex > 0)
                    {
                        --nextPortfolioImageIndex;
                    }
                }
            }
        }
    }

    public void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            initialTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mode = GALLERY_MODE.MANUAL;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            Vector2 endedTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            Vector2 currentSwipe = new Vector2(endedTouchPosition.x - initialTouchPosition.x, endedTouchPosition.y - initialTouchPosition.y);

            //normalize the 2d vector
            currentSwipe.Normalize();
            
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                ++nextPortfolioImageIndex;
                if (nextPortfolioImageIndex > 4)
                {
                    nextPortfolioImageIndex = 4;
                }
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (currentPortfolioImageIndex > 0)
                {
                    --nextPortfolioImageIndex;
                }
            }
        }
    }

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

        if(pageNo != 2)
        {
            isPortfolio = 0;
        }
        else
        {
            isPortfolio = 1;
        }
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
			ARCanvas.SetActive(true);
			gameObject.SetActive(false);
			ARCam.SetActive(true);
		break;
		case 5:
			Application.OpenURL("https://www.youtube.com");
		break;
		}
	}

	public void contactUsSubmit()
	{
		foreach (InputField i in contactUsInput)
        {
        	if (i.text == "")
        	{
				setAlert ("ERROR", "You cannot leave any blanks!");
				return;
        	}
        }

		if (!contactUsInput[1].text.Contains("@") || !contactUsInput[1].text.Contains("."))
		{
			setAlert ("ERROR", "Your email is invalid!\nPlease check and then try again!");
			return;
		}

		submitBtn.SetActive(false);

		MailMessage mail = new MailMessage();
 
		mail.From = new MailAddress("HammerStudioBot@gmail.com");
		mail.To.Add("roselanaraf@hammer-studio.com");
		mail.Subject = contactUsInput[2].text;
		mail.Body = string.Format("Name: {0}\nEmail: {1}\n\n{2}", contactUsInput[0].text, contactUsInput[1].text, contactUsInput[3].text);

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("HammerStudioBot@gmail.com", "HammerStudioBot123") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = 
        delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

        foreach (InputField i in contactUsInput)
        {
        	i.text = "";
        }

		setAlert ("Success!", "Email has been sent!\nPlease await your reply =D");
		submitBtn.SetActive(true);
	}

	private void setAlert(string titleInput, string messageInput)
	{
		title.text = titleInput;
		message.text = messageInput;
		alert.SetActive(true);
	}

	public void ok()
	{
		alert.SetActive(false);
	}
}
