using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP4_A : MonoBehaviour
{
    TP4_B tP4_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP4_B = GameObject.Find("¹Ø¿¨ÇÐ»»4_B").GetComponent<TP4_B>();
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
            Vector3 newPosition = tP4_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
