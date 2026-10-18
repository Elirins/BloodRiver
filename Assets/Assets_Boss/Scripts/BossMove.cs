using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;
// 由于某种原因，Boss1的faceDirection为1时，缩放为-1，与主角相反！！！
public class BossMove : MonoBehaviour
{

    private float dieTime = 0; // 死亡计时
    private float woundedTime = 0; // 受伤计时
    public float woundedTimeMax =2000; // 受伤最大时间
    private float PursuitInShortDistance = 4f; // 追击距离
    private float PursuitInMidDistance = 16f; // 中距离追击
    private float PursuitInLongDistance = 20f; // 远处追击
    public float enemyHP = 1000; //敌人血量
    private float currentHP = 1000;
    public float timeStayAlarmed;//记录主角进入警戒范围的时间
    public float timeToCheckWeatherCloser;//判断主角是否还在靠近

    private bool isPause = false;// 暂停
    private bool isComeOnState = false; // 是否播放完进场动画
    public bool isNormalAttack = false; // 是否正在普攻
    public bool isMidAttack = false; // 是否正在中刺
    public bool isDownwardKick = false; // 是否正在下劈
    public bool isFlashAndDown = false; // 是否瞬闪下劈
    public bool isFlashBack = false; // 是否瞬身斩
    public bool isRoundaboutSlash = false; // 是否回身斩

    public bool isHitPlayer = false; // 是否命中
    public bool isHitSword = false; // 是否拼刀
    public bool isAttack = false; // 是否攻击
    private bool AIAttack = false; // 自动攻击
    private bool canMove = true;  // 能否走路
    private bool isHit = false; // 是否受击
    private bool atkWarring = false;
    private bool canLongDistanceAtk = false; // 距离远时能否发动攻击
    private bool canMidDistanceAtk = false; // 距离中等时能否发动攻击
    private bool canAtk = false; // 能否攻击,用于锁住boss快速攻击
    private bool pg = false, xp = false, zc = false ; // ttk函数内置

    private Vector3 originalSwardPosition; // 剑原始的相对位置
    private Vector3 originalSwardScale;    // 剑原始的缩放比例
    private Quaternion originalSwardRotation;    // 剑原始的相对旋转
    private Vector3 FBPosition = new Vector3(-2.36f, -1.4f, 0f); // 瞬身斩的相对位置
    private Vector3 FBScale = new Vector3(3.8f, 2.7f, 1.14f); // 瞬身斩的缩放比例
    private Vector3 FDPosition = new Vector3(0f, -1.7f, 0f); // 瞬闪下落的相对位置
    private Vector3 FDScale = new Vector3(8.2f, 2.2f, 1.14f); // 瞬闪下落的缩放比例
    public  Vector3 RSPositionFirst = new Vector3(-1.6f, -1.2f, 0f); // 回身斩第一刀的相对位置
    public  Vector3 RSScaleFirst = new Vector3(3.2f, 3.2f, 1.14f); // 回身斩第一刀的缩放比例
    public  Vector3 RSPositionSecond = new Vector3(-2f, -1.2f, 0f); // 回身斩第二刀的相对位置
    public  Vector3 RSScaleSecond = new Vector3(3.6f, 3.2f, 1.14f); // 回身斩第二刀的缩放比例

    private int firstLayer; // 随机攻击
    public int faceDirection;  //移动方向
    public float movingSpeed = 1f;//移动速度
    private int pauseTimer = 900; // 攻击CD计时器
    private int backTimer = 0; // 转身计时器
    private int comeOnTimer = 0;// 出场动画计时器
    public State boss1State;

    public GameObject player;
    public GameObject sward;//获取武器对象
    public Transform swordTransform;
    private Transform playerTransform;
    public PlayerMove playerMove;
    private Rigidbody bossrb;
    private Healthbar boss1HpBar;
    private SpriteRenderer spriteRenderer;
    public float lastDistance;
    public float distance;//用于判断主角是否在靠近


    public Animator animator;

    public bool allowToGetCloser = true;
    public float perchaseSpeed;
   

