using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP5_B : MonoBehaviour
{
    TP5_A tP5_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP5_A = GameObject.Find("¹Ø¿¨ÇÐ»»5_A").GetComponent<TP5_A>();
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
            Vector3 newPosition = tP5_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
