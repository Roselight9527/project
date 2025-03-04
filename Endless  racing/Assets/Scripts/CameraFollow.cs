using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{ 
	public Transform camTarget;  // ����������Ŀ�꣨ͨ������һ�����

	public float startDelay;  // ��Ϸ��ʼʱ���ӳ�ʱ�䣬�ȴ�һ��ʱ����ٿ�ʼ�л�������Ƕ�
	public float distance = 6.0f;  // �������Ŀ��֮��ľ���
	public float height = 5.0f;  // ������ĸ߶�
	public float heightDamping = 0.5f;  // ������߶ȵ�ƽ������ϵ��
	public float rotationDamping = 1.0f;  // �������ת��ƽ������ϵ��

	// �� Inspector ���ڲ��ɼ��ı���
	float originalRotationDamping;  // ����ԭʼ����תƽ��ϵ���������Ժ�ָ�
	bool canSwitch;  // ���������Ƿ�����л���תƽ��ϵ��

	void Start()
	{
		// ��ȡ��ʼ����תƽ��ϵ��
		originalRotationDamping = rotationDamping;
		// ����תƽ��ϵ������Ϊһ���ǳ�С��ֵ�������������ƽ���ع��ɵ�Ŀ��λ��
		rotationDamping = 0.1f;

		// ���ӳ�һ��ʱ���ʼ�л����������תƽ��ϵ��
		StartCoroutine(SwitchAngle());
	}

	void Update()
	{
		// ����ҵ�һ�ο��Ƴ���ʱ���ָ���������תƽ��ϵ��
		if ((Input.GetMouseButtonDown(0) || Input.GetAxis("Horizontal") != 0) && rotationDamping == 0.1f && canSwitch)
			rotationDamping = originalRotationDamping;
	}

	void LateUpdate()
	{
		// ���û�������������Ŀ�꣬��ֱ�ӷ���
		if (!camTarget)
			return;

		// ˽�б��������ڼ����������ת��λ��
		float wantedRotationAngle = camTarget.eulerAngles.y;  // Ŀ�����ת�Ƕȣ��� Y �ᣩ
		float wantedHeight = camTarget.position.y + height;  // Ŀ��ĸ߶ȣ�����������Ŀ����һ���ĸ߶�
		float currentRotationAngle = transform.eulerAngles.y;  // ��ǰ���������ת�Ƕ�
		float currentHeight = transform.position.y;  // ��ǰ������ĸ߶�

		// ƽ�����ɵ�Ŀ�����ת�Ƕ�
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// ƽ�����ɵ�Ŀ��ĸ߶�
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// ���㵱ǰ����ת��Ԫ��
		Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

		// �����������λ�ã�������ΪĿ��λ��
		transform.position = camTarget.position;
		// ���������λ�ã�ʹ�䱣��һ��������Ŀ���
		transform.position -= currentRotation * Vector3.forward * distance;

		// ����������ĸ߶�
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

		// �����������Ŀ��
		transform.LookAt(camTarget);
	}

	// �л���תƽ��ϵ����Э�̷���
	IEnumerator SwitchAngle()
	{
		// �ȴ�ָ����ʱ����л���תƽ��ϵ��
		yield return new WaitForSeconds(startDelay);

		// ���ÿ����л�ƽ��ϵ��
		canSwitch = true;
	}
}
