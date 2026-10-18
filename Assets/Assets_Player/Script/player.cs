using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float Health = 5;
    public float Speed = 5f;//移速
    public float LadderSpeed = 5f;//爬梯子速度
    private Rigidbody2D rb;
    private Animator animator;
    private bool AllowInput = true;//设置移动输入是否有效
    private bool AllowAttack = true;//设置攻击输入是否有效
    private bool AllowLadder = false;//设置爬梯子输入是否有效

    //[SerializeField] private TrailRenderer tr;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (AllowInput)//角色移动
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 position = transform.position;
            position.x += horizontal * Speed;

            transform.position = position;
        }

        if (AllowLadder)//角色爬梯子
        {
            rb.gravityScale = 0;//使爬梯子时的重力失效
            float vertical = Input.GetAxis("Vertical");
            Vector2 position = transform.position;
            position.y += vertical * LadderSpeed;

            transform.position = position;
        }
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("下劈")|| animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击")|| animator.GetCurrentAnimatorStateInfo(0).IsName("防御")|| animator.GetCurrentAnimatorStateInfo(0).IsName("中刺")|| animator.GetCurrentAnimatorStateInfo(0).IsName("喝酒"))
        {
            AllowInput = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("移动操控")|| animator.GetCurrentAnimatorStateInfo(0).IsName("下劈后摇")|| animator.GetCurrentAnimatorStateInfo(0).IsName("普通挥击后摇")|| animator.GetCurrentAnimatorStateInfo(0).IsName("防御后摇")|| animator.GetCurrentAnimatorStateInfo(0).IsName("中刺后摇")|| animator.GetCurrentAnimatorStateInfo(0).IsName("喝酒后摇"))
        {
            AllowInput = true;
        }


            //状态机与按键输入之间的绑定

        if (Input.GetKeyDown(KeyCode.D) && AllowInput)
        {
            rb.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            animator.SetFloat("移动操控", 1);
        }
      
        if (Input.GetKeyDown(KeyCode.A) && AllowInput)
        {
            rb.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            animator.SetFloat("移动操控", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            rb.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            animator.SetFloat("移动操控", 0);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            rb.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            animator.SetFloat("移动操控", 0);
        }

        if(Input.GetKeyDown(KeyCode.H) && AllowAttack)
        {
            animator.SetBool("是否普攻", true);
            
        }

        if (Input.GetKeyUp(KeyCode.H) && AllowAttack)
        {
            animator.SetBool("是否普攻", false);
            
        }

        if (Input.GetKeyDown(KeyCode.K) && AllowAttack)
        {
            animator.SetBool("是否下劈", true);
            
        }

        if (Input.GetKeyUp(KeyCode.K) && AllowAttack)
        {
            animator.SetBool("是否下劈", false);
            
        }

        if (Input.GetKeyDown(KeyCode.Q) && AllowAttack)
        {
            animator.SetBool("是否喝酒", true);
            
        }

        if (Input.GetKeyUp(KeyCode.Q) && AllowAttack)
        {
            animator.SetBool("是否喝酒", false);
            
        }

        if (Input.GetKeyDown(KeyCode.J) && AllowAttack)
        {
            animator.SetBool("是否防御", true);
            
        }

        if (Input.GetKeyUp(KeyCode.J) && AllowAttack)
        {
            animator.SetBool("是否防御", false);
            
        }

        if (Input.GetKeyDown(KeyCode.U) && AllowAttack)
        {
            animator.SetBool("是否中刺", true);
            
        }

        if (Input.GetKeyUp(KeyCode.U) && AllowAttack)
        {
            animator.SetBool("是否中刺", false);
            
        }

        if(Health <= 0)
        {
            animator.SetBool("是否死亡", true);
            AllowInput = false;
            AllowAttack = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && AllowLadder)
        {
            animator.SetBool("是否正在爬梯子", true);
        }

        if (Input.GetKeyUp(KeyCode.W) && AllowLadder)
        {
            animator.SetBool("是否正在爬梯子", false);
        }

        if (Input.GetKeyDown(KeyCode.S) && AllowLadder)
        {
            animator.SetBool("是否正在爬梯子", true);
        }

        if (Input.GetKeyUp(KeyCode.S) && AllowLadder)
        {
            animator.SetBool("是否正在爬梯子", false);
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("ladder") && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("是否开始爬梯子", true);
            animator.SetBool("是否结束爬梯子", false);
            AllowInput = false;
            AllowAttack = false;
            AllowLadder = true;
            
        }

        if (collision.CompareTag("ladderExit") && AllowLadder)
        {
            animator.SetBool("是否开始爬梯子", false);
            animator.SetBool("是否结束爬梯子", true);
            AllowInput = true;
            AllowAttack = true;
            AllowLadder = false;
            rb.gravityScale = 1f;//调回爬梯子时弄没的重力

        }

        
    }



}

