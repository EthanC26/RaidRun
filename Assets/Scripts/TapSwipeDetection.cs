using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class TapSwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    //veribales for tap and swipe detection
    public float distanceThreshold = 0.1f; // Distance in units to consider a tap
    public float directionThershold = 0.9f; // Distance in units to consider a swipe direction
    public float tapTimeOut = 0.2f; // Maximum time to wait for a tap
    public float swipeTimeOut = 0.5f; // Maximum time to wait for a swipe

    //variables to store the state of the tap/swipe
    private float StartTime;
    private float EndTime;
    private Vector2 startPosition;
    private Vector2 endPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.OnTouchStarted += HandleTouchStarted;
        inputManager.OnTouchEnded += HandleTouchEnded;
    }

    private void OnDestroy()
    {
        inputManager.OnTouchStarted -= HandleTouchStarted;
        inputManager.OnTouchEnded -= HandleTouchEnded;
    }

    private void HandleTouchStarted()
    {
        StartTime = Time.time;
        startPosition = inputManager.PrimaryPosition();
    }
    private void HandleTouchEnded()
    {
        EndTime = Time.time;
        endPosition = inputManager.PrimaryPosition();
        DetectInput();
    }

    private void DetectInput()
    {
        float TimeDifference = EndTime - StartTime;
        float distance = Vector2.Distance(startPosition, endPosition);
        if (TimeDifference > swipeTimeOut)
        {
            Debug.Log("Swipe timeout exceeded");
            return;
        }
        if(distance >= directionThershold)
           if(CheckSwipe()) return;

        if (TimeDifference < tapTimeOut && distance < distanceThreshold)
        {
            TapDetected();
        }
    }

    private bool CheckSwipe()
    {
        Vector2 dir = (endPosition - startPosition).normalized;
        float checkUp = Vector2.Dot(dir, Vector2.up);
        float checkRight = Vector2.Dot(dir, Vector2.right);

        if(checkUp >= directionThershold)
        {
            Debug.Log("Swipe Up Detected");
            return true;
        }
        if (checkUp <= -directionThershold)
        {
            Debug.Log("Swipe Down Detected");
            return true;
        }
        if (checkRight >= directionThershold)
        {
            Debug.Log("Swipe Right Detected");
            return true;
        }
        if (checkRight <= -directionThershold)
        {
            Debug.Log("Swipe Left Detected");
            return true;
        }

        return false;

    }

    private void TapDetected()
    {
        Debug.Log("Tap Detected at: " + startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
