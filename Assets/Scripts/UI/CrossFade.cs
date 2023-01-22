using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    private Animator crossfadeAnimator;

    private GameObject _camForPreviousScene;

    void Awake()
    {
        crossfadeAnimator = GetComponent<Animator>();
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public IEnumerator LoadSceneCoroutine(string sceneName, bool unloadPreviousScene = true)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);

        // Load the new scene in additive mode, or activate it if it is already loaded
        if (!unloadPreviousScene || sceneName == "GameScene")
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

    }



    private GameObject FindCameraOnScene(Scene scene)
    {
        if (!scene.IsValid()) return null;
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            if (go.GetComponent<Camera>() != null)
                return go;
        }
        return null;
    }

    void OnActiveSceneChanged(Scene current, Scene next)
    {
        Debug.Log("OnActiveSceneChanged: " + current.name + " -> " + next.name);

        // Get the camera from the current scene
        _camForPreviousScene = FindCameraOnScene(current);
        if (_camForPreviousScene != null)
        {
            _camForPreviousScene.SetActive(false);
        }

        // Activate the camera on the new scene
        GameObject _cam = FindCameraOnScene(next);
        if (_cam != null)
        {
            _cam.SetActive(true);
        }

        // Destroy the previous scene only if it is not the GameScene
        if (current.IsValid() && current.name != "GameScene")
        {
            SceneManager.UnloadSceneAsync(current);
        }
        if (current.IsValid() && current.name == "GameScene")
        {
            crossfadeAnimator.SetTrigger("TransitionDone");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        SceneManager.SetActiveScene(scene);
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
