using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //需求：相机跟随主角移动
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = player.transform.position;
        offset = this.transform.position - player.transform.position ;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + offset ;
    }
}
