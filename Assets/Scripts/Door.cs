using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Door : MonoBehaviour
{
    TextMeshPro text;
    SpriteRenderer spriteRenderer;

    
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ejemplo para mostrar como se escribe texto
        SetDoorText("E");
        SetDoorColor(Color.red);
    }

    void SetDoorColor(Color DoorColor)
    {
        spriteRenderer.color = DoorColor;
    }
    void SetDoorText(string textToDisplay)
    {
     
        text.SetText(textToDisplay);
    }

    
}
