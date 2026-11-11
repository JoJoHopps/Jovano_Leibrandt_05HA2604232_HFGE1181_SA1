using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    [SerializeField] private bool showInEditor = false;

    private void Awake()
    {
        bool show = Application.isMobilePlatform || (showInEditor && Application.isEditor);
        gameObject.SetActive(show);
    }
}