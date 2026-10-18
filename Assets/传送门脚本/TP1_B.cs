using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP1_B : MonoBehaviour
{
    TP1_A tP1_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP1_A = GameObject.Find("¹Ø¿¨ÇÐ»»1_A").GetComponent<TP1_A>();
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
            Vector3 newPosition = tP1_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
