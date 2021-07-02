using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Door : MonoBehaviour
{
    public delegate string NotifyXD();
    public static event NotifyXD OnxD;
    public string lol;
    TextMeshPro text;
    SpriteRenderer spriteRenderer;

    public Color[] xd = new Color[3];
    public int color = 0;
    private void OnDisable()
    {
        PlayerController.ondoorenter -= EnterDoorxd;
        PlayerController.ondoorexit -= ExitDoorxd;

    }

    private void OnEnable()
    {
       PlayerController.ondoorenter += EnterDoorxd;
        PlayerController.ondoorexit += ExitDoorxd;

    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lol = (string)OnxD?.Invoke();
        SetDoorText(lol);
        color = Random.Range(0, 3);
        SetDoorColor(xd[color]);
    }
    void EnterDoorxd(int color,string id)
    {
        if (color == this.color)
            SetDoorColor(Color.black);
    }
    void ExitDoorxd(int color, string id)
    {
        if (color == this.color)
            SetDoorColor(xd[color]);
        if(id == lol)
        {
            color++;
            if(color > 2) color = 0 ;
            SetDoorColor(xd[color]);
        }
    }

    void SetDoorColor(Color color)
    {
        spriteRenderer.color = color;
    }
    void SetDoorText(string Danktext)
    {

        text.SetText(Danktext);
    }

}
