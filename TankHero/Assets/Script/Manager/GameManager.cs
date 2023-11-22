using System.Collections;
using TMPro;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    // 태양 & 시간 변화
    public GameObject sun;
    public float lateTime;

    // UI
    public GameObject gameOverUi;
    public GameObject settingUi;
    public GameObject clear;
    
    // 점수
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxOverScore;
    private int _score;
    private bool _isHide;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
        }

        _isHide = false;
        _score = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _isHide = !_isHide;
            Cursor.visible = _isHide;
            settingUi.SetActive(_isHide);
        }
        
        Time.timeScale = _isHide ? 0f : 1f;
        
        sun.transform.Rotate(new Vector3(50, 0,0) * (Time.deltaTime * lateTime));
        scoreText.text = "인민군 처치  : " + _score;

        if (sun.transform.eulerAngles.y > 360) {
            SoundManager.Instance.MoveStop();
            Time.timeScale = 0f;
            clear.SetActive(true);
        }
    }

    public void SettingHide()
    {
        _isHide = false;
        Cursor.visible = _isHide;
        settingUi.SetActive(_isHide);
    }

    public void GetScore()
    {
        _score++;
    }

    public IEnumerator GameEnd()
    {
        SoundManager.Instance.MoveStop();
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        _isHide = true;
        gameOverUi.SetActive(true);
        maxOverScore.text = "최종 점수 : " + _score;
    }
}
