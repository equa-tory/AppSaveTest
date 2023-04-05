using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistance
{

    private float horizontal;

    public float speed;
    public float jumpForce;

    private Rigidbody2D rb;
    [SerializeField] private Transform gCheck;
    [SerializeField] private LayerMask gMask;

    private bool isFacingRight = true;

    InputManager input;


    void Start()
    {
        input = InputManager.Instance;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Flip();
        horizontal = input.movementInput.x;

        if (input.jump_Input && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(gCheck.position, 0.2f, gMask);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Die()
    {
        GameManager.Instance.AddDeath();
        Destroy(gameObject);
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
