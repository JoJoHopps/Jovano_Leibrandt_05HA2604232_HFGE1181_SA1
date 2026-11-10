using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadSelectScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
