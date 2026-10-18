using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 注释：你正解决击退和拼刀判定问题
public class Boss1SwordState : MonoBehaviour
{
    private float RepellingForce = 40f;
    private bool isTrig = false;
    private BossMove bossMove;
    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        bossMove = transform.parent.GetComponent<BossMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTrig && other.tag == "Sword")
        {
            isTrig = true;
            playerMove = other.gameObject.transform.parent.GetComponent<PlayerMove>();


            // 前后为：克制造成伤害，拼刀无事发生，被克制扣血
            if (bossMove.isNormalAttack && playerMove.isDownwardKick)
            {
                playerMove.playerHP -= 20;
            }
            else if (bossMove.isMidAttack && playerMove.isNormalAttack)
            {
                playerMove.playerHP -= 30;
            }
            else if (bossMove.isDownwardKick && playerMove.isMidAttack)
            {
                playerMove.playerHP -= 40;
            }
            else
            {
                if (playerMove.isNormalAttack)
                    bossMove.enemyHP -= 20;
                else if (playerMove.isMidAttack)
                    bossMove.enemyHP -= 30;
                else if (playerMove.isDownwardKick)
                    bossMove.enemyHP -= 40;
            }
            return;
        }

        if (!isTrig && other.tag == "Player")
        {
            isTrig = true;
            playerMove = other.gameObject.GetComponent<PlayerMove>();

            if (bossMove.isNormalAttack)
            {
                playerMove.playerHP -= 20;
            }
            else if (bossMove.isMidAttack)
            {
                playerMove.playerHP -= 30;
            }
            else if (bossMove.isDownwardKick)
            {
                playerMove.playerHP -= 40;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        isTrig = false;
        //playerMove.isHitSword = false;
        //playerMove.isHitPlayer = false;
    }
}

