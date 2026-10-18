using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP8_A : MonoBehaviour
{
    TP8_B tP8_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP8_B = GameObject.Find("¹Ø¿¨ÇÐ»»8_B").GetComponent<TP8_B>();
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
            Vector3 newPosition = tP8_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
