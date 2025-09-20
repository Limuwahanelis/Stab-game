using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SceneName] public string _sceneToLoad;
    [SerializeField] bool _loadNextScene;
    [SerializeField] bool _loadOnStart;
    private void Start()
    {
        if(_loadOnStart)
        {
            if(_loadNextScene) LoadNextScene();
            else LoadScene();
        }
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
