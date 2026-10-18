using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//放在enemy的子物体weapon,玩家需要设置tag为Player
public class EnemyAttack : MonoBehaviour
{
    private int whickAttack;//决定执行哪个攻击动作
    private Transform parentTransform;//获取父物体
    private State parentState;//获取父物体状态
    private Animator animator;

    private bool weatherAttack;
    private float timeStartAttack;
    public float attackTime;

    private Transform player;
    private State playerState;
    public AnimatorStateInfo stateInfo;

    private bool allowToAttack;//判断player是否处于攻击范围
    private void Start()
    {
        parentTransform = transform.parent;
        animator = parentTransform.GetComponent<Animator>();
        weatherAttack = true;
        attackTime = 1.5f;
        allowToAttack = false;
        player = GameObject.Find("PlayerO").transform;
    }

    private void Update()
    {
        parentState = parentTransform.GetComponent<WanderEnemy>().enemyState;//更新父物体状态
        playerState = player.GetComponent<TestPlayer>().playerState;//更新玩家状态
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
     
        AttackPlayer();

        if(parentState == State.Huiji || parentState == State.Xiapi || parentState == State.Defend || parentState == State.Zhongci)
        {
            StopAttackAnimator();
        }

        if(parentState == State.Pindao)
        {
            PinDaoAnimator();
        }

    }

    private void StopAttackAnimator()
    {
        if (stateInfo.normalizedTime >= 0.99f)
        {
            StopAnimator();
            parentTransform.GetComponent<WanderEnemy>().enemyState = State.Alarmed;
        }

    }

    private void PinDaoAnimator()
    {
        if (stateInfo.normalizedTime >= 0.99f)
        {
            animator.SetBool("是否喝酒", false);
            parentTransform.GetComponent<WanderEnemy>().enemyState = State.Alarmed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            allowToAttack = true;
            parentTransform.GetComponent<WanderEnemy>().allowToGetCloser = false;
            animator.SetFloat("移动操控", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            weatherAttack = true;
            allowToAttack = false;
            parentTransform.GetComponent<WanderEnemy>().allowToGetCloser = true;
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - timeStartAttack >= attackTime)
        {
            weatherAttack = true;
        }

        if (weatherAttack == true && allowToAttack == true)
       {
            timeStartAttack = Time.time;
            //whickAttack = Random.Range(0, 3);
            whickAttack = 0;

            switch (whickAttack)
            {
                case 0:
                    XiaPi();
                    break;
                case 1:
                    HuiJi(); 
                    break;
                case 2:
                    Defend();
                    break;
            }

            weatherAttack = false;

        }
    }

    private void StopAnimator()
    {
        animator.SetBool("是否下劈", false);
        animator.SetBool("是否普攻", false);
        animator.SetBool("是否防御", false);
        animator.SetBool("是否中刺", false);
        animator.SetBool("是否喝酒", false);
    }

    

    private void XiaPi()
    {
        parentTransform.GetComponent<WanderEnemy>().enemyState = State.Xiapi;
        timeStartAttack = Time.time;
        animator.SetBool("是否下劈", true);
        if (allowToAttack == true && playerState == State.Xiapi)//allowToAttack用来保证下劈时玩家在攻击范围内(感觉加不加都行)
        {
            animator.SetBool("是否下劈", false);
            parentTransform.GetComponent<WanderEnemy>().enemyState = State.Pindao;
            animator.SetBool("是否喝酒", true);
            //transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void HuiJi()
    {
        parentTransform.GetComponent<WanderEnemy>().enemyState = State.Huiji;
        timeStartAttack = Time.time;
        animator.SetBool("是否普攻", true);
       
    }

    private void Defend()
    {
        parentTransform.GetComponent<WanderEnemy>().enemyState = State.Defend;
        timeStartAttack = Time.time;
        animator.SetBool("是否防御", true);

    }

    private void ZhongCi()
    {
        parentTransform.GetComponent<WanderEnemy>().enemyState = State.Zhongci;
        timeStartAttack = Time.time;
        animator.SetBool("是否中刺", true);

    }

}
