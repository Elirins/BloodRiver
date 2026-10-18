//挂在setup上实现设置界面
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Setup : MonoBehaviour
{
    //public Slider volumeSlider;
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    public void OnCloseHandler()
    {
        gameObject.SetActive(false);//关闭
    }
    public void SetVolum(float value)
    {
        audioMixer.SetFloat("bgm",value);//调节音量
    }

}
