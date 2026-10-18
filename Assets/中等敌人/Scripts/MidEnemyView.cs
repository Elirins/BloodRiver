using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidEnemyView : MonoBehaviour
{
    MidEnemy midEnemy;

    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = transform.parent;
        midEnemy = parentTransform.GetComponent<MidEnemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //主角进入视野主动接近
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            midEnemy.enemyState = State.Alarmed;
            midEnemy.timeStayAlarmed = Time.time;
            midEnemy.distance = (midEnemy.transform.position - other.gameObject.transform.position).magnitude;

        }
    }
    //主角处于视野中

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (midEnemy.allowToGetCloser == true)//主动接近主角
            {
                //transform.position += (other.transform.position - wanderEnemy.transform.position) 
                //                   * wanderEnemy.perchaseSpeed 
                //                   * wanderEnemy.movingSpeed 
                //                   * Time.fixedDeltaTime;
                midEnemy.animator.SetBool("是否移动", true);
                midEnemy.enemyState = State.Alarmed;
            }
            //判断主角是否还在接近
            if (Time.time - midEnemy.timeStayAlarmed >= midEnemy.timeToCheckWeatherCloser)
            {
                midEnemy.timeStayAlarmed = Time.time;
                midEnemy.lastDistance = midEnemy.distance;
                midEnemy.distance = (midEnemy.transform.position - other.transform.position).magnitude;
                if (midEnemy.distance == midEnemy.lastDistance)
                {
                    midEnemy.weatherMoveAway = true;
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
            midEnemy.enemyState = State.Idle;
            midEnemy.lastDistance = 0;
            midEnemy.distance = 0;
        }
    }
}
