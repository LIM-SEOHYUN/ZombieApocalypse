using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 변수
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // 현재의 게임 상태 변수
    public GameState gState;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // PlayerMove 클래스 변수
    PlayerMove player;

    // 옵션 화면 UI 오브젝트 변수
    public GameObject gameOption;

    public int enemyCount;

    void Start()
    {
        // 초기 게임 상태는 준비 상태로 설정한다.
        gState = GameState.Ready;

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다.
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 내용을 "Ready..."로 한다.
        gameText.text = "Ready";

        // 상태 텍스트의 색상을 주황색으로 한다.
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());

        // 플레이어 오브젝트를 찾은 뒤, 플레이어의 PlayerMove 컴포넌트 받아오기
        player = GameObject.Find("Soldier").GetComponent<PlayerMove>();
        EnemyCount();
}

    IEnumerator ReadyToStart()
    {
        // 2초간 대기한다.
        yield return new WaitForSeconds(2f);

        // 상태 텍스트의 내용을 "Go!"로 한다.
        gameText.text = "START";

        // 0.5초간 대기한다.
        yield return new WaitForSeconds(0.5f);

        // 상태 텍스트를 비활성화한다.
        gameLabel.SetActive(false);

        // 상태를 "게임 중" 상태로 변경한다.
        gState = GameState.Run;
    }
    void Update()
    {
        enemyCount = EnemyCount();
        Debug.Log("남은 적군의 수" + enemyCount);
        if (player.hp <= 0 || enemyCount == 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true);
            gameText.text = "Game Over";
            gameText.color = new Color32(255, 0, 0, 255);
            Transform buttons = gameText.transform.GetChild(0);
            buttons.gameObject.SetActive(true);
            gState = GameState.GameOver;
        }
    }

    public void OpenOptionWindow()
    {
        gameOption.SetActive(true);
        Time.timeScale = 0f;
        gState = GameState.Pause;
    }

    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gState = GameState.Run;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    int EnemyCount()
    {
        EnemyFSM[] enemies = FindObjectsOfType<EnemyFSM>();
        return enemies.Length;
    }
}