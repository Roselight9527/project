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
        virtualCamer = GameObject.FindGameObjectWithTag("VirtualCamer");//���ұ�ǩΪ"VirtualCamer"�Ķ��󲢸�ֵ
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;//ͨ��SpriteRenderer���ͼ����
        totalWidth = mapWidth * mapNums;//��ͼ�ܿ��
    }
    private void Update()
    {
        Vector3 tempPosition = transform.position;
        if (virtualCamer.transform.position.x > transform.position.x + totalWidth / 2)
        {
            tempPosition.x += totalWidth;//��ͼ����ƽ��һ��������ͼ���
            transform.position = tempPosition;
        }
        else if (virtualCamer.transform.position.x < transform.position.x - totalWidth / 2)
        {
            tempPosition.x -= totalWidth;//��ͼ����ƽ��һ��������ͼ���
            transform.position = tempPosition;
        }
    }
}
