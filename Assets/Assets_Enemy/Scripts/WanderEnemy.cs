// 下次制作：战斗！

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

//需要enemy物体挂载子物体命名为weapon，脚本放在父物体
public class WanderEnemy : MonoBehaviour
{
    private float timeStartToMove;//记录开始巡逻的时间
    private float movingTime;//设置巡逻时一次移动的时间
    private float dieTime = 0 ; // 死亡计时
    private float woundedTime = 0; // 受伤计时
    private float timeStartToWait;//记录巡逻时在一个地点停留时开始的时间
    private float waitingTime = 180;//设置巡逻时在一个地点停留的时间
    private float PursuitDistance = 0.9f; // 追击距离
    public  float enemyHP  ; //敌人血量
    private float currentHP  ;
    public  float timeStayAlarmed;//记录主角进入警戒范围的时间
    public  float timeToCheckWeatherCloser;//判断主角是否还在靠近
    public  float atk; // 攻击力
    public  bool weatherMoveAway;//判断是否要远离
    public  bool isNormalAttack = false; // 是否正在普攻
    public  bool isMidAttack = false; // 是否正在中刺
    public  bool isDownwardKick = false; // 是否正在下劈
    public  bool isHitPlayer = false; // 是否命中
    public  bool isHitSword = false; // 是否拼刀
    public  bool isAttack = false; // 是否攻击
    public  bool atkWarring = false; // 攻击预警 
    public  bool weatherNoticePlayer = false; 
    private bool AIAttack = false; // 自动攻击
    private bool canMove = true;  // 能否走路
    private bool isHit = false; // 是否受击
    private bool pg = false, xp = false; // ttk函数内置

    private Vector3 originalSwardPosition; // 剑原始的相对位置
    private Vector3 originalSwardScale;    // 剑原始的缩放比例
    private Quaternion originalSwardRotation;    // 剑原始的相对旋转

    private Vector3 NAPosition = new Vector3(0.72f, -1.21f, 0f); // 普攻的相对位置
    private Vector3 NAScale = new Vector3(0.7f, 0.1937545f, 1.1368643f); // 普攻的缩放比例
    private Vector3 DKPosition = new Vector3(1.22f, -1.4f, 0f); // 下劈的相对位置
    private Vector3 DKScale = new Vector3(1.24f, 0.1937545f, 1.1368643f); // 下劈的缩放比例
    private Quaternion DKRotation = Quaternion.Euler(new Vector3(0f, 0f, -23.4f)); // 下劈的相对旋转
    private Vector3 MPosition = new Vector3(1.42f, -1.21f, 0f); // 中刺的相对位置
    private Vector3 MScale = new Vector3(1.5f, 0.1937545f, 1.1368643f); // 中刺的缩放比例
    private Vector3 originalPosition;

    private int firstLayer; // 随机攻击
    public  int faceDirection;  //移动方向dw
    public  float movingSpeed = 1f;//移动速度
    private float timer = 0 ;

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

    public bool allowToGetCloser= true;
    public float perchaseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        timeStartToMove = Time.time; // 一开游戏就用于计时
        movingTime = 150;              // 单次移动的时间
        faceDirection = 1;               // 判断左右朝向
        enemyState = State.Idle;     
        timeToCheckWeatherCloser = 0.1f;
        weatherMoveAway = true;

