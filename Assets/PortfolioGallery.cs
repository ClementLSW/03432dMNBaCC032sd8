using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PortfolioGallery : MonoBehaviour {

    public ScrollRect portfolioScrollRect;
    
    public enum GALLERY_MODE
    {
        AUTO,
        MANUAL,
        MAX_MODE
    }

    GALLERY_MODE mode = GALLERY_MODE.AUTO;

    float modeTimer = 0;    // timer

    bool autoStop = false;

    float stopTimer = 0;    // timer

    public float stopDuration = 3;  // how long the auto scroll stops at an image for

    Vector3 portfolioGalleryOriginalPos;    // store variable so we can snap it back to start when auto mode

    Vector3 initialTouchPosition;   // variable to store a temp vector

    public MainScript mainScript;   // reference to main script. need to get whether user is at portfolio page

    byte nextPortfolioImageIndex = 0;
    byte currentPortfolioImageIndex = 0;
    public float portfolioGallerySwipeSpeed = 2000;

    bool swipeStartedFromArea = false;

    public float manualSwipeSpeed = 2000;

    // Use this for initialization
    void Start () {
        portfolioGalleryOriginalPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        // Do not update this script if user is not at portfolio page
        if(!mainScript.isPortfolio)
        {
            return;
        }

        GetMouseInput();

        GetTouchInput();

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
                    modeTimer = 0;
                }
            }

        }
        else
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration)
            {
                if (nextPortfolioImageIndex + 1 > 4)
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

    public void GetTouchInput()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                // Check if in swipe area, if it isnt, ignore the touch
                swipeStartedFromArea = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetChild(i).GetComponent<RectTransform>(), Input.mousePosition))
                    {
                        swipeStartedFromArea = true;
                        break;
                    }
                }

                if (swipeStartedFromArea)
                {
                    //save began touch 2d point
                    initialTouchPosition = new Vector2(t.position.x, t.position.y);
                    //portfolioScrollRect.vertical = false;
                    portfolioScrollRect.velocity = Vector2.zero;
                    mode = GALLERY_MODE.MANUAL;
                }
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
    }

    public void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartedFromArea = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetChild(i).GetComponent<RectTransform>(), Input.mousePosition))
                {
                    swipeStartedFromArea = true;
                    break;
                }
            }

            if (swipeStartedFromArea)
            {
                portfolioScrollRect.velocity = Vector3.zero;
                //save began touch 2d point
                initialTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                mode = GALLERY_MODE.MANUAL;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Do nothing if swipe did not start from swipe area
            if(!swipeStartedFromArea)
            {
                return;
            }

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

    IEnumerator Swipe(byte currentImageIndex)
    {
        modeTimer = 0;
        stopTimer = 0;
        
        Vector3 targetPos = portfolioGalleryOriginalPos;

        float speedToMoveAt = portfolioGallerySwipeSpeed;

        // Swipe left
        if (nextPortfolioImageIndex > currentImageIndex)
        {
            //targetPos -= Vector3.right * 720f;
            targetPos = new Vector3((-nextPortfolioImageIndex * 720f) + portfolioGalleryOriginalPos.x, transform.localPosition.y, portfolioGalleryOriginalPos.z);
        }
        else if (nextPortfolioImageIndex < currentImageIndex)// Swipe right
        {
            //targetPos += Vector3.right * 720f;
            targetPos = new Vector3((-nextPortfolioImageIndex * 720f) + portfolioGalleryOriginalPos.x, transform.localPosition.y, portfolioGalleryOriginalPos.z);
        }

        if (mode == GALLERY_MODE.AUTO && currentImageIndex == 4 && nextPortfolioImageIndex == 0)
        {
            targetPos = portfolioGalleryOriginalPos;
            speedToMoveAt = 5000;
        }
        else if(mode == GALLERY_MODE.MANUAL)// if manually swipe, use manual speed
        {
            speedToMoveAt = manualSwipeSpeed;
        }

        float distance = 720;

        GALLERY_MODE modeThatStartedSwipe = mode;

        while (distance > 1)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speedToMoveAt * Time.deltaTime);

            distance = Vector3.Distance(transform.localPosition, targetPos);

            if (modeThatStartedSwipe != mode)
            {
                break;
            }

            yield return null;
        }
    }
}
