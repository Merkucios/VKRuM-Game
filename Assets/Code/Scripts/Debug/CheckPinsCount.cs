using UnityEngine;
using UnityEngine.UI;

public class CheckPinsCount : MonoBehaviour
{
    public Text textField;

    public PinManagerSO pinManagerSO;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pinManagerSO.ResetPins();
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = pinManagerSO.GetTotalPins().ToString();
    }
}
