using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWind : MonoBehaviour
{
    // Start is called before the first frame update
    public int faceDirection = -1;
    private int hideTimer = 0;
    private float speed = 10;
    private bool canMove = false;
    private bool isCollide = true; // 是否碰撞
    private float radius = 0.5f; // 是用来定义检测范围的半径值
    private Vector3 hidePozition = new Vector3(999f, 999f, 999f); // 剑气碰撞之后会被挪走 
    private Animator animator;
    private BossMove bossMove;
    private PlayerMove playerMove;
    void Start()
    {
        animator = GetComponent<Animator>();
        bossMove = GameObject.Find("Boss1").GetComponent<BossMove>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Wall" || collider.tag == "Player")
            {
                
                isCollide = true;
                animator.SetBool("是否碰撞", true);
            }
            if (collider.tag == "Player")
            {
                playerMove = collider.transform.GetComponent<PlayerMove>();
                if (!playerMove.isDownwardKick)
                {
                    playerMove.playerHP -= 1f ;
                }            
            }
        }

        //碰撞后隐藏
        if (isCollide)
        {
            hideTimer++;
            speed = 2;
            if ( hideTimer>80 )
            {
                hideTimer = 0;
                canMove = isCollide = false;
                Debug.Log("消失");
                transform.position = hidePozition;
                animator.SetBool("是否刷新", true);
            }
        }
            
            
        if (bossMove.animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1剑气前摇"))
        {
            isCollide = false;
            speed = 10;
            animator.SetBool("是否碰撞", false);
            animator.SetBool("是否刷新", false);
            faceDirection = bossMove.faceDirection;
            Vector3 newPosition = bossMove.transform.position;
            newPosition.x -= faceDirection * 1f;
            newPosition.y -= 2.2f;
            transform.position = newPosition;
            transform.localScale = new Vector3(faceDirection * -0.5f, 0.5f, 1f);
            
            canMove = true;
        }
        if (canMove)
            transform.position += new Vector3(faceDirection*speed* Time.fixedDeltaTime, 0, 0) ;
    }
}
