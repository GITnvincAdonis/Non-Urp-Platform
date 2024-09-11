using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="UIEvents",menuName ="UIEvents")]
public class UserInterfaceSO : ScriptableObject
{
    public UnityEvent<string, Texture2D> InstructionEvent;
    public string InstructionText;
    public Texture2D InstructionImage;

    public UnityEvent ExitEvent;



    public UnityEvent<int> UpdateHealthBarEvent;
    public UnityEvent<float, float, float , float > UpdateSpeedEvent;
    public UnityEvent<Texture2D> UpdateContainerEvent1;
    public UnityEvent<Texture2D> UpdateContainerEvent2; 
    public UnityEvent<Texture2D> UpdateContainerEvent3;
    public UnityEvent<Texture2D> UpdateContainerEvent4;

    public int healthAmount;
    public float speedValue;
    public float[] position = new float[3];
    public Texture2D containerOneImg;
    public Texture2D containerTwoImg;
    public Texture2D containerThreeImg;
    public Texture2D containerFourImg;



    public UnityEvent fadeInEvent;
    public UnityEvent fadeOutEvent;

    [SerializeField] private List<GameObject> listeners = new ();


    private void OnEnable()
    {
        if (InstructionEvent == null) InstructionEvent = new UnityEvent<string, Texture2D>();
        if (ExitEvent == null) ExitEvent = new UnityEvent();
        if (UpdateHealthBarEvent == null) UpdateHealthBarEvent = new UnityEvent<int>();
        if (UpdateSpeedEvent == null) UpdateSpeedEvent = new UnityEvent<float, float, float, float>();
        if (UpdateContainerEvent1 == null) UpdateContainerEvent1 = new UnityEvent<Texture2D>();
        if (UpdateContainerEvent2 == null) UpdateContainerEvent2 = new UnityEvent<Texture2D>();
        if (UpdateContainerEvent3 == null) UpdateContainerEvent3 = new UnityEvent<Texture2D>();
        if (UpdateContainerEvent4 == null) UpdateContainerEvent4 = new UnityEvent<Texture2D>();
        if (fadeInEvent == null) fadeInEvent = new UnityEvent();
        if (fadeOutEvent == null) fadeOutEvent = new UnityEvent();
        
    }

    public void RegisterListener(GameObject listener)
    {

        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
    public void UnregisterListener(GameObject listener) { 
    
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public void AddTextEventRaiser(string text, Texture2D image)
    {
        InstructionText = text;
        InstructionImage = image;
        InstructionEvent.Invoke(InstructionText, InstructionImage);
    }
    public void RemoveTextEventRaiser() {
        ExitEvent.Invoke();
    }


    public void ChangeHealthBar(int health) { 
        healthAmount = health;
        UpdateHealthBarEvent.Invoke(healthAmount);
    }
    public void Resethealth()
    {
        healthAmount = 100;
    }
    public void ChangeSpeedBar(float speed, float x, float y, float z) {
        position[0] = x;
        position[1] = y;
        position[2] = z;

        speedValue = speed;
        UpdateSpeedEvent.Invoke(speedValue, position[0], position[1], position[2]);
    }
    public void ChangeContOneEventRaiser(Texture2D text)
    {
        containerOneImg = text;
        UpdateContainerEvent1.Invoke(containerOneImg);
    }
    public void ChangeContTwoEventRaiser(Texture2D text)
    {
        containerTwoImg = text;
        UpdateContainerEvent2.Invoke(containerTwoImg);
    }
    public void ChangeContThreeEventRaiser(Texture2D text)
    {
        containerThreeImg = text;
        UpdateContainerEvent3.Invoke(containerThreeImg);
    }
    void ChangeContFourEventRaiser(Texture2D text)
    {
        containerFourImg = text;
        UpdateContainerEvent4.Invoke(containerFourImg);
    }
    public void FadeInEventRaiser()
    {
        fadeInEvent.Invoke();
    }
    public void FadeOutEventRaiser() {
        fadeOutEvent.Invoke(); 
    }



}
