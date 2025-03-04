using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SceneLoaderEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    /*
     场景加载请求
     */
    //"loctionToLoad"=要加载的场景
    //"posToGo"=Player目的地
    //"fadeScreen"=是否渐入渐出
    public void RalseLoadRequestEvent(GameSceneSO locationToLoad,Vector3 posToGo,bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}