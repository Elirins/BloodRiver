using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP7_A : MonoBehaviour
{
    TP7_B tP7_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP7_B = GameObject.Find("¹Ø¿¨ÇÐ»»7_B").GetComponent<TP7_B>();
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
            Vector3 newPosition = tP7_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
