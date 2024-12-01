using System.Collections;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private UIMainMenu _mainMenuPanel = default;
    
    [SerializeField]
    private VoidEventSO _startGameEvent = default;
    
    private IEnumerator Start()
    {
        _inputReader.EnableMenuInput();
        yield return new WaitForSeconds(0.4f);
        SetMenuScreen();
    }
    
    void SetMenuScreen()
    {
        _mainMenuPanel.StartGameButtonAction += ButtonStartGameClicked;
    }
    
    public void ButtonStartGameClicked()
    {
        _startGameEvent.RaiseEvent();
    }
    
}
