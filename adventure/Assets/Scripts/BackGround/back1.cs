using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back1 : MonoBehaviour
{
    public GameObject virtualCamer;
    public float mapWidth;
    public int mapNums;
    private float totalWidth;
    private void Start()
    {
        virtualCamer = GameObject.FindGameObjectWithTag("VirtualCamer");//查找标签为"VirtualCamer"的对象并赋值
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;//通过SpriteRenderer获得图像宽度
        totalWidth = mapWidth * mapNums;//地图总宽度
    }
    private void Update()
    {
        Vector3 tempPosition = transform.position;
        if (virtualCamer.transform.position.x > transform.position.x + totalWidth / 2)
        {
            tempPosition.x += totalWidth;//地图向右平移一个完整地图宽度
            transform.position = tempPosition;
        }
        else if (virtualCamer.transform.position.x < transform.position.x - totalWidth / 2)
        {
            tempPosition.x -= totalWidth;//地图向左平移一个完整地图宽度
            transform.position = tempPosition;
        }
    }
}
