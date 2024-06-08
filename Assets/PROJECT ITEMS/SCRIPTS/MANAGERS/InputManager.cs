using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InputManager Instance;
    public static PlayerInput PlayerInput;

    private InputAction _menuOpenActions;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        PlayerInput = GetComponent<PlayerInput>();
        


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
