using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyUse : MonoBehaviour
{
    public bool isUse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUse)
            gameObject.SetActive(false);
    }
}
