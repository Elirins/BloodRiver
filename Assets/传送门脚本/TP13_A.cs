using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP13_A : MonoBehaviour
{
    TP13_B tP13_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP13_B = GameObject.Find("¹Ø¿¨ÇÐ»»13_B").GetComponent<TP13_B>();
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
            Vector3 newPosition = tP13_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
