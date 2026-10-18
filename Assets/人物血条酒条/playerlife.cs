using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;//加载场景

public class playerlife : MonoBehaviour
{
    public Healthbar healthBar;
    public float maxhealth = 20;
    public float wine=20;
    public float  currenthealth;
    public float recoveryspeed=1f;
    public bool isdead = false;
    public bool isharm = false;
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler OnPlayerDeath;

    private Animator anim;
    private Rigidbody2D rb;
    private float regenTimer;

    void Start()
    {
        currenthealth = maxhealth-15;
        healthBar.SetMaxHealth(maxhealth);//设置最大血量和当前血量
        healthBar.SetHealth(currenthealth);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        regenTimer = 3f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="enemy")
        {
            TakeHarm(2);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)&&!isdead)
        {
            isharm = false;
            while (regenTimer >= 0f)
            {
                Takecure();
                regenTimer -= Time.deltaTime;
                if (isharm)
                {
                    isharm =false;
                    regenTimer = 3f;
                    break;
                }
            }
            if(regenTimer<0f)
            {
                regenTimer=3f;
            }
        }
    }
    void Takecure()
    {
        if (currenthealth < maxhealth)
        {
            currenthealth += recoveryspeed * Time.deltaTime*0.1f;
            healthBar.SetHealth(currenthealth);
        }
        else return;
    }
    void TakeHarm(float harm)
    {
        isharm = true;
        regenTimer = -1f;
        currenthealth -= harm;
        healthBar.SetHealth(currenthealth);
        if(currenthealth<=0)
        {
            isdead = true;
            rb.bodyType = RigidbodyType2D.Static;//改变刚体类型
            anim.SetTrigger("death");//animator中使用trigger类型，命名为death。
            Restart();
            OnPlayerDeath?.Invoke();
            //Invoke("Restart", 1f);
           
        }
    }
    private void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//重置场景（全部刷新）
        rb.bodyType = RigidbodyType2D.Dynamic ;
        currenthealth = maxhealth;
        healthBar.SetHealth(currenthealth);
        isdead = false;
    }
}
//酒量和血量要存在一个换算机制，根据酒量和时间恢复血量,我的代码中有按一次E回血的时间这个可以
//当我知道换算比例就可以通过·当前血量设置出一个合适的恢复时间。
//回血是否要有cd,这里我没测试出来到底受到伤害后回血能不能被打断，按照代码来说应该是没问题的。
//此脚本包含的是掉血（伤害判定很粗糙）回血，死亡，监听主角死亡,重置场景