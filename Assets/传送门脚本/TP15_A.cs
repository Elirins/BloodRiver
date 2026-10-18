using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP15_A : MonoBehaviour
{
    TP15_B tP15_B;
    BossMove boss1;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP15_B = GameObject.Find("¹Ø¿¨ÇÐ»»15_B").GetComponent<TP15_B>();
        boss1 = GameObject.Find("Boss1").GetComponent<BossMove>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss1.enemyHP <= 0)
            boxCollider.isTrigger = true;
        else boxCollider.isTrigger = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player" )
        {
            Vector3 newPosition = tP15_B.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
