using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
public class IntroScreen : MonoBehaviour
{
    [SerializeField] private StyleSheet _selectMenuStyles;
    [SerializeField] private UIDocument _document;
    VisualElement root;
    [SerializeField] private LvlSwitchSO lvlEvents;
    [SerializeField] private UserInterfaceSO UIEvents;
    string Lvl;
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(GenerateMenu());
    }  
    private void OnValidate()
    {
        StartCoroutine(GenerateMenu());
    }
    private IEnumerator GenerateMenu()
    {

        yield return null;
        root = _document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(_selectMenuStyles);

        Label introLabel = new ();
        Label StartLabel = new ();
        Label ExitLabel = new ();

        VisualElement[] testElement = new VisualElement[3];
        for (int i = 0; i < testElement.Length; i++) {

            testElement[i] = createElement("container", "container-"+(i+1));
            
            root.Add(testElement[i]);

        }

        introLabel.text = "ROBOTO (My Game)";
        introLabel.AddToClassList("introText");

        StartLabel.text = "LEVELS";
        StartLabel.AddToClassList("startLabel");
        ExitLabel.text = "QUIT";
        ExitLabel.AddToClassList("exitLabel");

        testElement[0].Add(introLabel);
        testElement[1].Add(StartLabel);
        testElement[1].RegisterCallback<ClickEvent>(MenuClickEvent);


        testElement[2].Add(ExitLabel);
        testElement[2].RegisterCallback<ClickEvent>((Clickable) =>
        {
            Application.Quit(); 
            Debug.Log("exit");
        });
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
    private void retrieveLvl(string level)
    {
        Lvl = level;
    }
    private void FadeToBlack()
    {
        root.AddToClassList("visible");
    }
    private void FadeFromBlack()
    {
        if (root.ClassListContains("visible")) root.RemoveFromClassList("visible");
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
