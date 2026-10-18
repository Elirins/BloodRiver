using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP5_A : MonoBehaviour
{
    TP5_B tP5_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP5_B = GameObject.Find("¹Ø¿¨ÇÐ»»5_B").GetComponent<TP5_B>();
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
            Vector3 newPosition = tP5_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
