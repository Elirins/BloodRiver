using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP4_B : MonoBehaviour
{
    TP4_A tP4_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP4_A = GameObject.Find("¹Ø¿¨ÇÐ»»4_A").GetComponent<TP4_A>();
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
            Vector3 newPosition = tP4_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
