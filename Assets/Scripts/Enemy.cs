using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D enemyRb;
    CapsuleCollider2D enemyCc;
    Animator animator;
    [SerializeField] float moveSpeed;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float checkDistance;

    [SerializeField] Transform lookPosition;
    [SerializeField] Transform groundCheckPosition;

    [SerializeField] bool isGrounded;
    public bool isDead = false;
    [SerializeField] float jumpForce;


   

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyCc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("explosion"))
        {
            hitEnemy();
        }
    }

    public void hitEnemy()
    {
        enemyCc.enabled = false;
        animator.SetBool("isDead", true);
        isDead = true;
        enemyRb.gravityScale = 4;
        enemyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        StartCoroutine(KillEnemy());
    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(lookPosition.position, Vector2.down, checkDistance, groundLayer);
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, checkDistance / 2, groundLayer);



        if (hit.collider == null && isGrounded)
        {
            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
        }

        if (groundHit.collider != null)
        {
            isGrounded = true;
        }else
        {
            isGrounded = false;
        }

    }

    private void FixedUpdate()
    {
        moveEnemy();
    }

    void moveEnemy()
    {
        if (!isDead)
        {
            enemyRb.linearVelocityX = transform.right.x * moveSpeed;
        }
    }



    
}
