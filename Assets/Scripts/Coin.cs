using UnityEngine;

public class Coin : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    public GameManager gameManager;


    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 디버그 메세지
            Debug.Log("코인 획득!");
            gameObject.SetActive(false); // 코인 오브젝트 제거
            // 게임 매니저에서 점수 갱신
            gameManager.AddScore();

        }
    }
}

// 8.96