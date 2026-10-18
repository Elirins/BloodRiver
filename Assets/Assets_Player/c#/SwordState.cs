using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 注释：你正解决击退和拼刀判定问题
public class SwordState : MonoBehaviour
{
    private float RepellingForce = 40f;
    private bool isTrig = false;
    private PlayerMove playerMove;
    private WanderEnemy wanderEnemy;
    private MidEnemy midEnemy;
    private BossMove bossMove;
    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = transform.parent.GetComponent<PlayerMove>() ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerMove.isAttack)
        {
            if (!isTrig && other.tag == "Sword")
            {
                isTrig = true;
                if (other.transform.parent.gameObject.tag == "Enemy1"
                 || other.transform.parent.gameObject.tag == "Enemy2")
                {
                    wanderEnemy = other.gameObject.transform.parent.GetComponent<WanderEnemy>();
                    playerRigidbody = playerMove.gameObject.GetComponent<Rigidbody>();
                    // 前后为：克制造成伤害，拼刀被击退，被克制扣血
                    if (playerMove.isNormalAttack && wanderEnemy.isDownwardKick)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        wanderEnemy.enemyHP -= playerMove.atk;

                    }
                    else if (playerMove.isMidAttack && wanderEnemy.isNormalAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        wanderEnemy.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isDownwardKick && wanderEnemy.isMidAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        wanderEnemy.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isNormalAttack && wanderEnemy.isNormalAttack ||
                              playerMove.isMidAttack && wanderEnemy.isMidAttack ||
                              playerMove.isDownwardKick && wanderEnemy.isDownwardKick)
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                    else
                    {
                        if (wanderEnemy.isNormalAttack)
                            playerMove.playerHP -= wanderEnemy.atk;
                        else if (wanderEnemy.isMidAttack)
                            playerMove.playerHP -= wanderEnemy.atk;
                        else if (wanderEnemy.isDownwardKick)
                            playerMove.playerHP -= wanderEnemy.atk;
                    }
                    return;
                }
                else if (other.transform.parent.gameObject.tag == "Boss")
                {
                    bossMove = other.gameObject.transform.parent.GetComponent<BossMove>();
                    playerRigidbody = playerMove.gameObject.GetComponent<Rigidbody>();
                    // 前后为：拼刀被击退，被克制扣血
                    if (playerMove.isNormalAttack && bossMove.isDownwardKick)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        bossMove.enemyHP -= playerMove.atk;

                    }
                    else if (playerMove.isMidAttack && bossMove.isNormalAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        bossMove.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isDownwardKick && bossMove.isMidAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        bossMove.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isNormalAttack && bossMove.isNormalAttack ||
                              playerMove.isMidAttack && bossMove.isMidAttack ||
                              playerMove.isDownwardKick && bossMove.isDownwardKick)
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                    else
                    {
                        if (bossMove.isNormalAttack)
                            playerMove.playerHP -= 20;
                        else if (bossMove.isMidAttack)
                            playerMove.playerHP -= 30;
                        else if (bossMove.isDownwardKick)
                            playerMove.playerHP -= 40;
                    }
                    return;
                }
                else if (other.transform.parent.gameObject.tag == "MidEnemy"
                      || other.transform.parent.gameObject.tag == "MidEnemyBoss")
                {
                    midEnemy = other.gameObject.transform.parent.GetComponent<MidEnemy>();
                    playerRigidbody = playerMove.gameObject.GetComponent<Rigidbody>();
                    // 前后为：克制造成伤害，拼刀被击退，被克制扣血
                    if (playerMove.isNormalAttack && midEnemy.isDownwardKick)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        midEnemy.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isMidAttack && midEnemy.isNormalAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        midEnemy.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isDownwardKick && midEnemy.isMidAttack)
                    {
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                        midEnemy.enemyHP -= playerMove.atk;
                    }
                    else if (playerMove.isNormalAttack && midEnemy.isNormalAttack ||
                              playerMove.isMidAttack && midEnemy.isMidAttack ||
                              playerMove.isDownwardKick && midEnemy.isDownwardKick)
                        playerRigidbody.AddForce(Vector2.left * RepellingForce * playerMove.faceDirection, ForceMode.Impulse);
                    else
                    {
                        if (midEnemy.isNormalAttack)
                            playerMove.playerHP -= midEnemy.atk;
                        else if (midEnemy.isMidAttack)
                            playerMove.playerHP -= midEnemy.atk;
                        else if (midEnemy.isDownwardKick)
                            playerMove.playerHP -= midEnemy.atk;
                    }
                    return;
                }
            }

            if (!isTrig && (other.tag == "Enemy1" || other.tag == "Enemy2"))
            {
                isTrig = true;
                wanderEnemy = other.gameObject.GetComponent<WanderEnemy>();

                if (playerMove.isNormalAttack)
                {
                    wanderEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isMidAttack)
                {
                    wanderEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isDownwardKick)
                {
                    wanderEnemy.enemyHP -= playerMove.atk;
                }
            }

            if (!isTrig && other.tag == "Boss")
            {
                isTrig = true;
                bossMove = other.GetComponent<BossMove>();
                if (playerMove.isNormalAttack)
                {
                    bossMove.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isMidAttack)
                {
                    bossMove.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isDownwardKick)
                {
                    bossMove.enemyHP -= playerMove.atk;
                }
            }

            if (!isTrig && (other.tag == "MidEnemy" || other.tag == "MidEnemyBoss"))
            {
                isTrig = true;
                midEnemy = other.gameObject.GetComponent<MidEnemy>();

                if (playerMove.isNormalAttack)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isMidAttack)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isDownwardKick)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
            }
            if (!isTrig && other.tag == "MidEnemyBoss")
            {
                isTrig = true;
                midEnemy = other.gameObject.GetComponent<MidEnemy>();

                if (playerMove.isNormalAttack)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isMidAttack)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
                else if (playerMove.isDownwardKick)
                {
                    midEnemy.enemyHP -= playerMove.atk;
                }
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

