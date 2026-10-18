using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP14_B : MonoBehaviour
{
    TP14_A tP14_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP14_A = GameObject.Find("¹Ø¿¨ÇÐ»»14_A").GetComponent<TP14_A>();
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
            Vector3 newPosition = tP14_A.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
