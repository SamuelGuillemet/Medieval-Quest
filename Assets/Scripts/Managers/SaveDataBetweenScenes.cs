using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataBetweenScenes : MonoBehaviour
{
    private static SaveDataBetweenScenes _instance;
    public static SaveDataBetweenScenes Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveDataBetweenScenes>();
                if (_instance == null)
                {
                    new GameObject("SaveDataBetweenScenes", typeof(SaveDataBetweenScenes));
                }
            }
            return _instance;
        }
    }

    public PlayerType SelectedPlayer = PlayerType.None;
    public string PreviousScene;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
