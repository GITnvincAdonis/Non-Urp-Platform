using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum SceneName
{
    MenuSelect,
    homeScreen,
    LevelOne,
}

[CreateAssetMenu(fileName = "LevelSwitchSO",menuName = "levelSwitchSO")]
public class LvlSwitchSO : ScriptableObject
{

    public UnityEvent<string> switchedEvent;
    public string gameSceneToSwitchTo;
    
    Dictionary<SceneName, string> names = new Dictionary<SceneName, string>
    {
        { SceneName.MenuSelect, "MenuSelect" },
        { SceneName.homeScreen, "HomeScreen" },
        { SceneName.LevelOne, "LevelOne"},
        // ...
    };
    void OnEnable()
    {
        gameSceneToSwitchTo = null;
        if (switchedEvent == null)
        {
            switchedEvent = new UnityEvent<string>();
        }

    }



    //room switching functions. Call to switch to change the corresponding events
    //  must include:
    // - name of event to switch to (gameSceneToSwitchTo)
    // - spawn position (respawn vector)
    public void MenuSelectEventRaiser()
    {
        
        gameSceneToSwitchTo = names[SceneName.MenuSelect];
        switchedEvent?.Invoke(gameSceneToSwitchTo);

    }
    public void LevelOneEventRaiser()
    {
        gameSceneToSwitchTo = names[SceneName.LevelOne];
        switchedEvent?.Invoke(gameSceneToSwitchTo);
    }

    public void HomeScreenEventRaiser()
    {
        gameSceneToSwitchTo = names[SceneName.homeScreen];
        switchedEvent?.Invoke(gameSceneToSwitchTo);
    }




}