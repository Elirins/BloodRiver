using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在宝物上 需要collider
public class GetTreasure : MonoBehaviour
{
    public GameObject treasure;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 0;
            treasure.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
