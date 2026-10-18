using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//放在enemy的子物体weapon,玩家需要设置tag为Player
public class EnemyAttack_failed : MonoBehaviour
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
        whickAttack = -1;
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

        if(weatherAttack == false)
        {
            switch (parentState)
            {
                case State.Huiji:
                    animator.SetBool("是否普攻", true);
                    break;
                case State.Xiapi:
                    animator.SetBool("是否下劈", true);
                    break;
                case State.Defend:
                    animator.SetBool("是否防御", true);
                    break;
                case State.Zhongci:
                    animator.SetBool("是否中刺", true);
                    break;
            }
        }

        if(parentState == State.Huiji || parentState == State.Xiapi || parentState == State.Defend || parentState == State.Zhongci)
        {
            if (parentState != State.Defend && parentState == playerState)
            {
                StopAnimator();
                parentTransform.GetComponent<WanderEnemy>().enemyState = State.Pindao;
            }

            Debug.Log(stateInfo.IsName("普通挥击"));
            if (stateInfo.normalizedTime >= 5f)
            {
                Debug.Log(11);
                //StopAnimator();
                //parentTransform.GetComponent<WanderEnemy>().enemyState = State.Alarmed;
            }
        }

        if(parentState == State.Pindao)
        {
            PinDaoAnimator();
        }

    }

    private void StopAttackAnimator()
    {
        if (stateInfo.normalizedTime >= 1f)
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

            //if(whickAttack == -1)
            //{
            //    whickAttack = Random.Range(0, 3);
            //    Debug.Log(whickAttack);
            //}

            whickAttack = 0;

            switch (whickAttack)
            {
                case 0:
                    parentTransform.GetComponent<WanderEnemy>().enemyState = State.Huiji;
                    break;
                case 1:
                    parentTransform.GetComponent<WanderEnemy>().enemyState = State.Xiapi;
                    break;
                case 2:
                    parentTransform.GetComponent<WanderEnemy>().enemyState = State.Defend;
                    break;
                case 3:
                    parentTransform.GetComponent<WanderEnemy>().enemyState = State.Zhongci;
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

    //private void XiaPi()
    //{
    //    timeStartAttack = Time.time;
    //    animator.SetBool("是否下劈", true);
    //    if (allowToAttack == true && playerState == State.Xiapi)//allowToAttack用来保证下劈时玩家在攻击范围内(感觉加不加都行)
    //    {
    //        animator.SetBool("是否下劈", false);
    //        parentTransform.GetComponent<WanderEnemy>().enemyState = State.Pindao;
    //        animator.SetBool("是否喝酒", true);
    //        //transform.GetComponent<BoxCollider2D>().enabled = false;
    //    }
    //}

}
