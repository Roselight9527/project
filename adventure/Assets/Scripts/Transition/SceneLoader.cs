using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public ViodEventSO newGameEvent;
    public ViodEventSO backToMenuEvent;

    [Header("广播")]
    public ViodEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadSceneEvent;
    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    [SerializeField] public GameSceneSO currentLoadScene=null;

    public GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    private void Awake()
    {
    }
    private void Start()
    {
        loadEventSO.RalseLoadRequestEvent(menuScene, menuPosition, true);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnloadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnbackMenuEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnloadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnbackMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RalseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }
    private void OnloadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.Fadein(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        unloadSceneEvent.RalseLoadRequestEvent(sceneToLoad,positionToGo,true);
        yield return currentLoadScene.sceneReference.UnLoadScene();
        //关闭人物
        playerTrans.gameObject.SetActive(false);
        //加载新场景
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }
    private void OnLoadCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.Fadeout(fadeDuration);
        }
        isLoading = false;
        if (currentLoadScene.sceneType==SceneType.Loaction) 
        afterSceneLoadedEvent.RaiseEvent();
    }
    private void OnbackMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RalseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }
    public DataDefinition GetdataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if(data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.GetSaveScene();

            OnloadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
