using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
[RequireComponent(typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    private float jumpForce = 6f; // Force applied when jumping

    private Rigidbody2D rb;
    private CapsuleCollider2D bc;
    private TapSwipeDetection gesture;
    private GroundCheck groundCheck;

    private bool isGrounded = false;
    private bool isSliding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
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
                Slide();
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

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force

    }

    private void Slide()
    {
        if (isGrounded && !isSliding)
        {
            StartCoroutine(SlideTime());
            Debug.Log("Sliding");
            bc.size = new Vector2(bc.size.x, bc.size.y / 2f); 
            isSliding = true; 
        }

        else if(!isGrounded && !isSliding)
        {
            StartCoroutine(SlideTime());

            rb.gravityScale = 10;
            bc.size = new Vector2(bc.size.x, bc.size.y / 2f); // Reduce collider size for sliding
            isSliding = true; // Set sliding state to true
        }
    }

    IEnumerator SlideTime()
    {
        yield return new WaitForSeconds(0.3f); // Duration of the slide
        rb.gravityScale = 1; // Reset gravity scale after sliding
        bc.size = new Vector2(bc.size.x, bc.size.y * 2f); // Reset collider size after sliding
        isSliding = false;
        Debug.Log("Slide ended");
    }
}
