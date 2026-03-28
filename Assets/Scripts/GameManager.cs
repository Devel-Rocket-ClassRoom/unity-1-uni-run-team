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
        // // 게임오버 시 화면 씬 리셋
        // if (IsGameOver && Input.GetKeyDown(KeyCode.Space));
        // {
        //     // 이게 아직 잘 모르겠음;;
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // }
    }
}
