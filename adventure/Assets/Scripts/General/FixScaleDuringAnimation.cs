using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixScaleDuringAnimation : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        // 保存初始的缩放值
        originalScale = transform.localScale;
    }

    void LateUpdate()
    {
        // 在 LateUpdate 中更新缩放值，确保在动画之后执行
        transform.localScale = originalScale;
    }
}
