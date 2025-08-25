using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField, Range(0.0f, 0.1f)] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private LayerMask IsGroundLayer; // Layer mask to identify ground objects
    private Transform groundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject newGameObject = new GameObject();
        newGameObject.transform.SetParent(transform);
        float yOffSet = -GetComponent<CapsuleCollider2D>().size.y / 2f - groundCheckRadius;
        newGameObject.transform.localPosition = new Vector3(0,yOffSet,0);
        newGameObject.name = "GroundCheck";
        groundCheck = newGameObject.transform;
    }

    public bool IsGrounded()
    {
        if(!groundCheck) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, IsGroundLayer);

    }
}
