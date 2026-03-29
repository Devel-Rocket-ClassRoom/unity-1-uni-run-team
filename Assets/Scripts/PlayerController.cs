using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrounded;
    private bool isSlided;
    private int jumpCount;

    [SerializeField]
    private float jumpForce = 8f;

    public float Health { get; private set; } = 100;
    private float hitTime = 1.5f;                       // 히트 타임 초기값을 1.5초로 설정 (처음에는 피해를 받을 수 있는 상태)
    private float timer = 0f;                           // 무적 시간 타이머 (렌더링 깜빡임용)

    private GameManager gameManager;
    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider;
    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        // 게임오버시 입력처리 제한 (return처리)
        if (gameManager.IsGameOver) return;
        
        Health -= Time.deltaTime * 3;
        hitTime += Time.deltaTime;

        // 플레이어 체력이 0이하로 내려갈시 Die호출
        if (Health <= 0)
        {
            Die();
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

        
        if (hitTime < 1.5f) // 무적 구간에는 캐릭터 깜빡임
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
            spriteRenderer.enabled = true;
        }

        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Slided", isSlided);
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
        // 장애물 -30hp 수정
        if (collision.gameObject.CompareTag("Damaged"))
        {
            if (hitTime >= 1.5f)
            {
                Health -= 30f;
                hitTime = 0f; // 히트 타임 초기화
            }
        }
        // 코인 +0.5hp 수정
        if (collision.gameObject.CompareTag("Coin"))
        {
            Health += 0.5f;
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
    }

    // Offset : (0, -0.1) -> (0, -0.5)
    // Size : (0.85, 1.5) -> (0.85, 0.8)
}
