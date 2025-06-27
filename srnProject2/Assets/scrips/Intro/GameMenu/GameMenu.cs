using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject menuList; // 菜单列表
    [SerializeField] private bool menuKeys = true;
    [SerializeField] private AudioSource bgmSound; // 背景音乐

    void Update()
    {
        if (menuKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKeys = false;
                Time.timeScale = 0; // 时间暂停
                bgmSound.Pause();   // 音乐暂停
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(false);
                menuKeys = true;
                Time.timeScale = 1; // 时间恢复正常了
                bgmSound.Play();   // 音乐播放
            }
        }
    }

    public void Return() // 返回游戏
    {
        menuList.SetActive(false);
        menuKeys = true;
        Time.timeScale = 1;
        bgmSound.Play();
    }

    public void Exit() // 退出游戏
    {
        Application.Quit();
    }
}