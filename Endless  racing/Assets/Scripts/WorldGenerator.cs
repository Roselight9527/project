using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldGenerator : MonoBehaviour
{

	public Material meshMaterial;  // �������
	public float scale;  // �������
	public Vector2 dimensions;  // ����ĳߴ磨x: ����, y: ����
	public float perlinScale;  // �������������ı���
	public float waveHeight;  // �����߶�
	public float offset;  // ����ƫ����
	public float randomness;  // �����
	public float globalSpeed;  // �����ƶ����ٶ�
	public int startTransitionLength;  // ��ʼ���ɵĳ���
	public BasicMovement lampMovement;  // �ƹ⣨����⣩���˶�
	public GameObject[] obstacles;  // �ϰ�������
	public GameObject gate;  // ���Ŷ���
	public int startObstacleChance;  // ��ʼʱ�����ϰ���ĸ���
	public int obstacleChanceAcceleration;  // �ϰ�����ʼ���
	public int gateChance;  // �������ɸ���
	public int showItemDistance;  // ��ʾ��Ʒ�ľ���
	public float shadowHeight;  // ��Ӱ�߶�

	Vector3[] beginPoints;  // ��������ֵ���ʼ�㣬���ڹ���Ч��

	GameObject[] pieces = new GameObject[2];  // �洢�������粿��
	GameObject currentCylinder;  // ��ǰ���ɵ����粿�֣�Բ���壩

	void Start()
	{
		// �������飬���ڴ洢ÿ�����粿�ֵ���ʼ���㣨������ȷ����)
		beginPoints = new Vector3[(int)dimensions.x + 1];

		// �������������粿��
		for (int i = 0; i < 2; i++)
		{
			GenerateWorldPiece(i);
		}
	}

	void LateUpdate()
	{
		// ����ڶ��������Ѿ��ӽ���ң��Ƴ���һ�����ֲ���������
		if (pieces[1] && pieces[1].transform.position.z <= -15)
			StartCoroutine(UpdateWorldPieces());

		// ���³����е�������Ʒ�����ϰ���ʹ���
		UpdateAllItems();
	}

	void UpdateAllItems()
	{
		// �������д��� "Item" ��ǩ����Ʒ
		GameObject[] items = GameObject.FindGameObjectsWithTag("item");

		// ��������Ʒ���в���
		for (int i = 0; i < items.Length; i++)
		{
			// ��ȡ��Ʒ������ MeshRenderer
			foreach (MeshRenderer renderer in items[i].GetComponentsInChildren<MeshRenderer>())
			{
				// �����Ʒ��������㹻��������ʾ����Ʒ
				bool show = items[i].transform.position.z < showItemDistance;

				// �����Ҫ��ʾ��Ʒ����������ӰͶ��ģʽ
				// ����������Բ���εģ�ֻ�еװ벿�ֵ�������Ҫ��Ӱ
				if (show)
					renderer.shadowCastingMode = (items[i].transform.position.y < shadowHeight) ? ShadowCastingMode.On : ShadowCastingMode.Off;

				// ֻ������Ҫ��ʾ��Ʒʱ����������Ⱦ��
				renderer.enabled = show;
			}
		}
	}

	void GenerateWorldPiece(int i)
	{
		// ����һ���µ����粿�ֲ��������������
		pieces[i] = CreateCylinder();
		// ��������λ�õ������粿�ֵ�λ��
		pieces[i].transform.Translate(Vector3.forward * (dimensions.y * scale * Mathf.PI) * i);

		// ���´˲��֣�ʹ������յ㲢�ܹ��ƶ���
		UpdateSinglePiece(pieces[i]);
	}

	IEnumerator UpdateWorldPieces()
	{
		// �Ƴ���һ�����֣��������ٶ���ҿɼ�ʱ��
		Destroy(pieces[0]);

		// ���ڶ����ָ�ֵ����һ������
		pieces[0] = pieces[1];

		// ����һ���µĵڶ�����
		pieces[1] = CreateCylinder();

		// �����²��ֵ�λ�ò���ת�����һ�����ֶ���
		pieces[1].transform.position = pieces[0].transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
		pieces[1].transform.rotation = pieces[0].transform.rotation;

		// ���������ɵ����粿��
		UpdateSinglePiece(pieces[1]);

		// �ȴ�һ֡
		yield return 0;
	}

	void UpdateSinglePiece(GameObject piece)
	{
		// �������ɵĲ�����ӻ����˶��ű���ʹ�䳯������ƶ�
		BasicMovement movement = piece.AddComponent<BasicMovement>();
		// �������ƶ��ٶ�Ϊ globalSpeed��������ʾ����ҷ����ƶ���
		movement.movespeed = -globalSpeed;

		// ������ת�ٶ�Ϊ�ƹ⣨����⣩����ת�ٶ�
		if (lampMovement != null)
			movement.rotateSpeed = lampMovement.rotateSpeed;

		// Ϊ�˲��ִ���һ���յ�
		GameObject endPoint = new GameObject();
		endPoint.transform.position = piece.transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
		endPoint.transform.parent = piece.transform;
		endPoint.name = "End Point";

		// �ı� Perlin ������ƫ��������ȷ��ÿ�����粿������һ����ͬ
		offset += randomness;

		// �ı��ϰ������ɵĸ��ʣ�����ʱ��������ϰ�������
		if (startObstacleChance > 5)
			startObstacleChance -= obstacleChanceAcceleration;
	}

	public GameObject CreateCylinder()
	{
		// �������粿�ֵĻ�����������
		GameObject newCylinder = new GameObject();
		newCylinder.name = "World piece";

		// ���õ�ǰԲ����Ϊ�´����Ķ���
		currentCylinder = newCylinder;

		// ���²������ MeshFilter �� MeshRenderer ���
		MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();

		// ���²������ò���
		meshRenderer.material = meshMaterial;
		// �������񲢸�ֵ�� MeshFilter
		meshFilter.mesh = Generate();

		// ���������ƥ��� MeshCollider ���
		newCylinder.AddComponent<MeshCollider>();

		return newCylinder;
	}

	// ���ɲ����������粿�ֵ�����
	Mesh Generate()
	{
		// ����������������
		Mesh mesh = new Mesh();
		mesh.name = "MESH";

		// �����������洢���㡢UV �����������
		Vector3[] vertices = null;
		Vector2[] uvs = null;
		int[] triangles = null;

		// ����������״���������
		CreateShape(ref vertices, ref uvs, ref triangles);

		// ������ֵ
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		// ���¼��㷨��
		mesh.RecalculateNormals();

		return mesh;
	}

	void CreateShape(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles)
	{
		// ��ȡ�ò����� x �� z ��Ĵ�С
		int xCount = (int)dimensions.x;  // �� x ���Ϸָ�Ķ�������
		int zCount = (int)dimensions.y;  // �� z ���Ϸָ�Ķ�������

		// ��ʼ������� UV ����
		vertices = new Vector3[(xCount + 1) * (zCount + 1)];
		uvs = new Vector2[(xCount + 1) * (zCount + 1)];

		int index = 0;

		// ��ȡԲ����İ뾶
		float radius = xCount * scale * 0.5f;  // Բ���İ뾶

		// ˫��ѭ������ x �� z ������ж���
		for (int x = 0; x <= xCount; x++)
		{
			for (int z = 0; z <= zCount; z++)
			{
				// ��ȡԲ����ĽǶȣ�����ȷ���ö���λ��
				float angle = x * Mathf.PI * 2f / xCount;

				// ʹ�ýǶȵ����Һ�����ֵ�����ö���
				vertices[index] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z * scale * Mathf.PI);

				// ���� UV ����
				uvs[index] = new Vector2(x * scale, z * scale);

				// ʹ�� Perlin �������� X �� Z ֵ
				float pX = (vertices[index].x * perlinScale) + offset;
				float pZ = (vertices[index].z * perlinScale) + offset;

				// �������ƶ�������λ�ã����� z ���꣩��ʹ�� Perlin ��������λ��
				Vector3 center = new Vector3(0, 0, vertices[index].z);
				vertices[index] += (center - vertices[index]).normalized * Mathf.PerlinNoise(pX, pZ) * waveHeight;

				// �������粿��֮���ƽ������
				if (z < startTransitionLength && beginPoints[0] != Vector3.zero)
				{
					// ����ǹ��ɲ��֣���� Perlin ��������һ�����ֵ���ʼ��
					float perlinPercentage = z * (1f / startTransitionLength);
					Vector3 beginPoint = new Vector3(beginPoints[x].x, beginPoints[x].y, vertices[index].z);
					vertices[index] = (perlinPercentage * vertices[index]) + ((1f - perlinPercentage) * beginPoint);
				}
				else if (z == zCount)
				{
					// ������ʼ�㣬��ȷ����һ���ֵ�ƽ������
					beginPoints[x] = vertices[index];
				}

				// �����λ��������Ʒ
				if (Random.Range(0, startObstacleChance) == 0 && !(gate == null && obstacles.Length == 0))
					CreateItem(vertices[index], x);

				// ���Ӷ�������
				index++;
			}
		}

		// ��ʼ������������
		triangles = new int[xCount * zCount * 6];  // ÿ�������� 2 �������Σ�ÿ���������� 3 ��������ɣ��� 6 ������

		// ����ÿ������Ļ����������ε���ɸ��򵥣�
		int[] boxBase = new int[6];  // ÿ������������ 6 ��������ɣ����������Σ�

		int current = 0;

		// ���� x ���ϵ�����λ��
		for (int x = 0; x < xCount; x++)
		{
			boxBase = new int[]{
				x * (zCount + 1),
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),
				x * (zCount + 1) + 1,
				(x + 1) * (zCount + 1) + 1,
				(x + 1) * (zCount + 1),
			};

			// ���� z ���ϵ�����λ��
			for (int z = 0; z < zCount; z++)
			{
				// ���Ӷ�������������������
				for (int i = 0; i < 6; i++)
				{
					boxBase[i] = boxBase[i] + 1;
				}

				// ʹ�������������������
				for (int j = 0; j < 6; j++)
				{
					triangles[current + j] = boxBase[j] - 1;
				}

				// ���ӵ�ǰ����
				current += 6;
			}
		}
	}

	void CreateItem(Vector3 vert, int x)
	{
		// ��ȡԲ���������λ�ã���ʹ�ö���� z ����
		Vector3 zCenter = new Vector3(0, 0, vert.z);

		// ���������Ʒ����ȷλ��
		if (zCenter - vert == Vector3.zero || x == (int)dimensions.x / 4 || x == (int)dimensions.x / 4 * 3)
			return;

		// �������һ����Ʒ�����Ż��ϰ��
		GameObject newItem = Instantiate((Random.Range(0, gateChance) == 0) ? gate : obstacles[Random.Range(0, obstacles.Length)]);

		// ��ת��Ʒʹ�䳯������
		newItem.transform.rotation = Quaternion.LookRotation(zCenter - vert, Vector3.up);
		// ������Ʒλ��
		newItem.transform.position = vert;

		// ����Ʒ��Ϊ��ǰԲ����������壬ȷ����������һ���ƶ�
		newItem.transform.SetParent(currentCylinder.transform, false);
	}

	public Transform GetWorldPiece()
	{
		// ���ص�һ�����粿�ֵ� Transform
		return pieces[0].transform;
	}

}

