using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP12_A : MonoBehaviour
{
    TP12_B tP12_B;
    BoxCollider boxCollider;
    // 需要“锁着的木门"打开时才可以传送
    // Start is called before the first frame update
    void Start()
    {
        tP12_B = GameObject.Find("关卡切换12_B").GetComponent<TP12_B>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 newPosition = tP12_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
