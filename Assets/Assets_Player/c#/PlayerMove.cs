// 此处包含：角色移动及跳跃处理，与墙体接触处理，喝酒操作，角色身上提示
// 角色动画机的处理 ， 攻击判定及范围

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    private float moveSpeed = 5f; // 移动速度
    private float upSpeed = 4f ; // 爬梯子速度
    private float jumpForce = 15f ; // 跳跃力度
    private float woundedTime = 0f; // 僵直CD
    private float currentHP = 100f;
    public  float playerHP = 100f ;
    public  float maxHP = 100f;
    public  float atk = 40;
    public  bool atGround = false; // 是否落地
    private bool canJump = true ;  // 能不能跳
    private bool canMove = true ;  // 能不能走
    private bool canAtk = true;
    private bool isLabber = false;
    public bool isShrine = false; // 是否正在拜神龛
    public  bool isNormalAttack = false; // 是否正在普攻
    public  bool isMidAttack = false; // 是否正在中刺
    public  bool isDownwardKick = false; // 是否正在下劈
    public  bool isAttack = false;    // 是否正在攻击
    
    public static float horizontalInput; // 水平输入
    public static float verticalInput;   // 垂直输入
    //private int timer = 0;
    public int Bear = 4; // 酒的数量
    public int faceDirection = -1;
    private Color labelColor = Color.red ;
    private Vector3 newPosition;
    private Vector3 labelOffset = new Vector3(0, 2.5f, 0); // 标签偏移量

    private Vector3 originalSwardPosition; // 剑原始的相对位置
    private Vector3 originalSwardScale;    // 剑原始的缩放比例
    private Quaternion originalSwardRotation;    // 剑原始的相对旋转

    private Vector3 NAPosition = new Vector3(0.52f, -0.8f, 0f); // 普攻的相对位置
    private Vector3 NAScale = new Vector3(1.55f,1.9f, 1.14f); // 普攻的缩放比例
    private Vector3 DKPosition = new Vector3(1.22f, -1.4f, 0f); // 下劈的相对位置
    private Vector3 DKScale = new Vector3(1.24f, 0.1937545f, 1.14f); // 下劈的缩放比例
    private Quaternion DKRotation = Quaternion.Euler(new Vector3(0f, 0f, -23.4f)); // 下劈的相对旋转
    private Vector3 MPosition = new Vector3(1.7f, -0.8f, 0f); // 中刺的相对位置
    private Vector3 MScale = new Vector3(2.0f, 1.9f, 1.14f); // 中刺的缩放比例
    
    public string labelText = "" ;
    private string objectTag = "";
    public State playerState;
    public Rigidbody rb ; // 刚体 
    public Animator animator ;
    public Transform SwordTransform;
    private Healthbar playerHealthBar;
    private Collider playerCollider;
    private List<Collider> colliders;
    private PropertyUse propertyUse;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        SwordTransform = transform.Find("sword");
        originalSwardPosition = SwordTransform.localPosition;
        originalSwardScale = SwordTransform.localScale;
        originalSwardRotation = SwordTransform.localRotation;

        colliders = new List<Collider>();
        playerHealthBar = GameObject.Find("HealthBar").GetComponent<Healthbar>();
        playerCollider = GetComponent<Collider>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // 固定水平方向的旋转
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        playerHealthBar.SetHealth(playerHP);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("下劈前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击前摇") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("中刺前摇") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("喝酒前摇")
        )
            canMove = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("移动操控") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("下劈后摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击后摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("中刺后摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("喝酒后摇")
        )
        {
            canMove = true;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("待机")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("向右奔跑") )
        {
            animator.SetBool("是否结束爬梯子", false);
            canMove = true;
        }
        //死亡之后的代码
        if ( playerHP<0 )
        {
            animator.SetBool("是否死亡", true);
        }
        // 按E交互
        
        if (isShrine)
        {
            canMove = false;
            animator.SetBool("是否拜神龛", true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("拜神龛"))
                isShrine = false;
        }
        else
        {
            animator.SetBool("是否拜神龛", false);
        }
            
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ( objectTag == "Ladder" && !isLabber )
            {
                animator.SetBool("是否开始爬梯子", true);
                newPosition.z = transform.position.z;
                newPosition.y = transform.position.y+0.1f;
                transform.position = newPosition;
                isLabber = true;
                labelText = "";
            }
            if (objectTag == "Property")
            {
                foreach (Collider collider in colliders)
                {
                    propertyUse = collider.GetComponent<PropertyUse>();
                    propertyUse.isUse = true;
                    colliders.Remove(collider);
                    maxHP += 10;
                    playerHP += 10;
                    currentHP += 10;
                    labelText = "";
                    break;
                }
            }
        }

        
        if ( isLabber )
        {
            playerCollider.isTrigger = true;
            atGround = canJump = canMove = false;
            rb.useGravity = false; // 将重力和速度设置为0
            rb.velocity = Vector3.zero;
            
            if ( Input.GetKey(KeyCode.W) )
            {
                transform.Translate( Vector2.up * upSpeed * Time.deltaTime );
                animator.SetBool("是否开始爬梯子", false);
                animator.SetBool("是否正在爬梯子", true);
            }
            else if ( Input.GetKey(KeyCode.S) )
            {
                transform.Translate(Vector2.down * upSpeed * Time.deltaTime);
                animator.SetBool("是否开始爬梯子", false);
                animator.SetBool("是否正在爬梯子", true);
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                animator.SetBool("是否正在爬梯子", false);
            

        }

        if (isLabber) canAtk = false;
        else canAtk = true;

        if (canAtk)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetBool("是否普攻", true);
                isNormalAttack = true;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                animator.SetBool("是否下劈", true);
                isDownwardKick = true;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                animator.SetBool("是否中刺", true);
                isMidAttack = true;
            }
        }
        

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击"))
        {
            SwordTransform.localPosition = NAPosition;
            SwordTransform.localScale = NAScale;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("中刺"))
        {
            SwordTransform.localPosition = MPosition;
            SwordTransform.localScale = MScale;
            isAttack = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("下劈"))
        {
            SwordTransform.localPosition = DKPosition;
            SwordTransform.localScale = DKScale;
            SwordTransform.localRotation = DKRotation;
            isAttack = true;
            // 修改攻击判定碰撞器位置大小
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否普攻", false) ;
            isNormalAttack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("中刺后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否中刺", false);
            isMidAttack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("下劈后摇"))
        {
            animator.SetBool("是否移动", false);
            animator.SetBool("是否下劈", false);
            isDownwardKick = false;
            isAttack = false;
        }

        Vector2 movement = new Vector2( horizontalInput, 0f ) ;
        if ( !isLabber && canMove )
        {
            if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1.4f, 1.4f, 1); // 翻转180度
                animator.SetBool("是否移动", true);
                faceDirection = -1;
            }
            else if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(1.4f, 1.4f, 1); // 翻转180度
                animator.SetBool("是否移动", true);
                faceDirection = 1;
            }
            else
            {
                animator.SetBool("是否移动", false);
            }
        }
        else animator.SetBool("是否移动", false);


        if ( canMove != false )
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        /* 跳跃莫得了
        if ( Input.GetKeyDown(KeyCode.W) && canJump && !isLabber )
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            atGround = false;
            //transform.Translate(Vector2.up * jumpForce);
        }
        */
        if (transform.position.z != -1.5f )
        {
            fixZ(); // 修正z坐标
        }
            
        // 重力控制
        if ( atGround )
        {
            rb.useGravity = false ; // 将重力和速度设置为0
            rb.velocity = Vector3.zero ;
            
        }
        else if ( !atGround && !isLabber )
        {
            rb.useGravity = true ; // 否则恢复原来的重力值
        }

        if (!isNormalAttack && !isMidAttack && !isDownwardKick )
        {
            SwordTransform.localPosition = originalSwardPosition;
            SwordTransform.localScale = originalSwardScale;
            SwordTransform.localRotation = originalSwardRotation;
            // 恢复碰撞器为初始状态
        }

        if (currentHP > playerHP)
        {
            woundedTime++;
            
            if (woundedTime > 0 && woundedTime <= 52)
            {
                animator.SetBool("是否受击", true);
                animator.SetBool("是否普攻", false);
                animator.SetBool("是否中刺", false);
                animator.SetBool("是否下劈", false);
                animator.SetBool("是否移动", false);
                canMove = false;
                isNormalAttack = isMidAttack = isDownwardKick = false;
            }
            else if (woundedTime > 52 && woundedTime <= 300)
            {   
                animator.SetBool("是否受击", false);
                canMove = true;
            }
            else
            {
                currentHP = playerHP;
                woundedTime = 0;
            }
        }
        if (transform.position.z != -1)
        {
            fixZ(); // 修正z坐标
        }

        if (Input.GetKeyDown(KeyCode.Q) )
        {
            UseBear();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            animator.SetBool("是否喝酒", false);
        }
      
        // 是否死亡
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 判断碰撞的对象是否为地板
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 设置为接触到地板
            canJump = atGround = true;
            playerCollider.isTrigger = false;
        }
        
    }

    // 当Player对象与其他Collider停止碰撞时调用
    private void OnCollisionExit(Collision collision)
    {
        // 判断碰撞的对象是否为地板
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 设置为未接触到地板
            atGround = false;
        }
    }
    private void OnTriggerEnter( Collider other )
    {
        if (other.tag == "Ladder")
        {
            newPosition = other.transform.position;
            labelText = "按E使用";
            objectTag = other.tag;
        }
        if (other.tag == "Property")
        {
            labelText = "按E使用";
            colliders.Add(other);
            objectTag = other.tag;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if ( other.tag == "Ladder" )
        {
            animator.SetBool("是否正在爬梯子", false);
            animator.SetBool("是否结束爬梯子", true);
            labelText = objectTag = "";
            isLabber = false;
            canJump = canMove = true;
            playerCollider.isTrigger = false;
        }
        if (other.tag == "Property")
        {
            
            colliders.Remove(other);
        }
    }

    private void fixZ()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = -1.5f;
        transform.position = currentPosition;
    }
    
    public void RestoreBear()
    {
        Bear = 4;
    }

    
    public void UseBear()
    {
        if (Bear > 0)
        {
            Bear -= 1;
            playerHP += 80;
            currentHP = playerHP;
            if (playerHP > maxHP) 
                playerHP = maxHP;
            animator.SetBool("是否喝酒", true);
        }
        else ShowMessage("没有酒啦！", 1f);
            
    }

    void ShowMessage(string message, float duration)
    {
        labelText = message;

        // 使用协程等待一定时间后清除消息
        StartCoroutine(ClearMessageAfterDelay(duration));
    }
    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 清除控制台上的消息
        labelText = "";
    }

    private void OnGUI()
    {
        // 将物体的世界坐标转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + labelOffset);

        // 绘制标签
        GUIStyle style = new GUIStyle(GUI.skin.label); // 新建一个风格
        style.normal.textColor = labelColor; // 设置标签的颜色

        // 绘制标签
        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y-10, 100, 20), labelText, style);
    }

}

public enum State//状态机
{
    Idle,
    Xiapi,
    Huiji,
    Pindao,
    Zhongci,
    Defend,
    AttackBroken,
    BeHit,
    Alarmed,
    Dead
}
