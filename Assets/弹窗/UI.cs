//挂在text的UI上，实现游戏内菜单
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject Setup;
    public GameObject menu;
    public Button setup;
    public Button close;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void PlayGame()
    {
        SceneManager.LoadScene(0);//回到初始界面0

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//esc打开游戏中菜单
        {
            ResumeGame();
            menu.SetActive(true);
        }
    }
    public void clikSetupButton()//打开设置菜单
    {
        
            Setup.SetActive(true);
        
    }
    public void OnCloseHandler()//关闭菜单
    {
        menu.SetActive(false);
    }
    public void ResumeGame()//时间恢复
    {
        Time.timeScale = 1f;
    }
    public void PauseGame()//时间停止
    {
        Time.timeScale = 0f;
    }
}
