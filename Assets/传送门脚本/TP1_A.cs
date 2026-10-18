using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP1_A : MonoBehaviour
{
    TP1_B tP1_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP1_B = GameObject.Find("¹Ø¿¨ÇÐ»»1_B").GetComponent<TP1_B>();
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
            Vector3 newPosition = tP1_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
