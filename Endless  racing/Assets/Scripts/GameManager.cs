using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Text scoreLabel;  // ��ʾ��ǰ�������ı�
	public Text timeLabel;  // ��ʾ��ʱ���ı�
	public Text gameOverScoreLabel;  // ��Ϸ����ʱ��ʾ�������ı�
	public Text gameOverBestLabel;  // ��Ϸ����ʱ��ʾ��߷ֵ��ı�
	public Animator scoreEffect;  // �����仯ʱ�Ķ���Ч��
	public Animator UIAnimator;  // UI ����������
	public Animator gameOverAnimator;  // ��Ϸ��������������
	public AudioSource gameOverAudio;  // ��Ϸ����ʱ����Ч
	public Car car;  // ��ҿ��Ƶĳ�
	float time;  // ��Ϸ���е�ʱ��
	int score=0;  // ��ǰ����

	bool gameOver;  // �����Ϸ�Ƿ����

	void Start()
	{
		// ��ʼ��ʱ��ʾ����Ϊ 0
		UpdateScore(0);
	}

	void Update()
	{
		// ���µ�ǰʱ����ʾ
		UpdateTimer();

		// �����Ϸ��������Ұ��»س�������������¿�ʼ��Ϸ
		if (gameOver && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
		{
			//UIAnimator.SetTrigger("Start");
			StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
		}
	}
    private void OnDisable()
    {
		score = 0;
		PlayerPrefs.SetInt("best", score);
	}
    void UpdateTimer()
	{
		// ������Ϸʱ��
		time += Time.deltaTime;
		int timer = (int)time;

		// ������Ӻ�����
		int seconds = timer % 60;
		int minutes = timer / 60;

		// ��ʽ��ʱ���ַ�����ȷ�������ͷ�����������λ��
		string secondsRounded = ((seconds < 10) ? "0" : "") + seconds;
		string minutesRounded = ((minutes < 10) ? "0" : "") + minutes;

		// ��ʾʱ��
		timeLabel.text = minutesRounded + ":" + secondsRounded;
	}

	public void UpdateScore(int points)
	{
		// ���ӷ���
		score += points;

		// ���·�����ʾ
		scoreLabel.text = "" + score;

		// ����е÷֣�������������Ч��
		/*if (points != 0)
			scoreEffect.SetTrigger("Score");*/
	}

	public void GameOver()
	{
		// �����Ϸ�Ѿ�����������ִ��
		if (gameOver)
			return;

		// ���µ�ǰ��������߷���
		SetScore();

		// ������Ϸ������������Ч
		gameOverAnimator.SetTrigger("Game Over");
		//gameOverAudio.Play();

		// ������Ϸ������־
		gameOver = true;

		// �ó��ƻ����ݻٳ�����
		car.FallApart();

		// ֹͣ�����е������ƶ�����ת����ͣ�������ɵ������˶���
		foreach (BasicMovement basicMovement in GameObject.FindObjectsOfType<BasicMovement>())
		{
			basicMovement.movespeed = 0;  // ֹͣ�ƶ�
			basicMovement.rotateSpeed = 0;  // ֹͣ��ת
		}
	}

	void SetScore()
	{
		// �����ǰ������֮ǰ����߷ָߣ��������߷�
		if (score > PlayerPrefs.GetInt("best"))
			PlayerPrefs.SetInt("best", score);

		// ��ʾ��ǰ��������߷���
		gameOverScoreLabel.text = "score: " + score;
		gameOverBestLabel.text = "best: " + PlayerPrefs.GetInt("best");
	}
	
	// �ȴ�����һ���Ӻ���ظ����ĳ���
	IEnumerator LoadScene(string scene)
	{
		// �ȴ�һ��ʱ���ټ��س�����������Ϸ������Ĺ��ɶ�����
		yield return new WaitForSeconds(0.6f);

		// ����ָ���ĳ���
		SceneManager.LoadScene(scene);
	}
}

