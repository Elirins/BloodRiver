using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CloseButtonController : MonoBehaviour
{
    //the ButtonPauseMenu
    public GameObject ingameMenu;

    public void OnResume()//点击“回到游戏”时执行此方法
    {
        Time.timeScale = 1f;
        ingameMenu.SetActive(false);
    }


}