        originalPosition = transform.localPosition;
        if ( gameObject.tag == "Enemy1" )
        {
            currentHP = enemyHP = 90;
            atk = 10;
        }
        else if (gameObject.tag == "Enemy2")
        {
            currentHP = enemyHP = 80;
            atk = 20;
        }
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        swordTransform = transform.Find("sword");
        //sward = swardTransform.gameObject ;
        playerMove = player.GetComponent<PlayerMove>();
        enemyrb = GetComponent<Rigidbody>() ;
        enemyrb.constraints = RigidbodyConstraints.FreezeRotation;
        // 固定水平方向的旋转
        perchaseSpeed = 0.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSwardPosition = swordTransform.localPosition;
        originalSwardScale = swordTransform.localScale;
        originalSwardRotation = swordTransform.localRotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // 空闲状态时
        if (enemyState == State.Idle)
        {
            canMove = true;
            timer++ ;
            pg = xp = false;
            AIAttack = isHitPlayer = isHitSword = false;
            animator.SetBool("是否巡逻", true);
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("1下劈前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("1普攻前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("1中刺前摇")
        )
        {
            canMove = false;
            atkWarring = true;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("1待机"))
        {
            canMove = true;
        }
        // 攻击提示闪红
        if ( atkWarring )
            spriteRenderer.color = Color.red;
        else spriteRenderer.color = Color.white;
        //主角进入视野改变状态
        if (enemyState == State.Alarmed) 
        {
            animator.SetBool("是否巡逻", false);
            MoveAway();
            // 根据追击距离改变是否移动
            if ( !isHit && canMove && Mathf.Abs(transform.position.x - player.transform.position.x) > PursuitDistance)
            {
                Move(movingSpeed * faceDirection);
                animator.SetBool("是否移动", true);
                pg = xp = false;
                AIAttack = isHitPlayer = isHitSword = false;
            }
            else
            {
                // 在范围内就开始准备攻击
                animator.SetBool("是否移动", false);
                if (!isHit)
                    ttk();
            }

        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("1普攻"))
        {
            swordTransform.localPosition = NAPosition;
            swordTransform.localScale = NAScale;
            atkWarring = false;
            isNormalAttack = true;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("1中刺"))
        {
            swordTransform.localPosition = MPosition;
            swordTransform.localScale = MScale;
            atkWarring = false;
            isMidAttack = true;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("1下劈"))
        {
            swordTransform.localPosition = DKPosition;
            swordTransform.localScale = DKScale;
            swordTransform.localRotation = DKRotation;
            atkWarring = false;
            isDownwardKick = true;
            isAttack = true;
            // 修改攻击判定碰撞器位置大小
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("1普攻后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否普攻", false);
            isNormalAttack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("1中刺后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否中刺", false);
            isMidAttack = false;
            isAttack = false;
        }
        else if ( animator.GetCurrentAnimatorStateInfo(0).IsName("1下劈后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否下劈", false);
            isDownwardKick = false;
            isAttack = false;
        }
        if  (!isNormalAttack && !isMidAttack && !isDownwardKick)
        {
            swordTransform.localPosition = originalSwardPosition;
            swordTransform.localScale = originalSwardScale;
            swordTransform.localRotation = originalSwardRotation;
            // 恢复碰撞器为初始状态
        }
        if ( enemyHP <= 0) 
        {
           enemyState = State.Dead;
           animator.SetBool("是否死亡", true);
           dieTime++;
           if ( dieTime > 60 )
               gameObject.SetActive(false);
        }
        // 受击动画
        if (currentHP > enemyHP)
        {
            woundedTime++;
            
            if (woundedTime > 0 && woundedTime <= 52)
            {
                animator.SetBool("是否受击", true);
                animator.SetBool("是否普攻", false);
                animator.SetBool("是否中刺", false);
                animator.SetBool("是否下劈", false);
                isHit = true;
                pg = xp = false;
                AIAttack = isHitPlayer = isHitSword  = 
                isNormalAttack = isMidAttack = isDownwardKick = false;
                atkWarring = false;
            }
            else if (woundedTime > 52 && woundedTime <= 200)
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
        if ( transform.position.z != -1 )
        {
            fixPosition(); // 修正z坐标
        }
    }
    
    private void ttk ()
    {
        firstLayer = Random.Range(0, 2);
        if ( firstLayer == 0 && !xp && !pg )
            pg = true;
        else if ( firstLayer == 1 && !xp && !pg )
            xp = true;
        // 普通攻击
        if ( pg && !xp )
        {
            if ( !AIAttack)
            { 
                animator.SetBool("是否普攻", true);
                AIAttack = true;
            }
            else if ( isHitSword && AIAttack)
            {
                animator.SetBool("是否中刺", true);
            }
            else if ( isHitPlayer && AIAttack)
            {
                animator.SetBool("是否下劈", true);
            }
        }
        // 下劈
        else if ( xp && !pg )
        {
            if ( !AIAttack)
            {
                animator.SetBool("是否下劈", true);
                AIAttack = true;
            }
            if ( isHitSword && AIAttack )
            {
                int secondLayer = Random.Range(0, 2);
                if ( secondLayer == 0 )
                {
                    animator.SetBool("是否普攻", true);
                }
                else
                {
                    animator.SetBool("是否下劈", true);
                }
            }
            else if ( isHitPlayer && AIAttack )
            {
                animator.SetBool("是否普攻", true);
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
        
        int weathermove = Random.Range(0, 20) ;
        if (distance < lastDistance && weatherMoveAway && weathermove >10)
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


