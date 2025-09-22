using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
[RequireComponent(typeof(GroundCheck), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    private float jumpForce = 7f; // Force applied when jumping

    private Rigidbody2D rb;
    private CapsuleCollider2D bc;
    private TapSwipeDetection gesture;
    private GroundCheck groundCheck;
    private Animator anim;

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip slideClip;
    [SerializeField] private AudioClip CoinClip;
    [SerializeField] private AudioClip HitClip;


    private bool isGrounded = false;
    private bool isSliding = false;
    private bool isHit = false;

    private Vector2 originalColliderSize;
    private float originalGravityScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
        gesture = FindAnyObjectByType<TapSwipeDetection>();
        groundCheck = GetComponent<GroundCheck>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        originalColliderSize = bc.size;
        originalGravityScale = rb.gravityScale;

        gesture.OnTap += HandleTap;
        gesture.OnDoubleTap += HandleDoubleTap;
        gesture.OnSwipe += HandleSwipe;
    }
    private void Update()
    {
        // Check if the player is grounded
        isGrounded = groundCheck.IsGrounded();

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSliding", isSliding);
        anim.SetBool("IsHit", isHit);
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
                CancelSlide();
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

        audioSource.PlayOneShot(jumpClip);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force


    }

    private void Slide()
    {
        if (isSliding) return;

        StartCoroutine(SlideTime());

        audioSource.PlayOneShot(slideClip);

        bc.size = new Vector2(bc.size.x, bc.size.y / 2f); // Reduce collider height for sliding
        rb.gravityScale = isGrounded ? originalGravityScale : 10f;
        isSliding = true;
    }
    private void CancelSlide()
    {
        if (isGrounded && isSliding)
        {
            isSliding = false;
            Debug.Log("Slide cancelled");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply jump force
        }
        else return;
    }

    IEnumerator SlideTime()
    {
        yield return new WaitForSeconds(0.5f); // Duration of the slide
        rb.gravityScale = originalGravityScale;
        bc.size = originalColliderSize; // Reset collider size
        isSliding = false;
       
    }

    IEnumerator HandleHit()
    {
        isHit = true;
        audioSource.PlayOneShot(HitClip);

        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.PlayerHit();
        isHit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(HandleHit());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            ScoreManager.instance.AddDistance(10); // Add 10 points for each pickup
            audioSource.PlayOneShot(CoinClip);
        }
    }

}
