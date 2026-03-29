using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private float speed = 8f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime); // 방향 * 속력 * 시간 = 이동거리
        }

        // 이동거리 / 시간 = 속도
        // 이동거리 = 속도 * 시간
    }
}
