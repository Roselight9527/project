using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SceneLoaderEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    /*
     ������������
     */
    //"loctionToLoad"=Ҫ���صĳ���
    //"posToGo"=PlayerĿ�ĵ�
    //"fadeScreen"=�Ƿ��뽥��
    public void RalseLoadRequestEvent(GameSceneSO locationToLoad,Vector3 posToGo,bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}