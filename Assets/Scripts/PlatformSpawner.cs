using System.Collections.Generic;
using UnityEngine;

// 부모 오브젝트가 실제로 이동하고 자식 플랫폼들이 로컬 좌표로 배치되는 구조
public class PlatformSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject startPlatformPrefab; // 처음에만 나올 시작 플랫폼
    [SerializeField] private GameObject[] prefabs;           // 일반 플랫폼들

    [Header("References")]
    [SerializeField] private Transform movingParent;        // 플랫폼들이 자식으로 붙을 실제 이동하는 부모 오브젝트
    [SerializeField] private ScrollingObject scrollScript;  // 속도 동기화용 (배경의 스크립트 연결)
    [SerializeField] private float exitX = -25f;            // 충분히 화면 밖으로 나가는 지점

    private List<Platform> pool;                            // 플랫폼 풀
    private List<Platform> activePlatforms;                 // 활성화된 플랫폼들
    private float lastLocalX = 0f;                          // 마지막으로 배치된 플랫폼의 로컬 X 위치
    private Platform startPlatformInstance;                 // 시작 플랫폼 인스턴스 (재사용 안 함)

    private void Awake()
    {
        pool = new List<Platform>();
        activePlatforms = new List<Platform>();
    }

    void Start()
    {
        // 오브젝트 풀 생성
        foreach (var prefab in prefabs)
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject obj = Instantiate(prefab, movingParent); // 부모 오브젝트의 자식으로 생성
                obj.SetActive(false);
                pool.Add(obj.GetComponent<Platform>());
            }
        }

        // 시작 전용 플랫폼 배치
        if (startPlatformPrefab != null)
        {
            GameObject startObj = Instantiate(startPlatformPrefab, movingParent);
            startPlatformInstance = startObj.GetComponent<Platform>();
            startPlatformInstance.transform.localPosition = new Vector3(-4.6f, -1.5f, 0);
            activePlatforms.Add(startPlatformInstance);
            lastLocalX = 0;
        }

        // 시작 전용 플랫폼 뒤에 초기 플랫폼 3개 더 이어 붙이기
        for (int i = 1; i < 4; i++)
        {
            SpawnNext(lastLocalX + 17.92f);
        }
    }

    void Update()
    {
        // 부모 오브젝트 이동 (플랫폼들은 자식이므로 함께 이동함)
        if (scrollScript != null)
        {
            movingParent.position += Vector3.left * scrollScript.Speed * Time.deltaTime;
        }

        // 가장 앞에 있는 플랫폼이 화면 밖으로 나갔는지 체크
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.x <= exitX)
        {
            RecyclePlatform();
        }
    }

    private void SpawnNext(float localX) // 로컬 X 좌표로 다음 플랫폼 배치
    {
        Platform p = GetRandomInactive(); 
        if (p == null)
        {
            Debug.LogError("비활성화된 플랫폼이 없습니다! 풀 개수를 늘리세요.");
            return;
        }

        // [중요] .position(월드)이 아니라 .localPosition(로컬)을 사용해야 빈틈이 안 생깁니다.
        p.transform.localPosition = new Vector3(localX, -1.5f, 0);
        p.gameObject.SetActive(true);
        activePlatforms.Add(p);
        lastLocalX = localX; // 마지막 로컬 위치 갱신
    }

    private void RecyclePlatform()
    {
        Platform exited = activePlatforms[0];   // 가장 앞에 있는 플랫폼이 화면 밖으로 나갔으므로 재활용
        activePlatforms.RemoveAt(0);            // 활성화된 리스트에서 제거

        if (exited == startPlatformInstance)
        {
            Destroy(exited.gameObject); // 시작 플랫폼은 재사용 안 함
        }
        else
        {
            exited.gameObject.SetActive(false); // 비활성화해서 풀로 반환
        }

        SpawnNext(lastLocalX + 17.92f);
    }

    private Platform GetRandomInactive() // 풀에서 비활성화된 플랫폼 중 랜덤으로 하나 가져오기
    {
        var inactive = pool.FindAll(obj => !obj.gameObject.activeSelf); 
        if (inactive.Count == 0) return null;                           
        return inactive[Random.Range(0, inactive.Count)];               
    }
}