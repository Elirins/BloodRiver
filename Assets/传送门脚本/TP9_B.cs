using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP9_B : MonoBehaviour
{
    TP9_A tP9_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP9_A = GameObject.Find("¹Ø¿¨ÇÐ»»9_A").GetComponent<TP9_A>();
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
            Vector3 newPosition = tP9_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
