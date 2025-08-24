using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    private float jumpForce = 7f; // Force applied when jumping

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private TapSwipeDetection gesture;
    private GroundCheck groundCheck;
    private bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        gesture = FindAnyObjectByType<TapSwipeDetection>();
        groundCheck = GetComponent<GroundCheck>();

        gesture.OnTap += HandleTap;
        gesture.OnDoubleTap += HandleDoubleTap;
        gesture.OnSwipe += HandleSwipe;
    }
    private void Update()
    {
        // Check if the player is grounded
        isGrounded = groundCheck.IsGrounded();
    }

    private void OnDestroy()
    {
        if (gesture != null)
        {
            gesture.OnTap -= HandleTap;
            gesture.OnDoubleTap -= HandleDoubleTap;
            gesture.OnSwipe -= HandleSwipe;
        }
    }

    private void HandleTap(Vector2 position)
    {
        Debug.Log($"Tap detected at position: {position}");
    }

    private void HandleDoubleTap(Vector2 position)
    {
        Debug.Log($"Double Tap detected at position: {position}");

    }

    private void HandleSwipe(TapSwipeDetection.SwipeDirection direction)
    {
        Debug.Log($"Swipe detected in direction: {direction}");

        switch (direction)
        {
            case TapSwipeDetection.SwipeDirection.Up:
                Jump();
                break;
            case TapSwipeDetection.SwipeDirection.Down:
                // Handle down swipe if needed
                break;
            case TapSwipeDetection.SwipeDirection.Left:
                break;
            case TapSwipeDetection.SwipeDirection.Right:
                break;
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;

        Debug.Log("Jumping!");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force

    }
}
