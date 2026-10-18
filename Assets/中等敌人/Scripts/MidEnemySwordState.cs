using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidEnemySwordState : MonoBehaviour
{
    private float RepellingForce = 3f;
    private bool isTrig = false;
    private MidEnemy midEnemy;
    private PlayerMove playerMove;
    private Rigidbody enemyRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        midEnemy = transform.parent.GetComponent<MidEnemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTrig && other.tag == "PlayerSword")
        {
            isTrig = true;
            playerMove = other.gameObject.transform.parent.GetComponent<PlayerMove>();
            Debug.Log(playerMove);
            midEnemy.isHitPlayer = true;
            enemyRigidbody = midEnemy.gameObject.GetComponent<Rigidbody>();
            // 前后为：克制造成伤害，拼刀造成击退，被克制扣血
            if (playerMove.isNormalAttack && midEnemy.isDownwardKick)
            {
                enemyRigidbody.AddForce(Vector2.left * RepellingForce * midEnemy.faceDirection, ForceMode.Impulse);
                playerMove.playerHP -= midEnemy.atk;
            }
            else if (playerMove.isMidAttack && midEnemy.isNormalAttack)
            {
                enemyRigidbody.AddForce(Vector2.left * RepellingForce * midEnemy.faceDirection, ForceMode.Impulse);
                playerMove.playerHP -= midEnemy.atk;
            }
            else if (playerMove.isDownwardKick && midEnemy.isMidAttack)
            {
                enemyRigidbody.AddForce(Vector2.left * RepellingForce * midEnemy.faceDirection, ForceMode.Impulse);
                playerMove.playerHP -= midEnemy.atk;
            }
            else if ((playerMove.isNormalAttack && midEnemy.isNormalAttack) ||
                      (playerMove.isMidAttack && midEnemy.isMidAttack) ||
                      (playerMove.isDownwardKick && midEnemy.isDownwardKick))
                enemyRigidbody.AddForce(Vector2.left * RepellingForce * midEnemy.faceDirection, ForceMode.Impulse);
            else
            {
                if (midEnemy.isNormalAttack)
                    midEnemy.enemyHP -= playerMove.atk;
                else if (midEnemy.isMidAttack)
                    midEnemy.enemyHP -= playerMove.atk;
                else if (midEnemy.isDownwardKick)
                    midEnemy.enemyHP -= playerMove.atk;
            }
            return;
        }
        if (!isTrig && other.tag == "Player")
        {
            playerMove = other.gameObject.GetComponent<PlayerMove>();
            isTrig = true;
            midEnemy.isHitPlayer = true;
            if (midEnemy.isNormalAttack)
            {
                playerMove.playerHP -= midEnemy.atk;
            }
            else if (midEnemy.isMidAttack)
            {
                playerMove.playerHP -= midEnemy.atk;
            }
            else if (midEnemy.isDownwardKick)
            {
                playerMove.playerHP -= midEnemy.atk;
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
