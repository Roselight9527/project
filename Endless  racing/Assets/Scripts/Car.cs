using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	public Rigidbody rb;  // �����ĸ��壬��������ģ��

	public Transform[] wheelMeshes;  // ��������������ģ�ͣ�
	public WheelCollider[] wheelColliders;  // ������������ײ������������ģ�⣩

	public int rotateSpeed;  // ������ת���ٶ�
	public int rotationAngle;  // ÿ����ת�ĽǶ�
	public int wheelRotateSpeed;  // ���ӵ���ת�ٶ�

	public Transform[] grassEffects;  // �ݵ���Ч�������ӽӴ��ݵ�ʱ��ʾ��
	public Transform[] skidMarkPivots;  // ���۵�����λ��
	public float grassEffectOffset;  // �ݵ���Ч��ƫ����

	public Transform back;  // �����󷽵�λ�ã�����ʩ���ȶ���������
	public float constantBackForce;  // ������ʩ�ӵĳ���������

	public GameObject skidMark;  // ����Ԥ��
	public float skidMarkSize;  // ���۵Ĵ�С
	public float skidMarkDelay;  // �������ɵ��ӳ�ʱ��
	public float minRotationDifference;  // ��С��ת���죬���ڼ���Ƿ����㹻����ת�����ɻ���

	public GameObject ragdoll;  // ������ ragdoll ������󣨵������ƻ�ʱʹ�ã�

	// �� Inspector ���ڲ��ɼ��ı���
	int targetRotation;  // Ŀ����ת�Ƕ�
	WorldGenerator generator;  // ���������������ڻ�ȡ�����������

	float lastRotation;  // ��һ֡����ת�Ƕ�
	bool skidMarkRoutine;  // �Ƿ��������ɻ��۵ı�־

	void Start()
	{
		// ����������������������������Э��
		generator = GameObject.FindObjectOfType<WorldGenerator>();
		StartCoroutine(SkidMark());
	}

	void FixedUpdate()
	{
		// ���»��ۺͲݵ���Ч
		UpdateEffects();
	}

	void LateUpdate()
	{
		// �����������ӵ�����λ�ú���ת
		for (int i = 0; i < wheelMeshes.Length; i++)
		{
			// ��ȡ������ײ��������λ�ú���ת
			Quaternion quat;
			Vector3 pos;

			wheelColliders[i].GetWorldPose(out pos, out quat);

			// �������������λ��
			wheelMeshes[i].position = pos;

			// ��ת���ӣ�ʹ�俴��������������ʻ
			wheelMeshes[i].Rotate(Vector3.right * Time.deltaTime * wheelRotateSpeed);
		}

		// ��������Ҫת����ת����
		if (Input.GetMouseButton(0) || Input.GetAxis("Horizontal") != 0)
		{
			UpdateTargetRotation();
		}
		else if (targetRotation != 0)
		{
			// ������ת�ص�����λ��
			targetRotation = 0;
		}

		// ����Ŀ��ǶȽ�����ת
		Vector3 rotation = new Vector3(transform.localEulerAngles.x, targetRotation, transform.localEulerAngles.z);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotation), rotateSpeed * Time.deltaTime);
	}

	void UpdateTargetRotation()
	{
		// ������������ת
		if (Input.GetAxis("Horizontal") == 0)
		{
			// ��ȡ����λ�ã���Ļ����λ�ã�
			if (Input.mousePosition.x > Screen.width * 0.5f)
			{
				// ������ת
				targetRotation = rotationAngle;
			}
			else
			{
				// ������ת
				targetRotation = -rotationAngle;
			}
		}
		else
		{
			// ��������˷�������� a/d ��������ˮƽ������ת����
			targetRotation = (int)(rotationAngle * Input.GetAxis("Horizontal"));
		}
	}

	void UpdateEffects()
	{
		// ����������ֶ����Ӵ����棬addForce Ϊ true
		bool addForce = true;
		// ��鳵������ת�Ƿ����˱仯
		bool rotated = Mathf.Abs(lastRotation - transform.localEulerAngles.y) > minRotationDifference;

		// ������ֵĲݵ���Ч
		for (int i = 0; i < 2; i++)
		{
			// ��ȡ���֣�ÿ�ε���ѡ��һ�����֣�
			Transform wheelMesh = wheelMeshes[i + 2];

			// ��鵱ǰ�����Ƿ�Ӵ�����
			if (Physics.Raycast(wheelMesh.position, Vector3.down, grassEffectOffset * 1.5f))
			{
				// ����Ӵ����棬��ʾ�ݵ���Ч
				if (!grassEffects[i].gameObject.activeSelf)
					grassEffects[i].gameObject.SetActive(true);

				// ���²ݵ���Ч�ĸ߶Ⱥͻ��۵ĸ߶ȣ�ʹ��������ͬ��
				/*float effectHeight = wheelMesh.position.y - grassEffectOffset;
				Vector3 targetPosition = new Vector3(grassEffects[i].position.x, effectHeight, wheelMesh.position.z);
				grassEffects[i].position = targetPosition;
				skidMarkPivots[i].position = targetPosition;*/

				// ������ӽӴ����棬����Ҫʩ�Ӷ���������
				addForce = false;
			}
			else if (grassEffects[i].gameObject.activeSelf)
			{
				// �������û�нӴ����棬�����زݵ���Ч
				grassEffects[i].gameObject.SetActive(false);
			}
		}

		// ����������ֶ����Ӵ����棬ʩ�����µ��ȶ���
		if (addForce)
		{
			rb.AddForceAtPosition(back.position, Vector3.down * constantBackForce);
			// ����ʾ����
			skidMarkRoutine = false;
		}
		else
		{
			if (targetRotation != 0)
			{
				// �������������ת����ʾ����
				if (rotated && !skidMarkRoutine)
				{
					skidMarkRoutine = true;
				}
				else if (!rotated && skidMarkRoutine)
				{
					skidMarkRoutine = false;
				}
			}
			else
			{
				// �������������ת������λ�ã�����ʾ����
				skidMarkRoutine = false;
			}
		}

		// �������һ����ת�Ƕ�
		lastRotation = transform.localEulerAngles.y;
	}

	// �����ݻ�ʱ���ã����� ragdoll �����ó�������
	public void FallApart()
	{
		// ���� ragdoll �������
		Instantiate(ragdoll, transform.position, transform.rotation);
		// ���õ�ǰ��������
		gameObject.SetActive(false);
	}

	// ��������Э��
	IEnumerator SkidMark()
	{
		// ����ѭ�����ɻ���
		while (true)
		{
			// �ȴ��������ɵ��ӳ�ʱ��
			yield return new WaitForSeconds(skidMarkDelay);

			// �����Ҫ���ɻ���
			if (skidMarkRoutine)
			{
				// Ϊ�������ɻ��۲����丽�ӵ�������
				for (int i = 0; i < skidMarkPivots.Length; i++)
				{
					// ʵ�������۶���
					GameObject newskidMark = Instantiate(skidMark, skidMarkPivots[i].position, skidMarkPivots[i].rotation);
					// �����۸��ӵ�������������һ���������
					newskidMark.transform.parent = generator.GetWorldPiece();
					// ���û��۵Ĵ�С
					newskidMark.transform.localScale = new Vector3(1, 1, 4) * skidMarkSize;
				}
		    }
		}
	}
}
