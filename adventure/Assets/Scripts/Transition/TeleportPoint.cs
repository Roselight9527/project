using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,Iinteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;

    public void ExitAction()
    {
        
    }

    public void TriggerAction()
    {
        Debug.Log("����");
        loadEventSO.RalseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
