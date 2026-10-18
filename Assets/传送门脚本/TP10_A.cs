using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP10_A : MonoBehaviour
{
    TP10_B tP10_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP10_B = GameObject.Find("¹Ø¿¨ÇÐ»»10_B").GetComponent<TP10_B>();
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
            Vector3 newPosition = tP10_B.transform.position;
            newPosition.x += 1f;
            other.transform.position = newPosition;
        }
    }
}
