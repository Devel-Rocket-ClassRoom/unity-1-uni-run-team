using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float width = 17.92f; // 플랫폼의 가로 길이

    private void OnEnable()
    {
        // transform을 순회하면 직계 자식(Transform)들을 모두 가져올 수 있습니다.
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}