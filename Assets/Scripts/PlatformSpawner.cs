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
    private GameManager gameManager;

    private void Awake()
    {
        pool = new List<Platform>();
        activePlatforms = new List<Platform>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
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
            lastLocalX = -4.6f;
        }

        // 시작 전용 플랫폼 뒤에 초기 플랫폼 3개 더 이어 붙이기
        for (int i = 1; i < 4; i++)
        {
            SpawnNext(lastLocalX + 17.92f);
        }
    }

    void Update()
    {
        if (gameManager.IsGameOver)
            return;

        // 부모 오브젝트 이동 (플랫폼들은 자식이므로 함께 이동함)
        if (scrollScript != null)
        {
            movingParent.position += Vector3.left * scrollScript.Speed * Time.deltaTime;
        }

        if (movingParent.position.x <= -50f)
        {
            ResetOrigin();
        }

        // 가장 앞에 있는 플랫폼이 화면 밖으로 나갔는지 체크
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.x <= exitX)
        {
            RecyclePlatform();
        }
    }

    private void ResetOrigin() // 부모 오브젝트를 원점으로 순간이동시키고 자식 플랫폼들의 로컬 좌표로 배치
    {
        // 현재 부모 오브젝트의 오프셋 저장 (예: -100)
        float offset = movingParent.position.x;

        // 부모를 다시 월드 원점(0)으로 순간이동
        movingParent.position = Vector3.zero;

        // 자식들은 부모가 이동한 반대 방향으로 로컬 좌표를 밀어줌
        // 이렇게 해야 화면상에서는 플랫폼이 가만히 있는 것처럼 보입니다.
        foreach (var p in activePlatforms)
        {
            // 로컬 좌표에 -100을 더해줌 (즉, 100이었던 위치는 0이 됨)
            p.transform.localPosition += new Vector3(offset, 0, 0);
        }

        // 다음 생성 기준점인 lastLocalX도 똑같이 줄여줌
        lastLocalX += offset;

        Debug.Log("좌표 최적화 완료");
    }

    private void SpawnNext(float localX) // 로컬 X 좌표로 다음 플랫폼 배치
    {
        Platform p = GetRandomInactive(); 

        // position(월드)이 아니라 localPosition(로컬)을 사용해야 빈틈이 안 생김
        p.transform.localPosition = new Vector3(localX, -1.5f, 0);
        p.gameObject.SetActive(true);
        activePlatforms.Add(p);
        lastLocalX = localX; // 마지막 로컬 위치 갱신
    }

    private void RecyclePlatform()
    {
        Platform exited = activePlatforms[0];   // 화면 밖으로 나간 가장 앞에 있는 플랫폼
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