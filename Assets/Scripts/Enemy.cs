using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于开发时测试
public class Enemy : MonoBehaviour
{
    public State enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = State.Xiapi;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
