using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _StartGameButton = default;
    [SerializeField] private Button _SettingsButton = default;
    [SerializeField] private Button _ChoosePlayerButton = default;

    public UnityAction StartGameButtonAction;
    public UnityAction SettingsButtonAction;
    public UnityAction ChoosePlayerButtonAction;

    public void StartGameButton()
    {
        StartGameButtonAction.Invoke();
    }

    public void SettingsButton()
    {
        SettingsButtonAction.Invoke();
    }

    public void ChoosePlayerButton()
    {
        ChoosePlayerButtonAction.Invoke();
    }

}