using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerLocal : MonoBehaviour
{
    public static SceneManagerLocal script;

    public LvlSwitchSO sceneSwitcher;
    public WinConditionSO winSO;
    string currentLevel;

    [SerializeField] UserInterfaceSO UIevents;
    bool isPlaying =  false;
    void Awake()
    {
        currentLevel = sceneSwitcher.gameSceneToSwitchTo;
        if (script != null)
        {
            Destroy(gameObject);
            return;
        }
        script = this;
        DontDestroyOnLoad(gameObject);
    }
    

    // Update is called once per frame
    
    
    void OnEnable()
    {
        winSO.LoseEvent.AddListener(LoseState);
        winSO.WinEvent.AddListener(WinState);
        sceneSwitcher.switchedEvent.AddListener(retrieveLvlToSwitch);
    }
    void OnDisable()
    {
        winSO.LoseEvent.RemoveListener(LoseState);
        winSO.WinEvent.RemoveListener(WinState);
        sceneSwitcher.switchedEvent.RemoveListener(retrieveLvlToSwitch);
    }

    public async void WinState()
    {

    }
    AsyncOperation scene;
    
    public async void LoseState()
    {
        UIevents.FadeInEventRaiser();
        await Task.Delay(1000);
        //SceneManager.UnloadSceneAsync(currentLevel);
        string thisLvl = SceneManager.GetActiveScene().name;
        Debug.Log(thisLvl);
        scene = SceneManager.LoadSceneAsync(thisLvl, LoadSceneMode.Single);
     
        await Task.Delay(1000);
        UIevents.FadeOutEventRaiser();
    }
    public async void retrieveLvlToSwitch(string name)
    {
        if (name != currentLevel)
        {
            UIevents.FadeInEventRaiser();
            await Task.Delay(3000);
            var scene = SceneManager.LoadSceneAsync(name);
            currentLevel = name;
            
            UIevents.FadeOutEventRaiser();
        }


    }
}
