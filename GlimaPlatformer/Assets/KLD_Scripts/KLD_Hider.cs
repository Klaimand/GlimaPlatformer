﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KLD_Hider : MonoBehaviour
{
    [SerializeField]
    private float revealSpeed;

    private float alpha;

    //new Renderer renderer;
    Tilemap tileMap;

    Color materialColor;

    private void Awake()
    {
        //renderer = GetComponent<Renderer>();
        tileMap = GetComponent<Tilemap>();
        materialColor = tileMap.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        alpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        alpha += Time.fixedDeltaTime * revealSpeed;
        alpha = Mathf.Clamp01(alpha);
        //renderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);
        tileMap.color = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alpha -= Time.fixedDeltaTime * revealSpeed * 2f;
        }
    }
}
