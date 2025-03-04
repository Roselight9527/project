using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // UI ��������������
    public Animator UIAnimator;

    private void Start()
    {
        // �ڴ˴����Գ�ʼ����Ҫ�����ݣ�ĿǰΪ��
    }

    private void Update()
    {
        // ����û��Ƿ��»س������ߵ����Ļ/���������ҵ������ UI Ԫ����
        if (Input.GetKeyDown(KeyCode.Return) ||
            (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()))
        {
            // ����Ǵ������豸���Ҵ����㲻�� UI Ԫ����
            if (!(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
                  EventSystem.current.IsPointerOverGameObject((Input.GetTouch(0).fingerId))))
            {
                // ��ʼ��Ϸ
                StartGame();
            }
        }
    }

    // ��ʼ��Ϸ�Ĺ���
    public void StartGame()
    {
        // ���� UI ����
        //UIAnimator.SetTrigger("Start");
        // �������س�����Э��
        StartCoroutine(LoadScene("Game"));
    }

    // ���س�����Э��
    IEnumerator LoadScene(string scene)
    {
        // �ȴ� 0.6 ��Ĺ��ɶ���ʱ��
        yield return new WaitForSeconds(0.6f);

        // ����ָ���ĳ���
        SceneManager.LoadScene(scene);
    }
}
