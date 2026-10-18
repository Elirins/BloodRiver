using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在神龛上 需要collider
public class ShrineTeach : MonoBehaviour
{
    public GameObject shrineTeach;
    private bool weatherFirstApproach = true;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player" && weatherFirstApproach == true)
        {
            //Time.timeScale = 0;
            shrineTeach.SetActive(true);
            weatherFirstApproach = false;
        }
    }
}
