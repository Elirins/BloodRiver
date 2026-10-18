using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP13_B : MonoBehaviour
{
    TP13_A tP13_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP13_A = GameObject.Find("¹Ø¿¨ÇÐ»»13_A").GetComponent<TP13_A>();
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
            Vector3 newPosition = tP13_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
