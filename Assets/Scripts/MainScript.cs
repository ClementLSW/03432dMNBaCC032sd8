using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Vuforia;

public class MainScript : MonoBehaviour {

	public GameObject[] pageList;
	public AnimationClip slideIn, slideOut;
	public Texture back, burger;
	bool dir = true, isViewMore = false; 
	public bool isPortfolio = false;
	byte isService = 0;
	public GameObject ARCanvas, ARCam, submitBtn, alert, viewMoreList, picDisplay, imageDisplay;
	public Text title, message;
	public InputField[] contactUsInput;
	public Texture[] displayImages;

	void Start()
	{
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) navigationSlide();
	}

    public void navigationSlide()
	{
		if (isViewMore)
		{
			pageList[9].SetActive(true);
			viewMoreList.SetActive(false);
			picDisplay.SetActive(false);
			isViewMore = false;
			Screen.orientation = ScreenOrientation.Portrait;
			return;
		}
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
            isPortfolio = false;
        }
        else
        {
            isPortfolio = true;
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
			Application.OpenURL("https://www.youtube.com/channel/UC_CBq8I6S2agegl8FO_5-mg");
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
			Application.OpenURL("https://www.youtube.com/watch?v=K6vAYWeGVYE");
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

	public void viewMore()
	{
		Screen.orientation = ScreenOrientation.AutoRotation;
		viewMoreList.SetActive(true);
		pageList[9].SetActive(false);
		isViewMore = true;
	}

	public void displayPicture(int cmd)
	{
		if (cmd == -1)
		{
			picDisplay.SetActive(false);
			return;
		}
		picDisplay.SetActive(true);
		imageDisplay.GetComponent<RawImage>().texture = displayImages[cmd];
		if (cmd == 15)
			imageDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(712, 712);
		else
			imageDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(644, 360);
	}
}
