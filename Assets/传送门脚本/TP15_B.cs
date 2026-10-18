using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP15_B : MonoBehaviour
{
    TP15_A tP15_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP15_A = GameObject.Find("¹Ø¿¨ÇÐ»»15_A").GetComponent<TP15_A>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 newPosition = tP15_A.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
