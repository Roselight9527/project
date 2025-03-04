using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "Gmae Scene/SceneLoaderEventSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
}