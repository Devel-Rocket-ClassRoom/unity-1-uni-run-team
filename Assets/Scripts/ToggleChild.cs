using UnityEngine;

public class ToggleChild : MonoBehaviour
{
    // 부모가 활성화될 때 자동으로 실행됨
    private void OnEnable() // 블록에 있는 코인을 다시 활성화
    {
        foreach (Transform child in transform) // 자식 오브젝트들을 강제로 활성화 (코인 활성화용)
        {
            child.gameObject.SetActive(true);
        }
    }
}