using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP6_A : MonoBehaviour
{
    TP6_B tP6_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP6_B = GameObject.Find("¹Ø¿¨ÇÐ»»6_B").GetComponent<TP6_B>();
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
            Vector3 newPosition = tP6_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
