using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public State playerState;


    private float moveSpeed = 5f; // 移动速度
    public float playerHP = 100;
    private bool canMove = true;  // 能不能走
    public bool isNormalAttack = false; // 是否正在普攻
    public bool isMidAttack = false; // 是否正在中刺
    public bool isDownwardKick = false; // 是否正在下劈
    public bool isAttack = false;    // 是否正在攻击
    public static float horizontalInput; // 水平输入
    public static float verticalInput;   // 垂直输入
    //private int timer = 0;
    public int Bear = 3; // 酒的数量
    public int faceDirection = 1;


    public string labelText = "";
    public Rigidbody2D rb; // 刚体 
    public Animator animator;
    public Transform SwordTransform;


    //以下为开发喝酒增加的变量！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    private float timeStartAttack;
    private bool weatherDrinkWine;
    public int wineLeft;
    public GameObject wineUI;
    private Transform[] wines;

    void Start()
    {
        playerState = State.Idle;

        transform.localScale = new Vector3(-0.7f, 0.7f, 1); // 翻转180度
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();


        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        //喝酒初始化
        weatherDrinkWine = true;
        wineLeft = 3;
        wines = wineUI.transform.GetComponentsInChildren<Transform>(true);
    }

    // Update is called once per frame
    void Update()
    {
        playerHP = GetComponent<TakeDamage>().health;
        if (Input.GetMouseButtonDown(0))
        {
            playerState = State.Xiapi;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        Vector2 movement = new Vector2(horizontalInput, 0f);
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-0.7f, 0.7f, 1); // 翻转180度
            animator.SetFloat("移动操控", 1);
            faceDirection = -1;
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 1); // 翻转180度
            animator.SetFloat("移动操控", 1);
            faceDirection = 1;
        }
        else
        {
            animator.SetFloat("移动操控", 0);
        }

        if (canMove != false)
            transform.Translate(movement * moveSpeed * Time.deltaTime);

        if (transform.position.z != -1)
        {
            fixZ(); // 修正z坐标
        }

        if(Input.GetKeyDown(KeyCode.J)) GetComponent<TakeDamage>().PlayerTakeDamage(50); //按键扣血（测试用）

        if (Input.GetKeyDown(KeyCode.G))//G键喝酒，没有酒时输出日志
        {
            if(wineLeft > 0)
            {
                animator.SetBool("是否喝酒", true);
                timeStartAttack = Time.time;
            }
            else Debug.Log("没有酒啦！");
        }

        DrinkWine();





    }

    private void StopAttackAnimator(float time)//技能结束关闭动画 并重置敌人状态
    {
        if (Time.time - timeStartAttack >= time)
        {
            animator.SetBool("是否喝酒", false);
            weatherDrinkWine = true;
        }

    }

    private void fixZ()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = -1;
        transform.position = currentPosition;
    }

    private void DrinkWine()//喝酒逻辑
    {

        if(animator.GetBool("是否喝酒"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("喝酒后摇") && weatherDrinkWine == true)
            {
                Debug.Log("Drink Wine");
                GetComponent<TakeDamage>().health += 50;
                wines[wineLeft].gameObject.SetActive(false);
                wineLeft -= 1;
                weatherDrinkWine = false;

            }

            StopAttackAnimator(0.6f);

        }

    }



}
