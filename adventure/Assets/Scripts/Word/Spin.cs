using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpinAroundPoint : MonoBehaviour
{
    public float rotationSpeed = 100f; // ��ת�ٶȣ���/�룩

    void Update()
    {
        // ����Χ������������ת����ת�Ƕ���ʱ��仯
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);  // Z����ת
    }
}