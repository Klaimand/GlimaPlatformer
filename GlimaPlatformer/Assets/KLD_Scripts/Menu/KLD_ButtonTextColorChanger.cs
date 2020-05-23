using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_ButtonTextColorChanger : MonoBehaviour
{
    public Color normalColor, selectedColor, pressedColor;



    private Text buttonText;
    private Button thisButton;
    private Image thisButtonImage;

    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButtonImage = GetComponent<Image>();
        buttonText = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    
    
}