    // Start is called before the first frame update
    void Start()
    {
        
        faceDirection = -1;               // 判断左右朝向
        boss1State = State.Idle;
        timeToCheckWeatherCloser = 0.1f;

        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        swordTransform = transform.Find("sword");
        //sward = swardTransform.gameObject ;
        playerMove = player.GetComponent<PlayerMove>();
        bossrb = GetComponent<Rigidbody>();
        bossrb.constraints = RigidbodyConstraints.FreezeRotation;
        // 固定水平方向的旋转
        perchaseSpeed = 0.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss1HpBar = GameObject.Find("Boss1HpBar").GetComponent<Healthbar>();
        originalSwardPosition = swordTransform.localPosition;
        originalSwardScale = swordTransform.localScale;
        originalSwardRotation = swordTransform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if ( comeOnTimer>500 )
            isComeOnState = true;
        else comeOnTimer++;
        if ( pauseTimer>500 )
        {
            canAtk = true;
            pauseTimer = 0;
        }
        else
        {
            pauseTimer++;
            canAtk = false;
        }
        // 实时更新血条长度
        boss1HpBar.SetHealth(enemyHP);
        // 空闲状态时
        if (boss1State == State.Idle)
        {
            canMove = true;
            pg = zc = xp = false;
            AIAttack = isHitPlayer = isHitSword = canLongDistanceAtk = false;
        }
        // 攻击提示闪红
        if (atkWarring)
            spriteRenderer.color = Color.red;
        else spriteRenderer.color = Color.white;
        // boss时刻面向玩家
        if (backTimer > 60 && !canAtk )
        {
            TurnBack();
        }
        else backTimer++;
        // 当攻击开始时禁止移动,闪红提示
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1剑气前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1回身斩第一刀前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬身斩第一刀前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬闪下落攻击前摇") ||
            isFlashBack || isFlashAndDown || isRoundaboutSlash
        )
            canMove = false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1剑气前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1回身斩第一刀前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬身斩第一刀前摇") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬闪下落攻击前摇") 
        )
            atkWarring = true;

        if (Input.GetKeyDown(KeyCode.P))
            SwordWind();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1初始状态"))
        {
            canMove = true;
        }
        //主角进入视野改变状态
        if (boss1State == State.Alarmed && isComeOnState)
        {
            // 根据追击距离改变是否移动
            if (!isHit && canMove && Mathf.Abs(transform.position.x - player.transform.position.x) > PursuitInLongDistance )
            {
                animator.SetBool("移动", true);
                Move(movingSpeed * faceDirection);
                pg = zc = xp = false;
                AIAttack = isHitPlayer = isHitSword = false;
                canLongDistanceAtk = canMidDistanceAtk = true;
            }
            else if (!isHit && canMove &&  
                Mathf.Abs(transform.position.x - player.transform.position.x) <= PursuitInLongDistance &&
                Mathf.Abs(transform.position.x - player.transform.position.x) > PursuitInMidDistance )
            {
                canMidDistanceAtk = true;
                // 在进入范围时只判定一次，没成功就继续靠近
                if (canLongDistanceAtk)
                {
                    int guide = Random.Range(0, 2);
                    if (guide == 0)
                        atkkInLongDistance();
                    canLongDistanceAtk = false;
                }
                else
                {
                    Move(movingSpeed * faceDirection);
                    animator.SetBool("移动", true);
                    pg = zc = xp = false;
                    AIAttack = isHitPlayer = isHitSword = false;
                }

            }
            else if (!isHit && canMove &&
                Mathf.Abs(transform.position.x - player.transform.position.x) <= PursuitInMidDistance &&
                Mathf.Abs(transform.position.x - player.transform.position.x) > PursuitInShortDistance)
            {
                animator.SetBool("移动",false); 
                // 在此距离下不再自动靠近，反正打的到
                if (canAtk)
                {
                    atkkInMidDistance();
                    pauseTimer = 0;
                }
                
                
                  
            }
            else if (!isHit && canMove && Mathf.Abs(transform.position.x - player.transform.position.x) <= PursuitInShortDistance)
            {
                // 贴脸的
                if ( canAtk )
                {
                    atkkInShortDistance();
                    pauseTimer = 0;
                }
            }
        }

        // 动画器检测
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬身斩第一刀"))
        {
            swordTransform.localPosition = FBPosition;
            swordTransform.localScale = FBScale;
            isAttack = isFlashBack = true;
            atkWarring = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1回身斩第一刀"))
        {
            // 变换碰撞器在动画器事件FirstOfRS
            isAttack = isRoundaboutSlash = true;
            atkWarring = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬闪下落攻击"))
        {
            swordTransform.localPosition = FDPosition;
            swordTransform.localScale = FDScale;
            isAttack = isFlashAndDown = true;
            atkWarring = false;
            // 修改攻击判定碰撞器位置大小
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬身斩第一刀前摇"))
        {
            TurnBack();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1回身斩第二刀后摇"))
        {
            animator.SetBool("移动", false);
            animator.SetBool("回身斩", false);
            isNormalAttack = false;
            isRoundaboutSlash = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬身斩第二刀后摇"))
        {
            animator.SetBool("移动", false);
            animator.SetBool("瞬身斩", false);
            isNormalAttack = isFlashBack = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1瞬闪下落攻击后摇"))
        {
            animator.SetBool("移动", false);
            animator.SetBool("瞬闪下落", false);
            isDownwardKick = false;
            isFlashAndDown = false;
            isAttack = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BOSS1剑气后摇"))
        {
            animator.SetBool("移动", false);
            animator.SetBool("剑气", false);
            isMidAttack = false;
            isAttack = false;
            atkWarring = false;
        }
        if (!isFlashBack && !isFlashAndDown && !isRoundaboutSlash)
        {
            swordTransform.localPosition = originalSwardPosition;
            swordTransform.localScale = originalSwardScale;
            swordTransform.localRotation = originalSwardRotation;
            // 恢复碰撞器为初始状态
        }
        if (enemyHP <= 0)
        {
            boss1State = State.Dead;
            animator.SetBool("死亡", true);
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
                animator.SetBool("受击", true);
                animator.SetBool("剑气", false);
                animator.SetBool("回身斩", false);
                animator.SetBool("瞬身斩", false);
                animator.SetBool("瞬闪下落", false);
                isHit = true;
                pg = xp = zc = false;
                AIAttack = isHitPlayer = isHitSword =
                isNormalAttack = isMidAttack = isDownwardKick = 
                isFlashAndDown = isFlashBack = isRoundaboutSlash = false;
                atkWarring = false;
            }
            else if (woundedTime > 52 && woundedTime <= 2000)
            {   // boss的僵直保护时间更长
                isHit = false;
                animator.SetBool("受击", false);
            }
            else
            {
                currentHP = enemyHP;
                woundedTime = 0;
            }
        }
        if (transform.position.z != -1)
        {
            fixZ(); // 修正z坐标
        }
    }
    // 在外层调用这个函数时需先判定，此函数触发率50%
    private void atkkInLongDistance()
    {
        firstLayer = Random.Range(0, 2);
        if (firstLayer == 0 && !xp && !pg)
            pg = true;
        else if (firstLayer == 1 && !xp && !pg)
            xp = true;
        // 瞬身斩(普攻类)
        if (pg && !zc && !xp)
        {
            FlashBack();
        }
        // 超级多次下劈
        else if (!pg && !zc && xp)
        {
            FlashAndDown();
        }
        pg = xp = false;
    }
    private void atkkInMidDistance()
    {
        firstLayer = Random.Range(0, 2);
        if (firstLayer == 0 && !xp && !pg && !zc)
            pg = true;
        else if (firstLayer == 1 && !xp && !pg && !zc)
            zc = true; 
        // 瞬身斩 (普攻类)
        if (pg && !zc && !xp)
        {
            FlashBack();
        }
        // 剑气(中刺)
        else if (!pg && zc && !xp)
        {
            // 由于这是较远距离发波，主角攻击不到，不做攻击判定
            SwordWind();
        }
        pg = zc = xp = false;
    }
    private void atkkInShortDistance()
    {
        firstLayer = Random.Range(0, 3);
        if ( ( firstLayer == 0 || firstLayer == 1 ) && !xp && !pg && !zc )
            pg = true;
        else if ( firstLayer == 2 && !xp && !pg && !zc )
            xp = true ;
        // 回身斩
        if ( pg && !zc && !xp )
        {
            RoundaboutSlash();
        }
        // 超级多次下劈
        else if ( !pg && !zc && xp )
        {
            FlashAndDown();
        }
        pg = zc = xp = false;
    }
    IEnumerator PauseTime( float time )
    {
        yield return new WaitForSeconds(time);
        isPause = true;
    }

    // 很多种攻击

    // 瞬闪下落攻击N连(下劈)，发动时会闪现到主角的正上方进行下落攻击N次
    private void FlashAndDown()
    {
        animator.SetBool("瞬闪下落", true);
        // 其他操作在动画机行为脚本DoInFlashAndDown
    }

    // 瞬身斩(普攻)，闪现到主角身后一刀
    private void FlashBack() 
    {
        animator.SetBool("瞬身斩", true);
        //// 其他操作在动画机行为脚本DoInFlashBack    
    }

    // 回身斩(普攻)，敌人攻击(往前一下),然后跳起产生向后的位移
    private void RoundaboutSlash()
    {
        animator.SetBool("回身斩", true);
        isRoundaboutSlash = true;
        isNormalAttack = true;
        bossrb.AddForce(Vector2.left * 500f * faceDirection, ForceMode.Impulse);
        // 武器变形
        bossrb.AddForce(Vector2.right * 500f * faceDirection, ForceMode.Impulse);
    }


    // 剑气(中刺),发出剑气，碰到墙或者玩家停下，碰到玩家攻击时对BOSS无影响
    private void SwordWind()
    {
        animator.SetBool("剑气", true);
        Vector3 newPosition = transform.position; 
        newPosition.x += 5;
        newPosition.y -= 3;
        newPosition.z = -1;
        //GameObject swordWind = Instantiate( prefab, newPosition, transform.rotation);
      
        
    }
    //移动函数
    private void Move(float speed)
    {
        if ( canMove )
            transform.position += new Vector3(speed, 0, 0) * Time.fixedDeltaTime;
    }
    public void TurnBack()
    {

        if (transform.position.x < player.transform.position.x)
        {
            backTimer = 0;
            faceDirection = 1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            backTimer = 0;
            faceDirection = -1;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void fixZ()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = -1;
        transform.position = currentPosition;
    }
}
