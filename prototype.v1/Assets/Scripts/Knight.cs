using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.6f;
    public DetectionZone attachZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    public enum WalkableDirection
    {
        Left,
        Right
    }

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 walkDirectionVector = Vector2.right;
    public WalkableDirection walkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                // Dùng Vector3 thay vì Vector2 để không làm mất trục Z của nhân vật
                Vector3 localScale = gameObject.transform.localScale;

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                    // Ép trục X thành số dương -> Quay mặt sang phải
                    localScale.x = Mathf.Abs(localScale.x);
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                    // Ép trục X thành số âm -> Quay mặt sang trái
                    localScale.x = -Mathf.Abs(localScale.x);
                }

                // Gán lại scale mới cho nhân vật
                gameObject.transform.localScale = localScale;
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } private set 
        { 
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HasTarget = attachZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        if (CanMove)
            rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
        else 
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        if (walkDirection == WalkableDirection.Left)
        {
            walkDirection = WalkableDirection.Right;
        }
        else if (walkDirection == WalkableDirection.Right)
        {
            walkDirection = WalkableDirection.Left;
        }
        else
        {
            Debug.LogError("Unexpected walk direction: " + walkDirection);
        }
    }



    // Update is called once per frame

}
