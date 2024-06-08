using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    
    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        MenuManager.input.UI.Enable();
        MenuManager.input.MOVES.Disable();
    }
    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        MenuManager.input.UI.Disable();
        MenuManager.input.MOVES.Enable();
    }


}
