using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP11_A : MonoBehaviour
{
    TP11_B tP11_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP11_B = GameObject.Find("¹Ø¿¨ÇÐ»»11_B").GetComponent<TP11_B>();
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
            Vector3 newPosition = tP11_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
