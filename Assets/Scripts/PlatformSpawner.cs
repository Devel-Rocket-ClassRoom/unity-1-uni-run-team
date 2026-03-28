using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platforms;

    private List<GameObject> objectPool = new List<GameObject>();

    // 시작할 때, x좌표 13.46에 랜덤한 플랫폼을 생성

    void Start()
    {
        Platform.activeFalse += SpawnRandom; // FlatForm에서 activeFalse 이벤트가 발생할 때 SpawnRandom 메서드를 호출하도록 구독
        foreach (GameObject platform in platforms)
        {
            GameObject obj = Instantiate(platform);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool.Add(obj); // 비활성화된 플랫폼을 오브젝트 풀에 추가
        }

    }

    private void SpawnRandom(float positionX)
    {
        List<GameObject> inactiveList = objectPool.FindAll(obj => !obj.activeSelf); // 비활성화된 플랫폼들만 필터링

        if (inactiveList.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveList.Count);
            GameObject selected = inactiveList[randomIndex];

            selected.transform.position = new Vector3(13.3f , -1.5f, 0);

            selected.SetActive(true);
        }
    }
 
    // 7개를 일단 비활성화된 상태로 생성
    // 13.46에 랜덤한 플랫폼을 활성화(비활성화된 애들중에 랜덤으로 뽑기)
    // 화면밖으로 나간 플랫폼은 비활성화
}
