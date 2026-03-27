using UnityEngine;

public class Coin : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("코인 획득!");
            gameObject.SetActive(false); // 코인 오브젝트 제거
        }
    }
}

// 8.96