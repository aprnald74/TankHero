using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public enum ButtonType
{
    Start,
    End,
    Main,
    Reload,
    SettingHide,
    SoundSettingShow,
    SoundSettingHide,
    Explanation
    
}

public class ButtonManager : MonoBehaviour
{
    public ButtonType CurrentType;

    public void OnBtnClick()
    {
        switch (CurrentType)
        {
            case ButtonType.Start:
                Time.timeScale = 1f;
                SceneManager.LoadScene(1);
                break;
            
            case ButtonType.Main:
                SceneManager.LoadScene(0);
                break;
            
            case ButtonType.Reload:
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            
            case ButtonType.SoundSettingShow:
                SoundManager.Instance.Show();
                break;
            
            case ButtonType.SoundSettingHide:
                SoundManager.Instance.Hide();
                break;
            
            case ButtonType.SettingHide:
                GameManager.Instance.SettingHide();
                break;
            
            case ButtonType.Explanation:
                SceneManager.LoadScene(2);
                break;
            
            case ButtonType.End:
                //EditorApplication.isPlaying = false;
                Application.Quit();
                break;
            
        }
    }
}
