using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP3_A : MonoBehaviour
{
    GameObject tP3_B;
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tP3_B = GameObject.Find("¹Ø¿¨ÇÐ»»3_B");
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
            other.transform.position = tP3_B.transform.position ;
        }
    }
}
