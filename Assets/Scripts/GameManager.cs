using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 점수
    public TextMeshProUGUI scoreText;
    // 게임 오버 UI
    public GameObject gameOverUi;

    private int score = 0;
    public bool IsGameOver {get; private set;} 

    public void Awake()
    {
        gameOverUi.SetActive(false);
    }
   

    // Update is called once per frame
    void Update()
    {
        // 게임오버 시 화면 씬 리셋
        if (IsGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // 이게 아직 잘 모르겠음;;
            // -> Assets/Scenes 경로에 여러 씬들을 저장할 수 있는데 아래 코드는 지금 활성화된 씬의 이름을 가져와서 다시 로드하는 방식임
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore()
    {
        score++;
        scoreText.text = $"SCORE : {score}";
    }

    // 플레이어 사망 메서드
    public void OnPlayerDead()
    {   
        // IsGameOver bool 처리
        IsGameOver = true;
        // 게임오버 UI 호출
        gameOverUi.SetActive(true);
    }
}
