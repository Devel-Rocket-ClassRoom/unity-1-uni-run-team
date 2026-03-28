using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static event Action<float> activeFalse;

    private void OnEnable()
    {
        // transform을 순회하면 직계 자식(Transform)들을 모두 가져올 수 있습니다.
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }


    void Update()
    {
        if (gameObject.transform.position.x <= -13.46f)
        {
            gameObject.SetActive(false);
            activeFalse?.Invoke(gameObject.transform.position.x);
        }
    }

    
}
