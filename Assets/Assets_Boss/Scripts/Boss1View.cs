using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1View : MonoBehaviour
{
    BossMove bossMove;
    // Start is called before the first frame update
    void Start()
    {
        bossMove = transform.parent.GetComponent<BossMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player" )
        {
            bossMove.boss1State = State.Alarmed;
            bossMove.distance = Mathf.Abs(bossMove.transform.position.x - other.transform.position.x) ;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bossMove.boss1State =  State.Idle;
            bossMove.distance = 0;
        }
    }
}
