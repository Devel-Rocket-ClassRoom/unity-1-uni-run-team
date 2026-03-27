using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 6f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime); // 방향 * 속력 * 시간 = 이동거리

        // 이동거리 / 시간 = 속도
        // 이동거리 = 속도 * 시간
    }
}
