using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip enemyAtkAudio;
    BossMove bossMove;
    WanderEnemy wanderEnemy;
    MidEnemy midEnemy;
    // Start is called before the first frame update
    void Start()
    {
        if ( gameObject.tag == "Enemy1" || gameObject.tag == "Enemy2" )
            wanderEnemy = GetComponent<WanderEnemy>();
        else if ( gameObject.tag == "MidEnemy" || gameObject.tag == "MidEnemyBoss" )
            midEnemy = GetComponent<MidEnemy>();
        else if ( gameObject.tag == "Boss" )
            bossMove = GetComponent<BossMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Enemy1" || gameObject.tag == "Enemy2")
        {
            if (wanderEnemy.isAttack)
                PlayAudio(enemyAtkAudio);
        }
        else if (gameObject.tag == "MidEnemy" || gameObject.tag == "MidEnemyBoss")
        {
            if (midEnemy.isAttack)
                PlayAudio(enemyAtkAudio);
        }
        else if (gameObject.tag == "Boss")
        {
            if (bossMove.isAttack)
                PlayAudio(enemyAtkAudio);
        }
    }
    void PlayAudio(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
}
