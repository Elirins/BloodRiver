using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowName : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isActive = false;
    private bool canUse = true;
    public string labelText = "神龛";
    public Color labelColor = Color.red;
    public Vector3 labelOffset = new Vector3(-0.57f, 2.5f, 0) ; // 标签偏移量
    public float maxHP = 100f;
    public PlayerMove playerMove;
    void Start()
    {
        
    }
    void Update()
    {
        if ( isActive && canUse )
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canUse = false;
                playerMove.isShrine = true;
                playerMove.RestoreBear(); // 回酒回血
                playerMove.playerHP = playerMove.maxHP ;
            }
            labelText = "按E回血";
            
        }
        else labelText = "神龛";
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player" )
        {
            playerMove = GameObject.Find("PlayerO").GetComponent<PlayerMove>() ;
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerMove = null;
            isActive = false;
        }
    }
    private void OnGUI()
    {
        // 将物体的世界坐标转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + labelOffset);

        // 绘制标签
        GUIStyle style = new GUIStyle(GUI.skin.label); // 新建一个风格
        style.normal.textColor = labelColor; // 设置标签的颜色

        // 绘制标签
        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 100, 20), labelText, style);
    }

    

}
