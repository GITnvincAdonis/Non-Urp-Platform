using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerLocal : MonoBehaviour
{
    public static SceneManagerLocal script;
    public LvlSwitchSO sceneSwitcher;
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
        sceneSwitcher.switchedEvent.AddListener(retrieveLvlToSwitch);
    }
    void OnDisable()
    {
        sceneSwitcher.switchedEvent.RemoveListener(retrieveLvlToSwitch);
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
