//挂在start的canvas实现开始界面
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Setup;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);//初始加载出场景设为1，开始菜单场景为0，在file的build setting里面设置

    }
    public void QuitGame()
    {
        Application.Quit();//游戏终止
    }
    public void OnSetupHandler()
    {
        Setup.SetActive(true);//游戏设置界面
    }
}
