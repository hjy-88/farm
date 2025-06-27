using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // 添加UI命名空间

public class MultiSceneLoader : MonoBehaviour
{
    public string[] scenesToLoad; // 需加载的场景名数组
    public string sceneToUnload;  // 需卸载的场景名

    // 初始化场景配置
    public void InitializeScenes(string[] scenesToLoad, string sceneToUnload = "")
    {
        this.scenesToLoad = scenesToLoad;
        this.sceneToUnload = sceneToUnload;
    }

    // 开始加载场景（由按钮事件调用）
    public void StartLoadScenes()
    {
        StartCoroutine(LoadScenes());
    }

    IEnumerator LoadScenes()
    {
        // 检查场景配置
        if (scenesToLoad == null || scenesToLoad.Length == 0)
        {
            Debug.LogError("没有指定要加载的场景！");
            yield break;
        }

        // 加载所有新场景（叠加模式）
        AsyncOperation[] operations = new AsyncOperation[scenesToLoad.Length];
        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            operations[i] = SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
            Debug.Log($"开始加载场景: {scenesToLoad[i]}");
        }

        // 等待所有场景加载完成
        foreach (var op in operations)
        {
            while (!op.isDone)
            {
                // 可添加加载进度条更新逻辑
                yield return null;
            }
            Debug.Log("场景加载完成");
        }

        // 卸载旧场景（可选）
        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            Debug.Log($"开始卸载场景: {sceneToUnload}");
            yield return SceneManager.UnloadSceneAsync(sceneToUnload);
            Debug.Log("场景卸载完成");
        }

        // 设置主场景（控制光照/音频等设置）
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenesToLoad[0]));
        Debug.Log($"设置主场景: {scenesToLoad[0]}");
    }
}