using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    private Animator crossfadeAnimator;

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

    // Todo : Change transform of camera to the transform of the previous scene
    void OnActiveSceneChanged(Scene current, Scene next)
    {
        Debug.Log("OnActiveSceneChanged: " + current.name + " -> " + next.name);

        // Get the camera from the current scene
        GameObject _cam = Camera.main.gameObject;
        if (next.name == "GameScene")
        {
            _cam.transform.position = new Vector3(0, 33, 0);
            _cam.transform.rotation = Quaternion.Euler(73, 0, 0);
        }

        if (next.name == "MainMenu")
        {
            _cam.transform.position = new Vector3(50, 40, -15);
            _cam.transform.rotation = Quaternion.Euler(37, 0, 0);
        }

        if (next.name == "CharacterSelectionMenu")
        {
            _cam.transform.position = new Vector3(0, 1, -10);
            _cam.transform.rotation = Quaternion.Euler(0, 0, 0);
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
        Debug.Log("Playing Music : " + AudioManager.Instance.IsPlaying());
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
