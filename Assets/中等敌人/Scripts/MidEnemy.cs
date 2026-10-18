using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidEnemy : MonoBehaviour
{
    private float timeStartToMove;//记录开始巡逻的时间
    private float movingTime;//设置巡逻时一次移动的时间
    private float dieTime = 0; // 死亡计时
    private float woundedTime = 0; // 受伤计时
    private float pauseTimer = 300; // 攻击CD
    private float timeStartToWait;//记录巡逻时在一个地点停留时开始的时间
    private float waitingTime = 180;//设置巡逻时在一个地点停留的时间
    private float ClockDistance = 3f; // 索敌距离
    private float PursuitDistance = 0.7f; // 追击距离
    public  float enemyHP ; //敌人血量
    private float currentHP ;
    public  float atk;

    public float timeStayAlarmed;//记录主角进入警戒范围的时间
    public float timeToCheckWeatherCloser;//判断主角是否还在靠近

    public bool weatherMoveAway;//判断是否要远离
    public bool isNormalAttack = false; // 是否正在普攻
    public bool isMidAttack = false; // 是否正在中刺
    public bool isDownwardKick = false; // 是否正在下劈
    public bool isHitPlayer = false; // 是否命中
    public bool isHitSword = false; // 是否拼刀
    public bool isAttack = false; // 是否攻击
    public bool weatherNoticePlayer = false;
    private bool atkWarring = false;
    private bool canAtk = false; // 能否攻击,用于锁住快速攻击
    private bool AIAttack = false; // 自动攻击
    private bool canMove = true;  // 能否走路
    private bool isHit = false; // 是否受击
    private bool pg = false, xp = false; // ttk函数内置

    private Vector3 originalSwardPosition; // 剑原始的相对位置
    private Vector3 originalSwardScale;    // 剑原始的缩放比例
    private Quaternion originalSwardRotation;    // 剑原始的相对旋转

    private Vector3 NAPosition = new Vector3(0.14f, -1.4f, 0f); // 普攻的相对位置
    private Vector3 NAScale = new Vector3(20f, 22.3f, 15f); // 普攻的缩放比例
    private Vector3 DKPosition = new Vector3(0, -1.9f, 0f); // 下劈的相对位置
    private Vector3 DKScale = new Vector3(35f, 14f, 15f); // 下劈的缩放比例
    private Vector3 MPosition = new Vector3(0.14f, -1.4f, 0f); // 中刺的相对位置
    private Vector3 MScale = new Vector3(20f, 22.3f, 15f); // 中刺的缩放比例
    private Vector3 originalPosition;

    private int firstLayer; // 随机攻击
    public int faceDirection;  //移动方向dw
    public float movingSpeed = 1f;//移动速度
    private float timer = 0;

    public State enemyState;

    public GameObject player;
    public GameObject sward;//获取武器对象

    private Transform swordTransform;
    private Transform playerTransform;
    private Transform enemyTransform;//获取对象
    private PlayerMove playerMove;
    private Rigidbody enemyrb;
    private SpriteRenderer spriteRenderer;
    public float lastDistance;
    public float distance;//用于判断主角是否在靠近


    public Animator animator;

    public bool allowToGetCloser = true;
    public float perchaseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "MidEnemy")
        {
            currentHP = enemyHP = 300;
            atk = 15;
        }
        else if (gameObject.tag == "MidEnemyBoss")
        {
            currentHP = enemyHP = 1000;
            atk = 30;
        }
        timeStartToMove = Time.time; // 一开游戏就用于计时
        movingTime = 150;              // 单次移动的时间
        faceDirection = 1;               // 判断左右朝向
        enemyState = State.Idle;
        timeToCheckWeatherCloser = 0.1f;
        weatherMoveAway = true;

        originalPosition = transform.localPosition;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        swordTransform = transform.Find("sword");
        //sward = swardTransform.gameObject ;
        playerMove = player.GetComponent<PlayerMove>();
        enemyrb = GetComponent<Rigidbody>();
        enemyrb.constraints = RigidbodyConstraints.FreezeRotation;
        // 固定水平方向的旋转
        perchaseSpeed = 0.5f;

        originalSwardPosition = swordTransform.localPosition;
        originalSwardScale = swordTransform.localScale;
        originalSwardRotation = swordTransform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimer > 400)
        {
            canAtk = true;
            pauseTimer = 0;
        }
        else
        {
            pauseTimer++;
            canAtk = false;
        }
        // 空闲状态时
        if (enemyState == State.Idle)
        {
            canMove = true;
            timer++;
            pg = xp = false;
            AIAttack = isHitPlayer = isHitSword = false;
            if (timer - timeStartToMove < movingTime)//巡逻
            {
                Move(movingSpeed * faceDirection);
                animator.SetBool("是否移动", true);
                timeStartToWait = timer;
            }
            else
            {
                animator.SetBool("是否移动", false);
                if (timer - timeStartToWait >= waitingTime)
                {
                    timeStartToMove = timer;
                    faceDirection *= -1;
                    transform.Rotate(0, 180, 0);
                }
            }

        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("奔跑 前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("脚踏 前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("滚砸 前冲 前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("冲砸 前冲 前摇")
        )
        { 
            canMove = false;
            atkWarring = true;
        }
            
        // 攻击提示闪红
        if (atkWarring)
            spriteRenderer.color = Color.red;
        else spriteRenderer.color = Color.white;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("待机")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("奔跑") )
        {
            canMove = true;
        }
        //主角进入视野改变状态
        if (enemyState == State.Alarmed)
        {
            MoveAway();
            // 根据追击距离改变是否移动
            if (!isHit && canMove && Mathf.Abs(transform.position.x - player.transform.position.x) > ClockDistance)
            {
                
                Move(movingSpeed * faceDirection);
                animator.SetBool("是否移动", true);
                pg = xp = false;
                AIAttack = isHitPlayer = isHitSword = false;
            }
            else if( !isHit && Mathf.Abs(transform.position.x - player.transform.position.x) <= ClockDistance)
            {
                // 在范围内就开始准备攻击
                if (Mathf.Abs(transform.position.x - player.transform.position.x) > PursuitDistance)
                        Move(movingSpeed * faceDirection);
                animator.SetBool("是否移动", false);
                if (!isHit && canAtk )
                {
                    ttk();
                }
            }

        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("滚砸 前冲"))
        {
            //普攻类
            swordTransform.localPosition = NAPosition;
            swordTransform.localScale = NAScale;
            atkWarring = false;
            isNormalAttack = true;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("冲砸 前冲"))
        {
            // 中刺类
            swordTransform.localPosition = MPosition;
            swordTransform.localScale = MScale;
            atkWarring = false;
            isMidAttack = true;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("脚踏"))
        {
            // 下劈类
            swordTransform.localPosition = DKPosition;
            swordTransform.localScale = DKScale;
            atkWarring = false;
            isDownwardKick = true;
            isAttack = true;
            // 修改攻击判定碰撞器位置大小
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("滚砸 后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否滚砸", false);
            isNormalAttack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("冲砸 后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否冲砸", false);
            isMidAttack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("脚踏 后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否脚踏", false);
            isDownwardKick = false;
            isAttack = false;
        }
        if (!isNormalAttack && !isMidAttack && !isDownwardKick)
        {
            swordTransform.localPosition = originalSwardPosition;
            swordTransform.localScale = originalSwardScale;
            swordTransform.localRotation = originalSwardRotation;
            // 恢复碰撞器为初始状态
        }
        if (enemyHP <= 0)
        {
            enemyState = State.Dead;
            animator.SetBool("是否死亡", true);
            dieTime++;
            if (dieTime > 60)
                gameObject.SetActive(false);
        }
        // 受击动画
        if (currentHP > enemyHP)
        {
            woundedTime++;

            if (woundedTime > 0 && woundedTime <= 52)
            {
                animator.SetBool("是否受击", true);
                animator.SetBool("是否滚砸", false);
                animator.SetBool("是否冲砸", false);
                animator.SetBool("是否脚踏", false);
                isHit = true;
                pg = xp = false;
                AIAttack = isHitPlayer = isHitSword =
                isNormalAttack = isMidAttack = isDownwardKick = false;
                atkWarring = false;
            }
            else if (woundedTime > 52 && woundedTime <= 400)
            {
                isHit = false;
                animator.SetBool("是否受击", false);
            }
            else
            {
                currentHP = enemyHP;
                woundedTime = 0;
            }
        }
        if (transform.position.z != -1)
        {
            fixPosition(); // 修正z坐标
        }
    }

    private void ttk()
    {
        firstLayer = Random.Range(0, 2);
        if (firstLayer == 0 && !xp && !pg)
            pg = true;
        else if (firstLayer == 1 && !xp && !pg)
            xp = true;
        // 普通攻击
        if (pg && !xp)
        {
            if (!AIAttack)
            {
                animator.SetBool("是否滚砸", true);
                AIAttack = true;
            }
            else if (isHitSword && AIAttack)
            {
                animator.SetBool("是否冲砸", true);
            }
            else if (isHitPlayer && AIAttack)
            {
                animator.SetBool("是否脚踏", true);
            }
        }
        // 下劈
        else if (xp && !pg)
        {
            if (!AIAttack)
            {
                animator.SetBool("是否脚踏", true);
                AIAttack = true;
            }
            if (isHitSword && AIAttack)
            {
                int secondLayer = Random.Range(0, 2);
                if (secondLayer == 0)
                {
                    animator.SetBool("是否滚砸", true);
                }
                else
                {
                    animator.SetBool("是否脚踏", true);
                }
            }
            else if (isHitPlayer && AIAttack)
            {
                animator.SetBool("是否滚砸", true);
            }
        }
    }

    //移动函数
    private void Move(float speed)
    {
        transform.position += new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
    }

    //主角主动接近时有几率后退
    private void MoveAway()
    {

        int weathermove = Random.Range(0, 20);
        if (distance < lastDistance && weatherMoveAway && weathermove > 10)
        {
            Debug.Log("退！退！退！");
            if ((transform.position - player.transform.position).x >= 0)
            {
                enemyrb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
            }
            else
            {
                enemyrb.AddForce(Vector3.left * 5f, ForceMode.Impulse);
            }
            weatherMoveAway = false;
        }
    }
    private void fixPosition()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = -1;
        transform.position = currentPosition;
    }
}
