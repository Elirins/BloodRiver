using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordState : MonoBehaviour
{
    private float RepellingForce = 3f;
    private bool isTrig = false;
    private WanderEnemy wanderEnemy;
    private PlayerMove playerMove;
    private Rigidbody enemyRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        wanderEnemy = transform.parent.GetComponent<WanderEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( !isTrig && other.tag == "PlayerSword")
        {
            isTrig = true;
            playerMove = other.gameObject.transform.parent.GetComponent<PlayerMove>();
            Debug.Log(playerMove);
                wanderEnemy.isHitPlayer = true;
                enemyRigidbody = wanderEnemy.gameObject.GetComponent<Rigidbody>();
                // 前后为：克制造成伤害，拼刀造成击退，被克制扣血
                if (playerMove.isNormalAttack && wanderEnemy.isDownwardKick)
                {
                    enemyRigidbody.AddForce(Vector2.left * RepellingForce * wanderEnemy.faceDirection, ForceMode.Impulse);
                    playerMove.playerHP -= wanderEnemy.atk;
                    Debug.Log("触发击退");
                }
                else if (playerMove.isMidAttack && wanderEnemy.isNormalAttack)
                {
                    enemyRigidbody.AddForce(Vector2.left * RepellingForce * wanderEnemy.faceDirection, ForceMode.Impulse);
                    playerMove.playerHP -= wanderEnemy.atk;
                }
                else if (playerMove.isDownwardKick && wanderEnemy.isMidAttack)
                {
                    enemyRigidbody.AddForce(Vector2.left * RepellingForce * wanderEnemy.faceDirection, ForceMode.Impulse);
                    playerMove.playerHP -= wanderEnemy.atk;
                }
                else if ( (playerMove.isNormalAttack && wanderEnemy.isNormalAttack) ||
                          (playerMove.isMidAttack && wanderEnemy.isMidAttack) ||
                          (playerMove.isDownwardKick && wanderEnemy.isDownwardKick))
                    enemyRigidbody.AddForce(Vector2.left * RepellingForce * wanderEnemy.faceDirection, ForceMode.Impulse);
                else
                {
                    if (wanderEnemy.isNormalAttack)
                        wanderEnemy.enemyHP -= playerMove.atk;
                    else if (wanderEnemy.isMidAttack)
                        wanderEnemy.enemyHP -= playerMove.atk;
                    else if (wanderEnemy.isDownwardKick)
                        wanderEnemy.enemyHP -= playerMove.atk;
                }
            return;
        }
        if (!isTrig && other.tag == "Player")
        {
            playerMove = other.gameObject.GetComponent<PlayerMove>();
            isTrig = true;
            wanderEnemy.isHitPlayer = true;
            if (wanderEnemy.isNormalAttack)
            {
                playerMove.playerHP -= wanderEnemy.atk;
            }
            else if (wanderEnemy.isMidAttack)
            {
                playerMove.playerHP -= wanderEnemy.atk;
            }
            else if (wanderEnemy.isDownwardKick)
            {
                playerMove.playerHP -= wanderEnemy.atk;
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isTrig = false;
        //wanderEnemy.isHitSword = false;
        //wanderEnemy.isHitPlayer = false;
    }
}
