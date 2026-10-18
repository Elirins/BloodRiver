using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP8_B : MonoBehaviour
{
    TP8_A tP8_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP8_A = GameObject.Find("¹Ø¿¨ÇÐ»»8_A").GetComponent<TP8_A>();
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
            Vector3 newPosition = tP8_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
