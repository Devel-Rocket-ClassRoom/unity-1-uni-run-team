using UnityEngine;

public class ToggleChild : MonoBehaviour
{
    // 부모가 활성화될 때 자동으로 실행됨
    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true); // 자식들을 강제로 활성화
        }
    }
}