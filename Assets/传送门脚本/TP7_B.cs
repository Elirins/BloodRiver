using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP7_B : MonoBehaviour
{
    TP7_A tP7_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP7_A = GameObject.Find("¹Ø¿¨ÇÐ»»7_A").GetComponent<TP7_A>();
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
            Vector3 newPosition = tP7_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
