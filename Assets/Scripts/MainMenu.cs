using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _levelSelectorMenu;
    [SerializeField] private GameObject _optionMenu;

    public void ToggleLevelSelectorMenu()
    {
        _levelSelectorMenu.SetActive(!_levelSelectorMenu.activeSelf);
    }

    public void ToggleOptionMenu()
    {
        _optionMenu.SetActive(!_optionMenu.activeSelf);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
