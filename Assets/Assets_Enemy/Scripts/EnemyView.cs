using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    WanderEnemy wanderEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = transform.parent;
        wanderEnemy = parentTransform.GetComponent<WanderEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //主角进入视野主动接近
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            wanderEnemy.enemyState = State.Alarmed;
            wanderEnemy.timeStayAlarmed = Time.time;
            wanderEnemy.distance = (wanderEnemy.transform.position - other.gameObject.transform.position).magnitude;
            
        }
    }
    //主角处于视野中

    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            if (wanderEnemy.allowToGetCloser == true)//主动接近主角
            {
                //transform.position += (other.transform.position - wanderEnemy.transform.position) 
                //                   * wanderEnemy.perchaseSpeed 
                //                   * wanderEnemy.movingSpeed 
                //                   * Time.fixedDeltaTime;
                wanderEnemy.animator.SetBool("是否移动", true);
                wanderEnemy.enemyState = State.Alarmed;
            }
            //判断主角是否还在接近
            if (Time.time - wanderEnemy.timeStayAlarmed >= wanderEnemy.timeToCheckWeatherCloser)
            {
                wanderEnemy.timeStayAlarmed = Time.time;
                wanderEnemy.lastDistance = wanderEnemy.distance;
                wanderEnemy.distance = ( wanderEnemy.transform.position - other.transform.position).magnitude ;
                if (wanderEnemy.distance == wanderEnemy.lastDistance)
                {
                    wanderEnemy.weatherMoveAway = true;
                }
                // 如果主角靠近有小概率跑走一点点
            }
        }

    }

    //主角离开视野改变状态
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            wanderEnemy.enemyState = State.Idle;
            wanderEnemy.lastDistance = 0;
            wanderEnemy.distance = 0;
        }
    }
}
