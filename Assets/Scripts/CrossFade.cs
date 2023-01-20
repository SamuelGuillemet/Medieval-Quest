using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    [SerializeField]
    private Animator crossfadeAnimator;

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
