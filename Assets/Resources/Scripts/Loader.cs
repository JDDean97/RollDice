using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    static GameObject loadScreen;

    public static IEnumerator loadScene(string sceneName)
    {
        loadScreen = Resources.Load<GameObject>("Prefabs/LoadScreen");
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;
        GameObject.Instantiate(loadScreen, GameObject.FindObjectOfType<Canvas>().transform);
        while (asyncOp.progress < 0.9f)
        {
            yield return null;
        }
        asyncOp.allowSceneActivation = true;
    }
}
