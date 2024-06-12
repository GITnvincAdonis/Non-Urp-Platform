using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static MainMenuScreen;

public class LEVELSelect : MonoBehaviour
{
    [SerializeField] private StyleSheet _selectMenuStyles;
    VisualElement[] levelContainers = new VisualElement[5];
    string[] labelTexts = new string[5];
    string Lvl;


    [SerializeField] private MenuState state;
    [SerializeField] private UserInterfaceSO UIEvents;
    [SerializeField] private LvlSwitchSO lvlEvents;
    VisualElement root;
    [SerializeField] private UIDocument _document;
    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(GenerateMenuSelect());
        
    }
    private void OnValidate()
    {
        if(Application.isPlaying)return;
        StartCoroutine(GenerateMenuSelect());

        
    }
    // Update is called once per frame

    private IEnumerator GenerateMenuSelect()
    {

        yield return null;
        root = _document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(_selectMenuStyles);
        
        VisualElement levelContainer = createElement("lvl-container");
        
        for (int i = 0; i < levelContainers.Length; i++)
        {
            Label text = new();
            text.text = "LEVEL" + (i + 1);
            text.AddToClassList("text");
            text.AddToClassList("text-" + (i + 1));

            levelContainers[i] = createElement("levels", "level-" + i);
            

            levelContainers[i].Insert(0, text);
            levelContainer.Insert(0, levelContainers[i]);

        }
        levelContainers[0].RegisterCallback<ClickEvent>((ClickEvent) => {clickEvent(0);});
        levelContainers[1].RegisterCallback<ClickEvent>((ClickEvent) => {clickEvent(1);});
        levelContainers[2].RegisterCallback<ClickEvent>((ClickEvent) => {clickEvent(2);});
        levelContainers[3].RegisterCallback<ClickEvent>((ClickEvent) => {clickEvent(3);});
        levelContainers[4].RegisterCallback<ClickEvent>((ClickEvent) => {clickEvent(4);});

        root.Add(levelContainer);
        delay();

    }
    void clickEvent(int i)
    {

        MyDelay(i);
        Debug.Log("Clicked");
    }
    async Task MyDelay(int i)
    {

        if (Lvl != lvlEvents.gameSceneToSwitchTo) UIEvents.FadeInEventRaiser();
        await Task.Delay(1000);


        
        if (i==0) lvlEvents.RoomEventRaiser();
        if (i== 1) lvlEvents.LevelOneEventRaiser();
        if (i==2) lvlEvents.LevelOneEventRaiser();
        if (i==3) lvlEvents.LevelOneEventRaiser();
        //else lvlEvents.LevelOneEventRaiser();
        
    }
    async Task delay()
    {
        
        await Task.Delay(1000);
        root.ToggleInClassList("visible");
    }
    VisualElement createElement(params string[] classNames)
    {

        var element = new VisualElement();
        for (int i = 0; i < classNames.Length; i++)
        {
            
            element.AddToClassList(classNames[i]);
        }
        return element;

    }
    void AddImage(VisualElement element, Texture2D src)
    {
        if (element == null) return;

        element.style.backgroundImage = src;
    }


    private void FadeToBlack()
    {
        root.AddToClassList("visible");
    }
    private void FadeFromBlack()
    {
        if (root.ClassListContains("visible")) root.RemoveFromClassList("visible");
    }
    private void retrieveLvl(string level)
    {
        Lvl = level;
    }
    private void OnEnable()
    {

        lvlEvents.switchedEvent.AddListener(retrieveLvl);

        UIEvents.fadeInEvent.AddListener(FadeToBlack);
        UIEvents.fadeOutEvent.AddListener(FadeFromBlack);

        UIEvents.RegisterListener(gameObject);
    }
    private void OnDisable()
    {
        lvlEvents.switchedEvent.RemoveListener(retrieveLvl);

        UIEvents.fadeInEvent.RemoveListener(FadeToBlack);
        UIEvents.fadeOutEvent.RemoveListener(FadeFromBlack);

        UIEvents.UnregisterListener(gameObject);
    }
}
