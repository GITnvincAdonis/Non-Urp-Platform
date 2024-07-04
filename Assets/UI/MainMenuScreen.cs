using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MainMenuScreen : MonoBehaviour
{

    public enum MenuState
    {
        Paused,
        LevelScreen,
        DeathScreen
        
    }

    string Lvl;
    [SerializeField] private MenuState state;
    [SerializeField] private UserInterfaceSO UIEvents;
    [SerializeField] private LvlSwitchSO lvlEvents;
    [SerializeField] private HealthSOScript healthRef;
    VisualElement root;
    [SerializeField] private UIDocument _document;
   








    //Playing UI references

    [SerializeField] private StyleSheet _playingStyles;
    [SerializeField] private Texture2D HealthIndicatorImg;
    [SerializeField] private Texture2D PanelsImg;
    [SerializeField] private Texture2D speedIcon;

    [SerializeField] private Texture2D CollectableOne;
    [SerializeField] private Texture2D CollectableTwo;
    [SerializeField] private Texture2D CollectableThree;

    Texture2D[] collectables = new Texture2D[3];



    [SerializeField] PauseManager pauseManager;
    Label InstructionContainer;
    VisualElement bar;
    VisualElement innerBar;
    VisualElement speedbar;
    VisualElement[] containers = new VisualElement[4];





    // pause UI references

    [SerializeField] private StyleSheet _pauseMenuStyles;
    VisualElement[] optionContainers = new VisualElement[5];
    string[] optiontext = new string[5];
    VisualElement item;

    Vector3 PlayerPos;




    //MenuSelect UI references




    private void Awake()
    {
        collectables[0] = CollectableOne;
        collectables[1]= CollectableTwo;
        collectables[2]= CollectableThree;



        optiontext[0] = "Return";
        optiontext[1] = "Restart";
        optiontext[2] = "Home";
        optiontext[3] = "levels";
        optiontext[4] = "seetings";
        state = MenuState.LevelScreen;
    }
    private void Start()
    {
        
        StartCoroutine(GeneratePlayingUI());
        
    }
    private void Update()
    {
       
        if (pauseManager.IsPaused == false && state == MenuState.Paused)
        {

            
            state = MenuState.LevelScreen;
            StartCoroutine(GeneratePlayingUI());
            
        }
        else if (pauseManager.IsPaused == true && state == MenuState.LevelScreen)
        {
            
            state = MenuState.Paused;
            StartCoroutine(GenerateMenuUI());

        }
        if(state == MenuState.LevelScreen)
        {
            Vector3 screen = Camera.main.WorldToScreenPoint(PlayerPos);
            speedbar.style.left = screen.x + (speedbar.layout.width/2)-400;
            speedbar.style.top = (Screen.height - screen.y) - 300;

        }
        
       
        
    }
    private void OnValidate()
    {
        StartCoroutine(GenerateMenuUI());
        //Debug.Log("changed");
    }


    private IEnumerator GeneratePlayingUI()
    {
        
        //Debug.Log("player");
        yield return null;
        root = _document.rootVisualElement;
        root.Clear();
        

        root.styleSheets.Add(_playingStyles);

        

        InstructionContainer = new Label();
        InstructionContainer.AddToClassList("instruct-container");
        InstructionContainer.Insert(0, createElement("instruction-image"));
        for (int i = 0; i < containers.Length; i++) {
            VisualElement rendText = new();
            
            containers[i] = createElement("container", "container-"+i);
            AddImage(containers[i], PanelsImg); 
            root.Add(containers[i]);
        }
        bar = createElement("outer-bar");
        innerBar = createElement("inner-bar");
        speedbar = createElement("speed-bar");

        bar.Insert(0, innerBar);
        bar.Insert(1, speedbar);
        AddImage(bar, HealthIndicatorImg);
        var speedIconImg = createElement("speed-Icon");
        AddImage(speedIconImg,speedIcon);
        bar.Insert(2, speedIconImg);
        AddImage(bar, HealthIndicatorImg);

        
        root.Add(bar);
        root.Add(InstructionContainer);
        
        
        
    }
    private void ChangeHealth(int value)
    {
        if (value > 100)
        {
            innerBar.RemoveFromClassList("inner-bar-1");
            innerBar.RemoveFromClassList("inner-bar-2");
            innerBar.RemoveFromClassList("inner-bar-3");

        }
        else if (value < 100 && value >= 75)
        {
            innerBar.AddToClassList("inner-bar-1");
            innerBar.RemoveFromClassList("inner-bar-2");
            innerBar.RemoveFromClassList("inner-bar-3");
        }
        else if (value < 75 && value >= 50)
        {
            innerBar.RemoveFromClassList("inner-bar-1");
            innerBar.AddToClassList("inner-bar-2");
            innerBar.RemoveFromClassList("inner-bar-3");
        }
        else if (value < 50 && value >= 25)
        {
            innerBar.RemoveFromClassList("inner-bar-1");
            innerBar.RemoveFromClassList("inner-bar-2");
            innerBar.AddToClassList("inner-bar-3");
        }
    }
    private void ChangeSpeed(float value, float x, float y, float z)
    {
        PlayerPos.x = x;
        PlayerPos.y = y;
        PlayerPos.z = z;
        speedbar.style.width = (value + 10) * 3;
    }
    private void ChangeBoxOne(Texture2D src)
    {
        VisualElement image = new();
        image.style.backgroundImage = src;
        image.style.width = 140;
        image.style.height = 140;
        containers[0].Insert(0, image);
    }
    private void ChangeBoxTwo(Texture2D scr)
    {
        VisualElement image = new();
        image.style.backgroundImage = scr;
        image.style.width = 140;
        image.style.height = 140;
        containers[1].Insert(0, image);
    }
    private void ChangeBoxThree(Texture2D scr)
    {
       
        VisualElement image = new();
        image.style.backgroundImage = scr;
        image.style.width = 140;
        image.style.height = 140;
        containers[2].Insert(0, image);
    }
    private void ChangeBoxFour(Texture2D scr)
    {
        VisualElement image = new();
        image.style.backgroundImage = scr;
        image.style.width = 100;
        image.style.height = 100;   
        containers[3].Insert(0,image);
        
    }
    private void AppendInstructions(string text)
    {
        string mytext = text;
        DOTween.To(() => InstructionContainer.text, x => InstructionContainer.text = x, mytext, .2f).SetEase(Ease.Linear);
        InstructionContainer.ToggleInClassList("in-view");
    }
    private void RemoveInstructionText()
    {
        InstructionContainer.text = null;
        InstructionContainer.ToggleInClassList("in-view");
    }










   // =============================================================================================================================================================================================================


    private IEnumerator GenerateMenuUI()
    {
        //Debug.Log("menu");
        yield return null;
        root = _document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(_pauseMenuStyles);
        item = createElement("test");
        VisualElement optionMegaContainer = createElement("mega-Image-Container");

        for (int i = 0; i < optionContainers.Length; i++)
        {
            optionContainers[i] = createElement("option-container", "option-container-" + i);

            Label text = new Label();
            text.AddToClassList("text");
            text.text = optiontext[i];
            optionContainers[i].Insert(0, text);
            optionContainers[i].RegisterCallback<ClickEvent>(MenuClickEvent);
            optionMegaContainer.Insert(0,optionContainers[i]);

        }
        VisualElement renderTextureContainter = createElement("render-texture");  
        item.Insert(0, optionMegaContainer);
        item.Insert(1, renderTextureContainter);
        root.Add(item);
    }

    void MenuClickEvent(ClickEvent evt)
    {
        MyDelay();
        Debug.Log("clicked");
    }
    async Task MyDelay()
    {

        if (Lvl != lvlEvents.gameSceneToSwitchTo) UIEvents.FadeInEventRaiser();
        await Task.Delay(1000);
        Debug.Log("left");
        lvlEvents.MenuSelectEventRaiser();
    }




    // =============================================================================================================================================================================================================




    VisualElement createElement(params string[] classNames)
    {

        var element = new VisualElement();
        for (int i = 0;i< classNames.Length; i++)
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
    // commented out code for testing actions and unity events
    private void FadeToBlack()
    {
        root.AddToClassList("visible");
    }
    private void FadeFromBlack()
    {
        if(root.ClassListContains("visible")) root.RemoveFromClassList("visible");
    }
    private void retrieveLvl(string level)
    {
        Lvl = level;
    }

    private void OnEnable()
    {

        lvlEvents.switchedEvent.AddListener(retrieveLvl);

        UIEvents.InstructionEvent.AddListener(AppendInstructions);
        UIEvents.ExitEvent.AddListener(RemoveInstructionText);

        UIEvents.UpdateHealthBarEvent.AddListener(ChangeHealth);
        UIEvents.UpdateSpeedEvent.AddListener(ChangeSpeed);

        UIEvents.UpdateContainerEvent1.AddListener(ChangeBoxOne);
        UIEvents.UpdateContainerEvent2.AddListener(ChangeBoxTwo);
        UIEvents.UpdateContainerEvent3.AddListener(ChangeBoxThree);
        UIEvents.UpdateContainerEvent4.AddListener(ChangeBoxFour);
        UIEvents.fadeInEvent.AddListener(FadeToBlack);
        UIEvents.fadeOutEvent.AddListener(FadeFromBlack);

        UIEvents.RegisterListener(gameObject);
    }
    private void OnDisable()
    {
        lvlEvents.switchedEvent.RemoveListener(retrieveLvl);

        UIEvents.InstructionEvent.RemoveListener(AppendInstructions);
        UIEvents.ExitEvent.RemoveListener(RemoveInstructionText);

        UIEvents.UpdateHealthBarEvent.RemoveListener(ChangeHealth);
        UIEvents.UpdateSpeedEvent.RemoveListener(ChangeSpeed);

        UIEvents.UpdateContainerEvent1.RemoveListener(ChangeBoxOne);
        UIEvents.UpdateContainerEvent2.RemoveListener(ChangeBoxTwo);
        UIEvents.UpdateContainerEvent3.RemoveListener(ChangeBoxThree);
        UIEvents.UpdateContainerEvent4.RemoveListener(ChangeBoxFour);
        UIEvents.fadeInEvent.RemoveListener(FadeToBlack);
        UIEvents.fadeOutEvent.RemoveListener(FadeFromBlack);

        UIEvents.UnregisterListener(gameObject);
    }

}
