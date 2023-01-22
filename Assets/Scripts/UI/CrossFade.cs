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
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
