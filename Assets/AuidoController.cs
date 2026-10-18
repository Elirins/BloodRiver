using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoController : MonoBehaviour
{
    public AudioClip playerPGAudioClip; // 玩家普攻的时候播放
    public AudioClip playerZCAudioClip; // 玩家中刺的时候播放
    public AudioClip playerXPAudioClip; // 玩家下劈的时候播放
    public AudioClip playerMoveAudioClip; // 移动的时候播放
    public float horizontalInput; 
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayAudio(playerPGAudioClip); // 调用PlayAudio时传入特定的音频文件
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayAudio(playerZCAudioClip); // 调用PlayAudio时传入特定的音频文件
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayAudio(playerXPAudioClip); // 调用PlayAudio时传入特定的音频文件
        }
        if (horizontalInput!= 0)
        {
            PlayAudio(playerMoveAudioClip); // 调用PlayAudio时传入特定的音频文件
        }
    }


    void PlayAudio(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
}
