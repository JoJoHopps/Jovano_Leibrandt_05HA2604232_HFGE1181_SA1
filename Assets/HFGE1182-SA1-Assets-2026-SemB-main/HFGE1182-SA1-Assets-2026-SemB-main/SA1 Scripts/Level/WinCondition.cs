using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject sceneEnemy;
    
    public string sceneToLoadName;

    private void Update()
    {
        if (sceneEnemy  == null)
        {
            SceneManager.LoadScene(sceneToLoadName);
        }
    
    }
    
}
