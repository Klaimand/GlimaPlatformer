using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Cigarettes : MonoBehaviour
{

    SpriteRenderer sr;

    [SerializeField]
    Sprite cigarettes, fantomcigarettes;

    [SerializeField]
    bool fantom = false;

    [SerializeField]
    int fantomAlpha = 100;


    private void Awake()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkFantom();
    }

    void checkFantom ()
    {
        if  (fantom)
        {
            sr.sprite = fantomcigarettes;
            sr.color = new Color(1, 1, 1, (float)fantomAlpha/255f);
        }
        else
        {
            sr.sprite = cigarettes;
            sr.color = new Color(1, 1, 1, 1);
        }
    }

}
