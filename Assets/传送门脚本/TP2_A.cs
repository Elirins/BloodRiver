using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP2_A : MonoBehaviour
{
    TP2_B tP2_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP2_B = GameObject.Find("¹Ø¿¨ÇÐ»»2_B").GetComponent<TP2_B>();
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
            Vector3 newPosition = tP2_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
