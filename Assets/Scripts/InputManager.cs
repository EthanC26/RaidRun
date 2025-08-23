using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameActions gameActions;
    Camera maincamera;

    public event System.Action OnTouchStarted;
    public event System.Action OnTouchEnded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maincamera = Camera.main;
    }

    private void OnEnable()
    {
        gameActions = new GameActions();
        gameActions.Enable();

        gameActions.Gameplay.Touch.started += ctx => OnTouchStarted?.Invoke();
        gameActions.Gameplay.Touch.canceled += ctx => OnTouchEnded?.Invoke();
    }

    private void OnDisable()
    {
        gameActions.Gameplay.Touch.started -= ctx => OnTouchStarted?.Invoke();
        gameActions.Gameplay.Touch.canceled -= ctx => OnTouchEnded?.Invoke();
        gameActions.Disable();
        
    }

    public Vector2 PrimaryPosition()
    {
        Vector2 touchPosition = gameActions.Gameplay.PrimaryPosition.ReadValue<Vector2>();
        if (!maincamera)
        {
            maincamera = Camera.main;
            if(!maincamera)
                Debug.LogError("Main camera not found. Please ensure a camera is tagged as " +
            "'MainCamera' in the scene.");
            
        }
        return maincamera ? maincamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y,
            maincamera.nearClipPlane)) : Vector2.zero;
    }
}
