using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AdvancedSceneLoader : MonoBehaviour
{
    public int mainSceneIndex = 1;
    public int[] additionalScenes;
    public GameObject loadingPanel; // 加载进度UI

    public void StartGame()
    {
        StartCoroutine(LoadScenesWithProgress());
    }

    IEnumerator LoadScenesWithProgress()
    {
        loadingPanel.SetActive(true);

        // 加载主场景
        AsyncOperation mainOp = SceneManager.LoadSceneAsync(mainSceneIndex, LoadSceneMode.Single);
        while (!mainOp.isDone)
        {
            UpdateProgressBar(mainOp.progress);
            yield return null;
        }

        // 并行加载附加场景
        foreach (int sceneIdx in additionalScenes)
        {
            AsyncOperation addOp = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Additive);
            while (!addOp.isDone)
                yield return null;
        }

        loadingPanel.SetActive(false);
    }

    void UpdateProgressBar(float progress)
    {
        // 更新UI进度条逻辑
    }
}