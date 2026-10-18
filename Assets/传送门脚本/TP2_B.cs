using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP2_B : MonoBehaviour
{
    TP2_A tP2_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP2_A = GameObject.Find("¹Ø¿¨ÇÐ»»2_A").GetComponent<TP2_A>();
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
            Vector3 newPosition = tP2_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
