using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBetwenScene : MonoBehaviour
{
    private static SaveBetwenScene _instance;
    public static SaveBetwenScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveBetwenScene>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveBetwenScene", typeof(SaveBetwenScene));
                }
            }
            return _instance;
        }
    }

    public int characterIndex;
    public PlayerType SelectedPlayer = PlayerType.None;

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
