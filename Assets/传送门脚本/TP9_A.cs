using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP9_A : MonoBehaviour
{
    TP9_B tP9_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP9_B = GameObject.Find("¹Ø¿¨ÇÐ»»9_B").GetComponent<TP9_B>();
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
            Vector3 newPosition = tP9_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
