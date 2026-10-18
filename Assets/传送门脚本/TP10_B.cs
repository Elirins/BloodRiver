using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP10_B : MonoBehaviour
{
    TP10_A tP10_A;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP10_A = GameObject.Find("¹Ø¿¨ÇÐ»»10_A").GetComponent<TP10_A>();
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
            Vector3 newPosition = tP10_A.transform.position;
            newPosition.x -= 1f;
            other.transform.position = newPosition;
        }
    }
}
