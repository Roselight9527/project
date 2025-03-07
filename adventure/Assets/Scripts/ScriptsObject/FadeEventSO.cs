using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;
  public void Fadein(float duration)//�𽥱��
    {
        RaiseEvent(Color.black, duration, true);
    }
    public void Fadeout(float duration)//��͸��
    {
        RaiseEvent(Color.clear, duration, false);
    }
    public void RaiseEvent(Color target,float duration,bool fadein)
    {
        OnEventRaised?.Invoke(target,duration,fadein);
    }
}
