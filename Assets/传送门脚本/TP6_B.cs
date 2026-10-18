using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP6_B : MonoBehaviour
{
    TP6_A tP6_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP6_A = GameObject.Find("¹Ø¿¨ÇÐ»»6_A").GetComponent<TP6_A>();
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
            Vector3 newPosition = tP6_A.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
