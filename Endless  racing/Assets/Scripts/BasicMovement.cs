using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float movespeed;   // �ƶ��ٶ�
    public float rotateSpeed;  // ��ת�ٶ�
    public bool lamp;  // �Ƿ��ǵƹ⣨���ھ�����ת����

    WorldGenerator generator;  

    Car car;  
    Transform carTransform;  

    void Start()
    {
        car = GameObject.FindObjectOfType<Car>();
        generator = GameObject.FindObjectOfType<WorldGenerator>();

        if (car != null)
            carTransform = car.gameObject.transform;
    }

    void Update()
    {
        // ÿ֡����ǰ����ǰ�ƶ�
        transform.Translate(Vector3.forward * movespeed * Time.deltaTime);

        if (car != null)
            CheckRotate();
    }

    void CheckRotate()
    {
        // �����Ƿ��ǵƹ������ת�ķ���
        // ����ǵƹ⣬��ת����Ϊ�ң�Vector3.right��������Ϊǰ��Vector3.forward��
        Vector3 direction = (lamp) ? Vector3.right : Vector3.forward;

        // ��ȡ��������ת�Ƕȣ�y�ᣩ
        float carRotation = carTransform.localEulerAngles.y;

        // �����������ת�Ƕȳ�����һ��ֵ���˴��ǳ�������ת�Ƕ�*2�����������Ϊ��ֵ
        // ��Ϊ eulerAngles ���ص���һ��ʼ��Ϊ���ĽǶ�ֵ
        if (carRotation > car.rotationAngle * 2f)
            carRotation = (360 - carRotation) * -1f;

        // ������ת������ת�ٶȡ�������ת�ǶȺ�����ĳߴ����������������ת
        // ͨ��ʱ��Time.deltaTime��ʹ����ת����ƽ��
        transform.Rotate(direction * -rotateSpeed * (carRotation / (float)car.rotationAngle) * (36f / generator.dimensions.x) * Time.deltaTime);
    }
}