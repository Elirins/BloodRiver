using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP12_B : MonoBehaviour
{
    TP12_A tP12_A;
    BoxCollider boxCollider;
    // 这里要与木门有关
    // Start is called before the first frame update
    void Start()
    {
        tP12_A = GameObject.Find("关卡切换12_A").GetComponent<TP12_A>();
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
            Vector3 newPosition = tP12_A.transform.position;
            other.transform.position = newPosition;
        }
    }
}
