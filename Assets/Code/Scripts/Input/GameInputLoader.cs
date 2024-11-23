using UnityEngine;

public class GameInputLoader : MonoBehaviour
{
    public GameInput gameInput;

    private void Awake()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
        }

        gameInput.Player.Enable(); 
    }
}