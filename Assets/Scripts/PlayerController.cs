using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrounded;
    private bool isSlided;
    private int jumpCount;
    [SerializeField]
    private float jumpForce = 8f;
    // 게임매니저 받기
    public GameManager gameManager;

    public float Health { get; private set; } = 100;
    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider;
    private Animator animator;

    void Awake()
    {
        isGrounded = true;
        isSlided = false;
        jumpCount = 0;    
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        // 게임오버시 입력처리 제한 (return처리);
        if (gameManager.IsGameOver) return;
        
        Health -= Time.deltaTime;

        // 플레이어 체력이 0이하로 내려갈시 OnPlayerDead호출
        if (Health <= 0)
        {
            gameManager.OnPlayerDead();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            if (isSlided)
            {
                isSlided = false;
                ColliderReturn();
            }
            jumpCount++;
            isGrounded = false;
            rigidbody.linearVelocity = Vector2.zero; // 점프할 때 기존의 속도를 초기화
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 점프할 때 위로 힘을 가함
        }
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            isSlided = true;
            ColliderSlide();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSlided = false;
            ColliderReturn();
        }

        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Slided", isSlided);

        // 디버그 메세지 
        Debug.Log($"Health: {Health}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && collision.contacts[0].normal.y > 0.7f)
        {
            jumpCount = 0;
            isGrounded = true;
            isSlided = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 장애물 -10hp 수정
        if (collision.gameObject.CompareTag("Damaged"))
        {
            Health -= 10f;
        }
        // 코인 +5hp 수정
        if (collision.gameObject.CompareTag("Coin"))
        {
            Health += 5f;
        }

        if (collision.gameObject.CompareTag("Dead"))
        {
            // Die()호출 추가
            Health = 0f;
            Die();
        }
    }

    private void ColliderSlide()
    {
        collider.offset = new Vector2(0, -0.5f);
        collider.size = new Vector2(0.85f, 0.8f);
    }
    private void ColliderReturn()
    {
        collider.offset = new Vector2(0, -0.1f);
        collider.size = new Vector2(0.85f, 1.5f);
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        // 게임매니저 PlayerDead 호출 (플레이어 사망)
        gameManager.OnPlayerDead();

        // 사망 처리 로직 (예: 애니메이션 재생, 게임 오버 화면 표시 등)
    }

    // Offset : (0, -0.1) -> (0, -0.5)
    // Size : (0.85, 1.5) -> (0.85, 0.8)
}
