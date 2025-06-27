// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("场景配置")]
    public int mainMenuSceneIndex = 1;
    public int mainGameSceneIndex = 2;
    public int[] additiveScenes = { 3, 4 };

    [Header("持久化对象")]
    public GameObject audioManagerPrefab;

    private GameObject audioManagerInstance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("发现多个GameManager实例！");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager实例已创建");
        Initialize();
    }

    void Initialize()
    {
        if (audioManagerInstance == null && audioManagerPrefab != null)
        {
            audioManagerInstance = Instantiate(audioManagerPrefab);
            DontDestroyOnLoad(audioManagerInstance);
            Debug.Log("音频管理器已创建");
        }

        Debug.Log($"加载主菜单场景: {mainMenuSceneIndex}");
        SceneManager.LoadScene(mainMenuSceneIndex, LoadSceneMode.Additive);
    }

    public void LoadGameScenes()
    {
        Debug.Log("调用LoadGameScenes()");
        StartCoroutine(LoadGameScenesCoroutine());
    }

    private IEnumerator LoadGameScenesCoroutine()
    {
        Debug.Log($"卸载主菜单场景: {mainMenuSceneIndex}");
        SceneManager.UnloadSceneAsync(mainMenuSceneIndex);

        Debug.Log($"加载主游戏场景: {mainGameSceneIndex}");
        AsyncOperation mainLoad = SceneManager.LoadSceneAsync(mainGameSceneIndex, LoadSceneMode.Additive);
        yield return mainLoad;

        Debug.Log($"设置活动场景: {mainGameSceneIndex}");
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(mainGameSceneIndex));

        Debug.Log($"加载附加场景: {string.Join(", ", additiveScenes)}");
        foreach (int sceneIndex in additiveScenes)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }

        Debug.Log("场景加载完成");
        yield break;
    }
}