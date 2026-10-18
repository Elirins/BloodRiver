using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public string labelText = "";
    public Color labelColor = Color.red;
    public Vector3 labelOffset = new Vector3(0, 2.5f, 0); // 标签偏移量
    public float doorIstance = 3f; // 能按开门的距离
    public float rotationAngle = 90f;
    public float doorMove = -1f;
    public GameObject player;
    private Vector3 initialPosition; // 门的初始位置
    private Quaternion initialRotation; // 门的初始旋转
    private bool isOpen = false;
    void Start()
    {
        player = GameObject.Find("PlayerO");
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= doorIstance)
        {
            labelText = "按E按使用";
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpen)
                {
                    transform.Rotate(Vector3.right, rotationAngle);
                    transform.Translate(Vector3.back * doorMove);
                    isOpen = true;

                }
                else
                {
                    transform.rotation = initialRotation; // 恢复初始旋转
                    transform.position = initialPosition; // 恢复初始位置
                    isOpen = false;
                }
            }

        }
        else labelText = "";
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

