using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] PauseManager pauseManager;
    public static CONTROLSASSET input;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        input = new CONTROLSASSET();

  
        input.MOVES.Pause.canceled += PauseEvent;
       
        input.UI.Unpause.canceled += UnpauseEvent;
        pauseManager.UnpauseGame();
    }
    void PauseEvent(InputAction.CallbackContext context)
    {
        pauseManager.PauseGame();
    }
    void UnpauseEvent(InputAction.CallbackContext context) { 
        pauseManager.UnpauseGame();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        input.MOVES.Enable();
        input.UI.Enable();
    }
    private void OnDisable()
    {
        input.MOVES.Disable();
        input.UI.Disable();
    }
}
