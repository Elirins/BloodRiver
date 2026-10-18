using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP11_B : MonoBehaviour
{
    TP11_A tP11_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP11_A = GameObject.Find("¹Ø¿¨ÇÐ»»11_A").GetComponent<TP11_A>();
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
            Vector3 newPosition = tP11_A.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
