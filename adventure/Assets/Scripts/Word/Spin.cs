using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpinAroundPoint : MonoBehaviour
{
    public float rotationSpeed = 100f; // 自转速度（度/秒）

    void Update()
    {
        // 物体围绕自身中心自转，旋转角度随时间变化
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);  // Z轴旋转
    }
}