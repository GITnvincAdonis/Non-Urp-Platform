using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private UserInterfaceSO UIEvents;

    [SerializeField] private UIDocument _document;
    [SerializeField] private StyleSheet _styles;

    VisualElement root;
    
    private void Start()
    {

        StartCoroutine(Generate());

    }
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(Generate());
    }
    private IEnumerator Generate()
    {
        yield return null;
        root = _document.rootVisualElement;
        root.Clear();

        root.styleSheets.Add(_styles);
        VisualElement el = createElement("test");
        root.Add(el);



        



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
    // commented out code for testing actions and unity events


    private void FadeToBlack()
    {
        
    }
    private void FadeFromBlack()
    {
        
    }
    private void OnEnable()
    {

        UIEvents.fadeInEvent.AddListener(FadeToBlack);
        UIEvents.fadeOutEvent.AddListener(FadeFromBlack);

        UIEvents.RegisterListener(gameObject);
    }
    private void OnDisable()
    {

        UIEvents.fadeInEvent.RemoveListener(FadeToBlack);
        UIEvents.fadeOutEvent.RemoveListener(FadeFromBlack);

        UIEvents.UnregisterListener(gameObject);
    }
}
