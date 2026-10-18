using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float flashTime;
    private SpriteRenderer sr;
    private Color originalColor;
    public int health;
    private GameObject player;
    private bool firstTakeDamage;
    public GameObject wineTeach;
    private bool weatherBeHitToMove;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        health = 100;
        flashTime = 0.5f;
        player = GameObject.Find("PlayerO");
        firstTakeDamage = true;
        weatherBeHitToMove = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (health <= 0)
        //{
        //    Destroy(gameObject);
        //}

        BeHitToMove();

    }

    public void PlayerTakeDamage(int damage)
    {
        if (firstTakeDamage)//在Test场景下启用
        {
            Time.timeScale = 0;
            wineTeach.SetActive(true);
            firstTakeDamage = false;
        }
        health -= damage;
        FlashColor(flashTime);
        weatherBeHitToMove = true;
        //gameObject.GetComponent<TestPlayer>().enabled = false;//受伤时硬直不可操控
        //player.GetComponent<CameraShakeRequester>().RequestShake();//Test场景关闭
    }
    private void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        sr.color = originalColor;
        weatherBeHitToMove = false;
        //gameObject.GetComponent<TestPlayer>().enabled = true;
    }

    private void BeHitToMove()
    {
        if (weatherBeHitToMove && wineTeach.activeSelf == false) transform.position += new Vector3(0.05f, 0, 0) * -gameObject.GetComponent<TestPlayer>().faceDirection * Time.fixedDeltaTime;
    }
}
