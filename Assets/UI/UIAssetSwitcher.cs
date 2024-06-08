using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MainMenuScreen mainMenuScreen;
    [SerializeField] private PauseMenuScript pauseMenu;
    [SerializeField] PauseManager pauseManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused)
        {
            if (mainMenuScreen.enabled)
            {
                mainMenuScreen.enabled = false;
                
            }
            pauseMenu.enabled = true;
        }
        else {

            if (pauseMenu.enabled) { 
                pauseMenu.enabled = false;
            }
            mainMenuScreen.enabled=true;
        }
    }
}
